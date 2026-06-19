## TestResults

### Verification Approach

The `TestResults` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/TestResultsTests.cs`. Verification uses direct
construction of the real `TestResults` class because the unit is a simple collection and
metadata container with no external dependencies to mock or stub. The tests provide direct
evidence for requirements `TestResults-Model-RunIdentity`, `TestResults-Model-RunName`,
`TestResults-Model-RunUserName`, `TestResults-Model-RunCollectionNonNull`, and
`TestResults-Model-RunCollectionEmpty` by checking run-level identity,
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

**Run name default**: A new `TestResults` instance shall initialize `Name` to an empty string
so the model is safe to use before enrichment, satisfying `TestResults-Model-RunName`. This
scenario is tested by `TestResults_Name_Default_IsEmpty`.

**Run user name default**: A new `TestResults` instance shall initialize `UserName` to an
empty string so the model is safe to use before enrichment, satisfying
`TestResults-Model-RunUserName`. This scenario is tested by
`TestResults_UserName_Default_IsEmpty`.

**Results collection non-null initialization**: A new `TestResults` instance shall expose a
non-null `Results` list ready for callers to populate, satisfying
`TestResults-Model-RunCollectionNonNull`. This scenario is tested by
`TestResults_Results_Default_IsNotNull`.

**Results collection empty initialization**: A new `TestResults` instance shall expose an
empty `Results` list so callers can build a result set from scratch, satisfying
`TestResults-Model-RunCollectionEmpty`. This scenario is tested by
`TestResults_Results_Default_IsEmpty`.
