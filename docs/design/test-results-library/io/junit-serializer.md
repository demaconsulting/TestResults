# JUnitSerializer

The `JUnitSerializer` class reads and writes test result data in the JUnit XML format.
It operates on the model layer types and has no knowledge of how test results are
produced or consumed beyond the JUnit XML structures it reads and writes.

## JUnit XML Format

JUnit XML is a widely-adopted, cross-platform test result format supported by many
CI/CD systems including Jenkins, GitLab CI, GitHub Actions, and CircleCI.

This satisfies requirements `TestResults-JUnit-Serialize` and `TestResults-JUnit-Deserialize`.

### JUnit Document Structure

A JUnit document has the following top-level structure:

```text
testsuites
└── testsuite (one per unique class name)
    └── testcase (one per test case)
        ├── failure   (present when outcome is Failed)
        ├── error     (present when outcome is Error, Timeout, or Aborted)
        ├── skipped   (present when outcome is not executed)
        ├── system-out (present when SystemOutput is non-empty)
        └── system-err (present when SystemError is non-empty)
```

A document with a single test suite may use `testsuite` as the root element directly,
omitting the `testsuites` wrapper. Both forms are handled transparently during
deserialization.

### JUnit Serialization

When serializing a `TestResults` object to JUnit XML:

- The `testsuites` root element receives a `name` attribute set from `TestResults.Name`
- Test results are **grouped by `ClassName`** into `testsuite` elements under a
  `testsuites` root
- Each `testsuite` carries `name`, `tests`, `failures`, `errors`, `skipped`,
  `time` (total duration in seconds), and `timestamp` (ISO 8601 UTC start time,
  formatted with a trailing `Z`, of the earliest test in the suite) aggregate attributes
- Each `TestResult` is written as a `testcase` element with `name`, `classname`,
  and `time` (duration in seconds) attributes
- A `failure` child element is written when `Outcome` is `Failed`; the `message`
  attribute is set from `ErrorMessage` when non-empty, and the element text content
  is set from `ErrorStackTrace` when non-empty
- An `error` child element is written when `Outcome` is `Error`, `Timeout`, or
  `Aborted`; the `message` attribute and text content follow the same convention as
  `failure`
- A `skipped` child element is written when `IsExecuted()` returns `false`; if
  `ErrorMessage` is non-empty it is written as a `message` attribute on the element
- Standard output is written to `system-out` and standard error to `system-err`
  child elements if the respective properties are non-empty

### JUnit Deserialization

When deserializing a JUnit document to a `TestResults` object:

- Each `testsuite` element carries an optional `timestamp` attribute (ISO 8601 UTC); when
  present it is used as the `StartTime` for all test cases in that suite; when absent or
  malformed, `StartTime` defaults to the time of deserialization
- Each `testcase` element is mapped to a `TestResult`
- `Name` and `ClassName` are read from the `name` and `classname` attributes
- `Duration` is read from the `time` attribute (seconds as a decimal)
- The presence of a `failure` child element sets `Outcome` to `Failed` and populates
  `ErrorMessage` and `ErrorStackTrace`
- The presence of an `error` child element sets `Outcome` to `Error` and populates
  `ErrorMessage` and `ErrorStackTrace`
- The presence of a `skipped` child element sets `Outcome` to `NotExecuted`
- If none of the above child elements are present, `Outcome` is set to `Passed`
- `SystemOutput` and `SystemError` are read from `system-out` and `system-err` child
  elements if present

### JUnit Round-Trip Fidelity

JUnit XML does not have distinct elements for every `TestOutcome` value, so two known
fidelity limitations apply when round-tripping through JUnit. This satisfies requirement
`TestResults-Serializer-RoundTrip` for JUnit, subject to these limitations:

- **`Timeout` and `Aborted` outcomes are not preserved.** Both are serialized as an
  `error` child element (since JUnit has no distinct timeout or aborted element), and
  deserialize back as `TestOutcome.Error`. Callers that require precise outcome
  preservation should use TRX.

- **`ClassName = "DefaultSuite"` round-trips with an empty `ClassName`.** Tests without
  a class name are grouped under the sentinel value `"DefaultSuite"` during
  serialization. On deserialization, that sentinel is mapped back to an empty string.
  Therefore a test whose `ClassName` is literally `"DefaultSuite"` will lose its class
  name after a JUnit round-trip.
