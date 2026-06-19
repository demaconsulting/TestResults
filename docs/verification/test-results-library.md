# TestResults Library

## Verification Approach

System verification is performed with automated xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/TestResultsLibraryTests.cs`, with additional
end-to-end evidence from `test/DemaConsulting.TestResults.Tests/IO/IOTests.cs`. These
tests exercise the real in-memory model and the real serializer implementations with inline
TRX and JUnit XML payloads; no mocks or stubs are introduced at the system boundary because
the library has no external service dependencies. The system scenarios provide direct
coverage for requirements `TestResultsLibrary-Model-FormatIndependence`,
`TestResultsLibrary-IO-ReadResults`, `TestResultsLibrary-IO-WriteResults`, and
`TestResultsLibrary-IO-IdentifyFormat`.

## Test Environment

System tests run through the standard repository test runner by invoking `pwsh ./build.ps1`
or `dotnet test --configuration Release`. The test project targets `net481` on Windows and
`net8.0`, `net9.0`, and `net10.0` across supported environments. No database, network
service, or external configuration is required; the tests use in-memory objects, inline XML,
and embedded example files included in the repository.

## Acceptance Criteria

- All system-level tests in `TestResultsLibraryTests.cs` pass with zero failures.
- The library produces a navigable in-memory model that is independent of TRX and JUnit
  storage formats.
- The library reads supported TRX and JUnit inputs into `TestResults` objects.
- The library writes `TestResults` objects to both TRX and JUnit XML outputs.
- The library identifies TRX, JUnit, and unknown content formats without deserializing.

## Test Scenarios

**Format-agnostic in-memory model**: The system shall expose a model that callers can create
and inspect without any file-format knowledge, satisfying
`TestResultsLibrary-Model-FormatIndependence`. This scenario is tested by
`TestResultsLibrary_FormatAgnosticModel_CanRepresentTestResults`.

**Read TRX results into the model**: The system shall read TRX XML and return a populated,
navigable `TestResults` object, satisfying `TestResultsLibrary-IO-ReadResults`. This
scenario is tested by `TestResultsLibrary_Deserialize_TrxContent_ReturnsNavigableModel`.

**Read JUnit results into the model**: The system shall read JUnit XML and return a
populated, navigable `TestResults` object, satisfying
`TestResultsLibrary-IO-ReadResults`. This scenario is tested by
`TestResultsLibrary_Deserialize_JUnitContent_ReturnsNavigableModel`.

**Write TRX output from the model**: The system shall serialize an in-memory run to TRX XML,
satisfying `TestResultsLibrary-IO-WriteResults`. This scenario is tested by
`TestResultsLibrary_Serialize_InMemoryModel_ProducesTrxContent`.

**Write JUnit output from the model**: The system shall serialize an in-memory run to JUnit
XML, satisfying `TestResultsLibrary-IO-WriteResults`. This scenario is tested by
`TestResultsLibrary_Serialize_InMemoryModel_ProducesJUnitContent`.

**Identify test result file format**: The system shall determine the format of test result
content without deserializing it, satisfying `TestResultsLibrary-IO-IdentifyFormat`. This
scenario is tested by `TestResultsLibrary_Identify_TrxContent_ReturnsTrx`,
`TestResultsLibrary_Identify_JUnitContent_ReturnsJUnit`, and
`TestResultsLibrary_Identify_UnknownContent_ReturnsUnknown`.
