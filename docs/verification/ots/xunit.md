## xUnit

### Verification Approach

xUnit v3 is verified through the library test exercise approach: the five system-level tests in
`test/DemaConsulting.TestResults.Tests/TestResultsLibraryTests.cs` can only pass if xUnit
discovers and executes them correctly, produces accurate pass/fail results, and writes valid TRX
output files. These tests are the primary evidence that xUnit is operational; if xUnit were
non-functional, all tests would fail or not run at all. The tests are named in the OTS
requirements and tracked by ReqStream.

### Test Environment

The standard local or CI test runner environment invoking `dotnet test --configuration Release
--logger trx`. The test project targets `net481`, `net8.0`, `net9.0`, and `net10.0`; tests
must pass on all target frameworks. No database, network service, or external configuration is
required.

### Acceptance Criteria

- All five named library system tests pass with zero failures on all target frameworks.
- TRX result files are produced at the expected output paths after `dotnet test`.
- The requirement `TestResults-OTS-XUnit` is linked to all five named test identifiers in the
  ReqStream trace matrix.

### Test Scenarios

**Format-agnostic in-memory model**: xUnit shall discover and execute
`TestResultsLibrary_FormatAgnosticModel_CanRepresentTestResults` and report a passing result.
This scenario is confirmed by `TestResultsLibrary_FormatAgnosticModel_CanRepresentTestResults`.

**Read TRX results**: xUnit shall discover and execute
`TestResultsLibrary_Deserialize_TrxContent_ReturnsNavigableModel` and report a passing result.
This scenario is confirmed by `TestResultsLibrary_Deserialize_TrxContent_ReturnsNavigableModel`.

**Read JUnit results**: xUnit shall discover and execute
`TestResultsLibrary_Deserialize_JUnitContent_ReturnsNavigableModel` and report a passing result.
This scenario is confirmed by
`TestResultsLibrary_Deserialize_JUnitContent_ReturnsNavigableModel`.

**Write TRX output**: xUnit shall discover and execute
`TestResultsLibrary_Serialize_InMemoryModel_ProducesTrxContent` and report a passing result.
This scenario is confirmed by `TestResultsLibrary_Serialize_InMemoryModel_ProducesTrxContent`.

**Write JUnit output**: xUnit shall discover and execute
`TestResultsLibrary_Serialize_InMemoryModel_ProducesJUnitContent` and report a passing result.
This scenario is confirmed by `TestResultsLibrary_Serialize_InMemoryModel_ProducesJUnitContent`.
