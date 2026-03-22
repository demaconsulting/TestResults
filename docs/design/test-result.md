# TestResult

The `TestResult` class represents the result of a single test case execution. Each
instance holds the identity, timing, output streams, and error information for one test.

## TestResult Class

The `TestResult` class (`TestResult.cs`) represents the result of a single test case
execution. Each instance holds the identity, timing, output streams, and error information
for one test.

### Identity Properties

| Property      | Type     | Default                    | Description                              |
|---------------|----------|----------------------------|------------------------------------------|
| `TestId`      | `Guid`   | `Guid.NewGuid()`           | Uniquely identifies the test definition  |
| `ExecutionId` | `Guid`   | `Guid.NewGuid()`           | Uniquely identifies this execution       |
| `Name`        | `string` | `string.Empty`             | Display name of the test case            |
| `CodeBase`    | `string` | `string.Empty`             | Assembly or file path containing the test|
| `ClassName`   | `string` | `string.Empty`             | Fully-qualified class name of the test   |

`TestId` and `ExecutionId` are auto-generated on construction so that every `TestResult`
is uniquely identifiable without requiring the caller to supply identifiers. They are
preserved during round-trip serialization to maintain referential integrity in TRX files,
where `TestDefinitions`, `TestEntries`, and `Results` sections all cross-reference these
identifiers.

### Execution Properties

| Property       | Type       | Default                      | Description                             |
|----------------|------------|------------------------------|-----------------------------------------|
| `ComputerName` | `string`   | `Environment.MachineName`    | Host that executed the test             |
| `StartTime`    | `DateTime` | `DateTime.UtcNow`            | UTC timestamp when execution began      |
| `Duration`     | `TimeSpan` | `TimeSpan.Zero`              | Wall-clock duration of the test         |

`ComputerName` defaults to the current machine name so that it is populated correctly
in the common case where the test is being recorded on the same machine that ran it.

### Outcome Properties

| Property          | Type          | Default              | Description                          |
|-------------------|---------------|----------------------|--------------------------------------|
| `Outcome`         | `TestOutcome` | `NotExecuted`        | Result classification of the test    |
| `ErrorMessage`    | `string`      | `string.Empty`       | Human-readable failure message       |
| `ErrorStackTrace` | `string`      | `string.Empty`       | Stack trace associated with a failure|

The default outcome of `NotExecuted` is intentional: a `TestResult` that has been
constructed but not yet populated with execution data is considered not executed. This
satisfies requirement `TestResults-Mdl-NotExecutedOutcome`.

### Output Properties

| Property       | Type     | Default        | Description                              |
|----------------|----------|----------------|------------------------------------------|
| `SystemOutput` | `string` | `string.Empty` | Content written to standard output       |
| `SystemError`  | `string` | `string.Empty` | Content written to standard error        |

Capturing output streams satisfies requirement `TestResults-Mdl-TestOutput`. Error
messages and stack traces satisfy requirement `TestResults-Mdl-ErrorInfo`.
