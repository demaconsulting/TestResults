## TestResult

### Verification Approach

The `TestResult` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/TestResultTests.cs`. Verification uses direct
construction and mutation of the real `TestResult` class, with no mocked or injected
dependencies because the unit is a data container. The tests provide direct evidence for
requirements `TestResults-Model-TestOutput` and `TestResults-Model-ErrorInfo`, and they also
verify supporting design behaviors such as generated identifiers, default metadata values,
and the default `NotExecuted` outcome used by the serializers.

### Test Environment

N/A - standard test environment. The unit tests construct `TestResult` instances directly and
do not require any external services or files.

### Acceptance Criteria

- All `TestResultTests` pass with zero failures.
- New `TestResult` instances generate non-empty, unique `TestId` and `ExecutionId` values.
- Default property values match the design for names, timing, host information, and outcome.
- Output, error message, and stack trace properties retain assigned values without loss.

### Test Scenarios

**Generated identifiers and metadata defaults**: A new `TestResult` shall generate unique
`TestId` and `ExecutionId` values and initialize descriptive metadata to safe defaults so
serializers can emit a valid record without null handling. This scenario is tested by
`TestResult_TestId_Default_IsNotEmpty`, `TestResult_TestId_TwoInstances_AreUnique`,
`TestResult_ExecutionId_Default_IsNotEmpty`, `TestResult_ExecutionId_TwoInstances_AreUnique`,
`TestResult_Name_Default_IsEmpty`, `TestResult_CodeBase_Default_IsEmpty`, and
`TestResult_ClassName_Default_IsEmpty`.

**Execution context defaults**: A new `TestResult` shall default `ComputerName`, `StartTime`,
`Duration`, and `Outcome` to values consistent with the design intent for a newly-created
record. This scenario is tested by
`TestResult_ComputerName_Default_IsEnvironmentMachineName`,
`TestResult_StartTime_Default_IsApproximatelyNow`, `TestResult_Duration_Default_IsZero`, and
`TestResult_Outcome_Default_IsNotExecuted`.

**Captured output streams**: The unit shall support empty-by-default standard output and
standard error properties and preserve assigned content, satisfying
`TestResults-Model-TestOutput`. This scenario is tested by
`TestResult_SystemOutput_Default_IsEmpty`, `TestResult_SystemOutput_Set_RetainsValue`,
`TestResult_SystemError_Default_IsEmpty`, and
`TestResult_SystemError_Set_RetainsValue`.

**Captured error details**: The unit shall support empty-by-default error fields and preserve
assigned error text, satisfying `TestResults-Model-ErrorInfo`. This scenario is tested by
`TestResult_ErrorMessage_Default_IsEmpty`, `TestResult_ErrorMessage_Set_RetainsValue`,
`TestResult_ErrorStackTrace_Default_IsEmpty`, and
`TestResult_ErrorStackTrace_Set_RetainsValue`.
