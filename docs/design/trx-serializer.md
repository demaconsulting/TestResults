# TrxSerializer

The `TrxSerializer` class reads and writes test result data in the TRX (Visual Studio
Test Results) XML format. It operates on the model layer types and has no knowledge of
how test results are produced or consumed beyond the TRX XML structures it reads and
writes.

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
- Throws `InvalidOperationException` if the document structure is invalid: a
  `UnitTestResult` that references a non-existent `testId`, or the `TestDefinitions`
  section contains duplicate `UnitTest/@id` values

### TRX Round-Trip Fidelity

Round-trip fidelity (serialize → deserialize → same data) is fully preserved for the
TRX format. All `TestResults` and `TestResult` properties that are written during
serialization are read back identically during deserialization. This satisfies
requirement `TestResults-Ser-RoundTrip` for TRX.
