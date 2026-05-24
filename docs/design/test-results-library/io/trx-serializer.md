### TrxSerializer

#### Purpose

The TrxSerializer unit converts between the shared in-memory model and Microsoft TRX test
result documents. It is responsible for preserving the richer TRX metadata model, including
run metadata, test-definition cross-references, output streams, and summary counters.

#### Data Model

**TrxNamespace**: `XNamespace` - Fixed TRX namespace used for element construction and lookup.

**TestTypeGuid**: `string` - Standard GUID written to each `UnitTestResult/@testType` to mark
unit-test entries.

**TestListId**: `string` - Standard GUID written to `TestEntry` and `TestList` elements for the
`All Loaded Results` list.

**TestListName**: `string` - Human-readable name of the standard TRX test list.

**InvalidTrxFileMessage**: `string` - Shared error text used when the input TRX structure is not
self-consistent.

**DateTimeFormatString**: `string` - ISO 8601 round-trip format string (`"o"`) used to format
and parse TRX timestamps.

**DurationFormatString**: `string` - Constant/invariant format string (`"c"`) used to format
and parse TRX durations as TimeSpan values.

#### Key Methods

**Serialize**: Writes a `TestResults` model as TRX XML.

- *Parameters*: `TestResults results` - Run metadata and ordered test results to serialize.
- *Returns*: `string` - TRX XML text with a UTF-8 declaration.
- *Preconditions*: `results` must be non-null.
- *Postconditions*: Emits a `TestRun` document containing `Results`, `TestDefinitions`,
  `TestEntries`, `TestLists`, and `ResultSummary` sections that reflect the supplied model.

The method builds the TRX tree from helper methods so every `TestResult` is represented in
both the execution section and the definition section, preserving the TRX cross-reference
structure.

**Known limitation**: `ResultSummary/Counters` emits only `total`, `executed`, `passed`, and
`failed` counter attributes. Extended counter attributes such as `error`, `timeout`,
`inconclusive`, and `notExecuted` are not emitted. This is a known limitation of the current
serialization implementation.

**Deserialize**: Reads TRX XML into the shared model.

- *Parameters*: `string trxContents` - TRX XML text to parse.
- *Returns*: `TestResults` - Populated run model.
- *Preconditions*: `trxContents` must be non-null and non-whitespace.
- *Postconditions*: Returns run metadata plus one `TestResult` per `UnitTestResult`, with
  names and code metadata resolved from matching `UnitTest` definitions.

The method builds a lookup from `UnitTest/@id` to `TestMethod` so `UnitTestResult/@testId`
resolution is linear rather than repeatedly scanning the XML document.

**ParseTestOutcome**: Normalizes TRX outcome text to a defined `TestOutcome` member.

- *Parameters*: `string? value` - Raw `outcome` attribute value.
- *Returns*: `TestOutcome` - Parsed outcome, or `TestOutcome.Failed` when parsing fails.
- *Preconditions*: None.
- *Postconditions*: Always returns a defined enum member.

The method accepts named values and defined numeric values, then falls back to `Failed` so
callers never receive an undefined `TestOutcome` from TRX deserialization.

#### Error Handling

`Serialize()` throws `ArgumentNullException` when `results` is null. `Deserialize()` throws
`ArgumentNullException` or `ArgumentException` for null or whitespace input, and throws
`InvalidOperationException` when the TRX structure is inconsistent, including duplicate
`UnitTest/@id` values or `UnitTestResult` records that reference a missing test definition.
Malformed or missing GUIDs fall back to newly generated GUIDs, malformed durations fall back
to `TimeSpan.Zero`, malformed timestamps fall back to `DateTime.UtcNow`, and unrecognized
outcome values fall back to `TestOutcome.Failed`.

#### Dependencies

- **TestResults** - provides run-level metadata for `TestRun` and receives deserialized runs.
- **TestResult** - provides per-test execution data and receives deserialized test cases.
- **TestOutcome** - supplies outcome values and summary classification helpers.
- **SerializerHelpers** - provides `Utf8StringWriter` for UTF-8 XML output.
- **System.Xml.Linq** and **System.Xml.XPath** - construct and query TRX XML.
- **System.Globalization** - formats and parses invariant timestamps and durations.

#### Callers

- **Serializer** - delegates TRX deserialization after format identification.
- **Library consumers** - call the static serialize and deserialize entry points directly when
  they already know the content is TRX.
