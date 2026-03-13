# Model

## Overview

The model layer provides a platform-agnostic, in-memory representation of test results.
It is deliberately free of serialization concerns — it knows nothing about TRX or JUnit
XML — so that it can serve as a neutral interchange format between different serializers
and consumers.

The model consists of four types:

- `TestOutcome` — an enumeration of all possible test execution outcomes
- `TestOutcomeExtensions` — extension methods that classify a `TestOutcome` into logical categories
- `TestResult` — a single test case result, including timing, output, and error information
- `TestResults` — a collection of `TestResult` objects representing a complete test run

## TestOutcome Enumeration

The `TestOutcome` enumeration (`TestOutcome.cs`) defines the full set of outcome values
recognized by the library. The values are drawn from the Visual Studio TRX format and
cover all states that a test case may reach during execution.

| Outcome               | Category     | Description                                                             |
|-----------------------|--------------|-------------------------------------------------------------------------|
| `Error`               | Failed       | Test encountered an unexpected error or exception                       |
| `Failed`              | Failed       | Test executed but did not meet its success criteria                     |
| `Timeout`             | Failed       | Test exceeded its allotted execution time                               |
| `Aborted`             | Failed       | Test was terminated before completion                                   |
| `Inconclusive`        | Executed     | Test executed but produced no definitive pass or fail result            |
| `PassedButRunAborted` | Passed       | Test passed, but the overall run was subsequently aborted               |
| `NotRunnable`         | Not executed | Test could not be executed (e.g., invalid configuration)                |
| `NotExecuted`         | Not executed | Test was not run (maps to JUnit `skipped`)                              |
| `Disconnected`        | Executed     | Test agent became disconnected; execution started but result is unknown |
| `Warning`             | Passed       | Test completed successfully but produced warnings                       |
| `Passed`              | Passed       | Test executed and met all success criteria                              |
| `Completed`           | Executed     | Test completed execution (used by some frameworks)                      |
| `InProgress`          | Executed     | Test is currently executing                                             |
| `Pending`             | Not executed | Test is scheduled but has not yet started                               |

The *Category* column is consistent with the `TestOutcomeExtensions` helpers: only outcomes
for which `IsExecuted()` returns `false` are labeled **Not executed**; outcomes that execute
but do not count as passed or failed are labeled **Executed**.

## TestOutcome Extensions

The `TestOutcomeExtensions` class provides three extension methods on `TestOutcome` that
classify an outcome into the three logical categories used by the serialization layer and
by consumers who need to aggregate or summarize results.

### IsPassed()

Returns `true` when the outcome falls into the *passed* category:

- `Passed`
- `PassedButRunAborted`
- `Warning`

All other outcomes return `false`. This satisfies requirements
`TestResults-Mdl-PassedOutcome`, `TestResults-Mdl-WarningOutcome`.

### IsFailed()

Returns `true` when the outcome falls into the *failed* category:

- `Failed`
- `Error`
- `Timeout`
- `Aborted`

All other outcomes return `false`. This satisfies requirements
`TestResults-Mdl-FailedOutcome`, `TestResults-Mdl-ErrorOutcome`,
`TestResults-Mdl-TimeoutOutcome`, `TestResults-Mdl-AbortedOutcome`.

### IsExecuted()

Returns `false` (i.e., the test was **not** executed) for the following outcomes:

- `NotRunnable`
- `NotExecuted`
- `Pending`

Returns `true` for all other outcomes, including `Inconclusive`, `InProgress`, and
`Disconnected`. This satisfies requirements `TestResults-Mdl-NotExecutedOutcome`,
`TestResults-Mdl-PendingOutcome`, `TestResults-Mdl-InconclusiveOutcome`.

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

## TestResults Class

The `TestResults` class (`TestResults.cs`) represents a complete test run — a named
collection of `TestResult` objects along with run-level metadata.

### Properties

| Property   | Type                | Default          | Description                            |
|------------|---------------------|------------------|----------------------------------------|
| `Id`       | `Guid`              | `Guid.NewGuid()` | Uniquely identifies this test run      |
| `Name`     | `string`            | `string.Empty`   | Display name of the test run           |
| `UserName` | `string`            | `string.Empty`   | User or identity that initiated the run|
| `Results`  | `List<TestResult>`  | `[]`             | Ordered collection of test case results|

`Id` is auto-generated on construction for the same reasons as `TestResult.TestId` — it
ensures every run is uniquely identifiable in TRX output without requiring callers to
supply an identifier.

`Results` is initialized to an empty list, so callers can simply add items without
first checking for null.
