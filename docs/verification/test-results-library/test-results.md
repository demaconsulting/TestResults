## TestResults

### Verification Approach

The `TestResults` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/TestResultsTests.cs`. Verification uses direct
construction of the real `TestResults` class because the unit is a simple collection and
metadata container with no external dependencies to mock or stub. The tests provide direct
evidence for requirements `TestResults-Model-RunIdentity`, `TestResults-Model-RunNaming`,
and `TestResults-Model-RunCollection` by checking run-level identity,
metadata defaults, and collection initialization.

### Test Environment

N/A - standard test environment. The tests run entirely in process and require no files,
services, or configuration beyond the standard .NET test runner.

### Acceptance Criteria

- All `TestResultsTests` pass with zero failures.
- New `TestResults` instances generate non-empty, unique run identifiers.
- Run name and user name default to empty strings.
- The `Results` collection is initialized, non-null, and empty by default.

### Test Scenarios

**Generated run identifier**: A new `TestResults` instance shall create a unique run ID so a
test run can be serialized without additional caller setup, satisfying
`TestResults-Model-RunIdentity`. This scenario is tested by
`TestResults_Id_Default_IsNotEmpty` and `TestResults_Id_TwoInstances_AreUnique`.

**Run metadata defaults**: A new `TestResults` instance shall initialize `Name` and
`UserName` to empty strings so the model is safe to use before enrichment, satisfying
`TestResults-Model-RunNaming`. This scenario is tested by
`TestResults_Name_Default_IsEmpty` and `TestResults_UserName_Default_IsEmpty`.

**Results collection initialization**: A new `TestResults` instance shall expose a non-null,
empty `Results` list ready for callers to populate, satisfying
`TestResults-Model-RunCollection`. This scenario is tested by
`TestResults_Results_Default_IsNotNull` and `TestResults_Results_Default_IsEmpty`.
