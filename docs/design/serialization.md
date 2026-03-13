# Serialization

## Overview

The serialization layer is responsible for reading and writing test result data in the
two formats supported by the library: TRX (Visual Studio Test Results) and JUnit XML.
It is built on top of the model layer and has no knowledge of how test results are
produced or consumed beyond the XML structures it reads and writes.

The serialization layer consists of four types:

- `TestResultFormat` — an enumeration identifying the file format
- `Serializer` — a facade that auto-detects format and delegates to the correct serializer
- `TrxSerializer` — reads and writes TRX XML files
- `JUnitSerializer` — reads and writes JUnit XML files

## TestResultFormat Enumeration

The `TestResultFormat` enumeration identifies the file format of a test result document.

| Value     | Description                                                       |
|-----------|-------------------------------------------------------------------|
| `Unknown` | Format could not be determined                                    |
| `Trx`     | Visual Studio Test Results (TRX) XML format                       |
| `JUnit`   | JUnit XML format                                                  |

## Format Identification

The `Serializer.Identify()` method determines the format of a serialized test result
document without fully deserializing it. This satisfies requirement
`TestResults-Ser-FormatIdentify`.

The identification algorithm:

1. Returns `TestResultFormat.Unknown` immediately if the input is null or whitespace
2. Attempts to parse the full XML document; returns `TestResultFormat.Unknown` if
   parsing fails with an `XmlException` (the input is not valid XML)
3. Returns `TestResultFormat.Trx` if the root element name is `TestRun` **and** the
   namespace URI is `http://microsoft.com/schemas/VisualStudio/TeamTest/2010`
4. Returns `TestResultFormat.JUnit` if the root element name is `testsuites` or
   `testsuite` (case-sensitive, no namespace required)
5. Returns `TestResultFormat.Unknown` for any other document structure

Using the XML namespace for TRX detection makes identification unambiguous — a document
with a `TestRun` root element in any other namespace is not treated as TRX.

## TRX Format

TRX is the native test result format for Visual Studio and Azure DevOps. It is an XML
format with the namespace `http://microsoft.com/schemas/VisualStudio/TeamTest/2010`.

This satisfies requirements `TestResults-Trx-Serialize` and `TestResults-Trx-Deserialize`.

### TRX Document Structure

A TRX document has the following top-level structure:

```text
TestRun (xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010")
├── Results
│   └── UnitTestResult (one per test case)
│       └── Output
│           ├── StdOut
│           ├── StdErr
│           └── ErrorInfo
│               ├── Message
│               └── StackTrace
├── TestDefinitions
│   └── UnitTest (one per test case)
│       ├── Execution
│       └── TestMethod
├── TestEntries
│   └── TestEntry (one per test case)
├── TestLists
│   └── TestList (at least one, the default list)
└── ResultSummary
    └── Counters
```

### TRX Serialization

When serializing a `TestResults` object to TRX:

- The `TestRun` element receives the `id`, `name`, and `runUser` attributes from
  `TestResults.Id`, `TestResults.Name`, and `TestResults.UserName`
- Each `TestResult` is written as a `UnitTestResult` element under `Results`, with
  attributes for `testId`, `executionId`, `testName`, `computerName`, `startTime`,
  `endTime`, `duration`, `outcome`, `testType` (fixed GUID identifying the unit test
  type), and `testListId` (referencing the standard "All Loaded Results" test list)
- An `Output` element is always written for each `UnitTestResult`; its child
  elements `StdOut`, `StdErr`, and `ErrorInfo` are written conditionally
- Standard output is written to `Output/StdOut` if `SystemOutput` is non-empty
- Standard error is written to `Output/StdErr` if `SystemError` is non-empty
- Error information is written to `Output/ErrorInfo/Message` and
  `Output/ErrorInfo/StackTrace` if `ErrorMessage` or `ErrorStackTrace` is non-empty
- A corresponding `UnitTest` element is written under `TestDefinitions` with the
  `testId`, `name`, and `storage` (from `CodeBase`) attributes, plus a `TestMethod`
  child element carrying `codeBase` (also from `CodeBase`), `className`, and `name`
- A `TestEntry` element is written under `TestEntries` linking `testId`, `executionId`,
  and `testListId`
- A single default `TestList` is included under `TestLists`
- A `ResultSummary` element with outcome counters closes the document

### TRX Deserialization

When deserializing a TRX document to a `TestResults` object:

- `TestResults.Id`, `TestResults.Name`, and `TestResults.UserName` are read from the
  `TestRun` attributes (`id`, `name`, and `runUser` respectively)
- Each `UnitTestResult` element under `Results` is mapped to a `TestResult`
- `TestId`, `ExecutionId`, `ComputerName`, `StartTime`, `Duration`, and
  `Outcome` are read from the `UnitTestResult` element attributes
- `SystemOutput`, `SystemError`, `ErrorMessage`, and `ErrorStackTrace` are read from
  the corresponding child elements under `Output`
- `Name`, `CodeBase`, and `ClassName` are resolved by locating the matching `UnitTest`
  element in `TestDefinitions` by matching `UnitTestResult/@testId` against
  `UnitTest/@id`, then reading from the `TestMethod` child element

## JUnit XML Format

JUnit XML is a widely-adopted, cross-platform test result format supported by many
CI/CD systems including Jenkins, GitLab CI, GitHub Actions, and CircleCI.

This satisfies requirements `TestResults-Jun-Serialize` and `TestResults-Jun-Deserialize`.

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

- Each `testsuite` element carries an optional `timestamp` attribute (ISO 8601); when
  present it is used as the `StartTime` for all test cases in that suite
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
fidelity limitations apply when round-tripping through JUnit:

- **`Timeout` and `Aborted` outcomes are not preserved.** Both are serialized as an
  `error` child element (since JUnit has no distinct timeout or aborted element), and
  deserialize back as `TestOutcome.Error`. Callers that require precise outcome
  preservation should use TRX.

- **`ClassName = "DefaultSuite"` round-trips with an empty `ClassName`.** Tests without
  a class name are grouped under the sentinel value `"DefaultSuite"` during
  serialization. On deserialization, that sentinel is mapped back to an empty string.
  Therefore a test whose `ClassName` is literally `"DefaultSuite"` will lose its class
  name after a JUnit round-trip.

## Format Conversion

The `Serializer.Deserialize()` method provides a single entry point for reading test
result files regardless of their format. This satisfies requirement
`TestResults-Ser-FormatConversion`.

The conversion algorithm:

1. Calls `Serializer.Identify()` to determine the format of the input document
2. Delegates to `TrxSerializer.Deserialize()` for `TestResultFormat.Trx`
3. Delegates to `JUnitSerializer.Deserialize()` for `TestResultFormat.JUnit`
4. Throws an exception for `TestResultFormat.Unknown`

This design means that callers do not need to know or specify the format — they simply
pass the raw content and receive a `TestResults` object.

Round-trip fidelity (serialize → deserialize → same data) is fully preserved for the
TRX format. For JUnit XML, two known limitations apply (see **JUnit Round-Trip Fidelity**
above), satisfying requirement `TestResults-Ser-RoundTrip`.
