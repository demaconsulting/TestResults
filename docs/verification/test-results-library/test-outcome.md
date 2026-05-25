## TestOutcome

### Verification Approach

The `TestOutcome` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/TestOutcomeTests.cs`. Verification uses the real enum
values and extension methods with no mocked dependencies because the unit is pure
classification logic. The tests provide direct evidence for outcome requirements
`TestResults-Model-PassedOutcome`, `TestResults-Model-FailedOutcome`,
`TestResults-Model-ErrorOutcome`, `TestResults-Model-TimeoutOutcome`,
`TestResults-Model-NotExecutedOutcome`, `TestResults-Model-InconclusiveOutcome`,
`TestResults-Model-AbortedOutcome`, `TestResults-Model-PendingOutcome`,
`TestResults-Model-WarningOutcome`, `TestResults-Model-PassedButRunAbortedOutcome`,
`TestResults-Model-NotRunnableOutcome`, `TestResults-Model-CompletedOutcome`,
`TestResults-Model-InProgressOutcome`, and `TestResults-Model-DisconnectedOutcome`.

### Test Environment

N/A - standard test environment. The unit tests run entirely in process and require only the
standard .NET test runner.

### Acceptance Criteria

- All `TestOutcomeTests` pass with zero failures.
- `IsPassed()` returns `true` only for outcomes classified as passed.
- `IsFailed()` returns `true` only for outcomes classified as failed.
- `IsExecuted()` distinguishes executed from non-executed outcomes exactly as documented.

### Test Scenarios

**Passed outcome classification**: Passed-style outcomes shall be reported as passed while all
other outcomes remain false, satisfying `TestResults-Model-PassedOutcome`,
`TestResults-Model-WarningOutcome`, and
`TestResults-Model-PassedButRunAbortedOutcome`. This scenario is tested by
`TestOutcome_IsPassed_AllOutcomes_ReturnsExpectedResult`.

**Failed outcome classification**: Failed-style outcomes shall be reported as failed while
non-failure outcomes remain false, satisfying `TestResults-Model-FailedOutcome`,
`TestResults-Model-ErrorOutcome`, `TestResults-Model-TimeoutOutcome`, and
`TestResults-Model-AbortedOutcome`. This scenario is tested by
`TestOutcome_IsFailed_AllOutcomes_ReturnsExpectedResult`.

**Executed state classification across all outcomes**: The extension methods shall preserve
the library's executed-versus-not-executed distinction for inconclusive, pending,
not-runnable, completed, in-progress, and disconnected outcomes, satisfying the remaining
`TestResults-Model-*Outcome` requirements. This scenario is tested by
`TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult`.

**Not executed guardrail**: A `NotExecuted` result shall be classified as not executed,
reinforcing `TestResults-Model-NotExecutedOutcome`. This scenario is tested by
`TestOutcome_IsExecuted_NotExecutedOutcome_ReturnsFalse`.
