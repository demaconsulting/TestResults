### JUnitSerializer

#### Purpose

The JUnitSerializer unit converts between the shared in-memory model and JUnit XML test
result documents. It adapts the richer local model to the narrower JUnit schema by grouping
results into suites, projecting outcomes onto JUnit elements, and recovering as much metadata
as the format provides when reading JUnit input.

#### Data Model

**DefaultSuiteName**: `string` - Sentinel suite name used when a `TestResult` has an empty
`ClassName`.

**TimeFormatString**: `string` - Fixed numeric format used when emitting durations in seconds
with three decimal places.

**InvalidJUnitFileMessage**: `string` - Shared error text used when the XML document has no
root element.

**MessageAttributeName**: `string` - Attribute name reused for `failure`, `error`, and
`skipped` message text.

#### Key Methods

**Serialize**: Writes a `TestResults` model as JUnit XML.

- *Parameters*: `TestResults results` - Run metadata and ordered test results to serialize.
- *Returns*: `string` - JUnit XML text with a UTF-8 declaration.
- *Preconditions*: `results` must be non-null.
- *Postconditions*: Emits a `testsuites` root containing one `testsuite` per class name and
  one `testcase` per `TestResult`.

The method groups tests by `ClassName`, uses the earliest `StartTime` in each suite as the
suite timestamp, writes failure details for `Failed`, writes error details for `Error`,
`Timeout`, and `Aborted`, and writes `skipped` for outcomes where `IsExecuted()` is false.

**Deserialize**: Reads JUnit XML into the shared model.

- *Parameters*: `string junitContents` - JUnit XML text to parse.
- *Returns*: `TestResults` - Populated run model.
- *Preconditions*: `junitContents` must be non-null and non-whitespace.
- *Postconditions*: Returns one `TestResult` per `testcase`, using suite timestamps when
  present and populating output, error, and skip fields from child elements.

The method accepts both `testsuites` and single `testsuite` roots so callers can ingest the
common JUnit variants emitted by different tools.

**ParseOutcome**: Maps JUnit child elements to the shared outcome model.

- *Parameters*: `XElement testCaseElement` - `testcase` element to inspect.
- *Returns*: `(TestOutcome outcome, string errorMessage, string errorStackTrace)` - Outcome and
  attached failure information.
- *Preconditions*: `testCaseElement` must represent one JUnit testcase element.
- *Postconditions*: `failure` maps to `Failed`, `error` maps to `Error`, `skipped` maps to
  `NotExecuted`, and a testcase with no outcome element maps to `Passed`.

The method concentrates the JUnit-to-model projection in one place so Deserialize
behavior stays consistent with round-trip tests.

**CreateTestSuiteElement**: Builds a `testsuite` XElement for one class-name group.

- *Parameters*: `IGrouping<string, TestResult> suiteGroup` - All test results sharing one class
  name.
- *Returns*: `XElement` - A valid `testsuite` element with child `testcase` elements and summary
  counters.
- *Preconditions*: `suiteGroup` must be non-null and contain at least one element.
- *Postconditions*: Returns a `testsuite` element whose `name` attribute uses `DefaultSuiteName`
  when the class name is empty; whose `tests`, `failures`, `errors`, `skipped`, and `time` (total
  suite duration in seconds, formatted to 3 decimal places) attributes reflect the grouped results;
  whose `timestamp` attribute is set to the earliest
  `StartTime` across all test cases in the group (formatted as UTC ISO 8601 with Z suffix); and
  whose child `testcase` elements are produced by `CreateTestCaseElement`.

The method uses `Min(StartTime)` over the group to produce a suite-level timestamp that
represents the earliest test start, matching the convention used by common JUnit producers.

**ParseTestCase**: Converts a JUnit `testcase` element into a `TestResult`.

- *Parameters*: `XElement testCaseElement` - The `testcase` element to parse; `DateTime? startTime`
  - Suite-level start time from the enclosing `testsuite` element, or `null` if absent.
- *Returns*: `TestResult` - Populated model object.
- *Preconditions*: `testCaseElement` must be non-null.
- *Postconditions*: Returns a `TestResult` with `Name`, `ClassName`, `Duration`, `Outcome`,
  `ErrorMessage`, `ErrorStackTrace`, `SystemOutput`, and `SystemError` populated from the element;
  `StartTime` is set to `startTime` when the parameter is non-null, otherwise left at the
  `TestResult` default; the `DefaultSuiteName` sentinel classname is mapped back to an empty
  string so round-trips produce the original `ClassName` value.

#### Outcome Mapping

JUnit XML supports only three distinct outcome states — `failure`, `error`, and `skipped` child
elements, with absence of any child element meaning the test passed. The table below documents how
every `TestOutcome` value maps through JUnit serialization and deserialization. Entries marked
**lossy** do not survive a round-trip.

| `TestOutcome` | Serializes to JUnit element | Deserializes back as | Fidelity |
| --- | --- | --- | --- |
| `Passed` | *(no child element)* | `Passed` | Preserved |
| `PassedButRunAborted` | *(no child element)* | `Passed` | **Lossy** |
| `Warning` | *(no child element)* | `Passed` | **Lossy** |
| `Inconclusive` | *(no child element)* | `Passed` | **Lossy** |
| `Disconnected` | *(no child element)* | `Passed` | **Lossy** |
| `Completed` | *(no child element)* | `Passed` | **Lossy** |
| `InProgress` | *(no child element)* | `Passed` | **Lossy** |
| `Failed` | `failure` element | `Failed` | Preserved |
| `Error` | `error` element | `Error` | Preserved |
| `Timeout` | `error` element | `Error` | **Lossy** |
| `Aborted` | `error` element | `Error` | **Lossy** |
| `NotExecuted` | `skipped` element | `NotExecuted` | Preserved |
| `NotRunnable` | `skipped` element | `NotExecuted` | **Lossy** |
| `Pending` | `skipped` element | `NotExecuted` | **Lossy** |

The grouping logic mirrors `TestOutcomeExtensions`: `Failed` maps to a `failure` element;
`Error`, `Timeout`, and `Aborted` (the outcomes where `IsErrorOutcome` returns true) map to an
`error` element; outcomes where `IsExecuted()` returns false (`NotRunnable`, `NotExecuted`,
`Pending`) map to a `skipped` element; all remaining executed outcomes that are not `Failed` or
error-class produce a plain `testcase` with no child outcome element, which reads back as `Passed`.

#### Error Handling

`Serialize()` throws `ArgumentNullException` when `results` is null. `Deserialize()` throws
`ArgumentNullException` or `ArgumentException` for null or whitespace input and throws
`InvalidOperationException` only when the XML document has no root element. Missing or
malformed durations fall back to `TimeSpan.Zero`. Missing or malformed suite timestamps leave
`StartTime` at the default value created by `TestResult`. Because JUnit has no dedicated
`Timeout` or `Aborted` element, those outcomes deserialize back as `TestOutcome.Error`. The
`DefaultSuiteName` sentinel also means a literal class name of `DefaultSuite` round-trips back
to an empty class name. Because JUnit has no inconclusive element, `Inconclusive` tests
serialize as plain `testcase` elements, making them indistinguishable from `Passed` outcomes
on deserialization; this round-trip loss is a known limitation of the JUnit format.

#### Dependencies

- **TestResults** - provides run metadata for the `testsuites` root and receives
  deserialized runs.
- **TestResult** - provides per-test data and receives deserialized testcase data.
- **TestOutcome** - supplies outcome values and execution classification helpers.
- **SerializerHelpers** - provides `Utf8StringWriter` for UTF-8 XML output.
- **System.Xml.Linq** - constructs and queries JUnit XML.
- **System.Globalization** - formats and parses timestamps and durations using invariant
  culture.

#### Callers

- **Serializer** - delegates JUnit deserialization after format identification.
- **Library consumers** - call the static serialize and deserialize entry points directly when
  they already know the content is JUnit XML.
