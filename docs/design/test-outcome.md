# TestOutcome

The `TestOutcome` enumeration and its companion `TestOutcomeExtensions` class define
the full set of outcome values recognized by the library and provide extension methods
that classify those values into the logical categories used by the serialization layer
and by consumers who need to aggregate or summarize results.

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
`TestResults-Mdl-PassedOutcome`, `TestResults-Mdl-PassedButRunAbortedOutcome`,
`TestResults-Mdl-WarningOutcome`.

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
`TestResults-Mdl-NotRunnableOutcome`, `TestResults-Mdl-PendingOutcome`,
`TestResults-Mdl-InconclusiveOutcome`, `TestResults-Mdl-CompletedOutcome`,
`TestResults-Mdl-InProgressOutcome`, `TestResults-Mdl-DisconnectedOutcome`.
