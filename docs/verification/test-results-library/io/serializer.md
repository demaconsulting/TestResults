### Serializer

#### Verification Approach

The `Serializer` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/IO/SerializerTests.cs`. Verification uses real TRX and
JUnit XML strings and the real `TrxSerializer` and `JUnitSerializer` implementations; no
mocks or stubs are used because the unit's primary responsibility is dispatch and boundary
validation over local code. The tests provide evidence for requirements
`TestResults-Serializer-FormatIdentify`, `TestResults-Serializer-FormatConversion`, and
`TestResults-Serializer-RoundTrip`.

#### Test Environment

N/A - standard test environment. Tests use inline XML and embedded example TRX files that are
part of the test assembly.

#### Acceptance Criteria

- All `SerializerTests` pass with zero failures.
- Supported TRX and JUnit documents are identified correctly.
- Invalid, empty, whitespace, wrong-namespace, and unknown inputs are handled as documented.
- `Deserialize()` delegates to the correct serializer and preserves core test data during
  conversion workflows.

#### Test Scenarios

**Identify supported formats**: The unit shall detect TRX and both JUnit root forms,
satisfying `TestResults-Serializer-FormatIdentify`. This scenario is tested by
`Serializer_Identify_TrxContent_ReturnsTrx`,
`Serializer_Identify_JUnitTestsuitesContent_ReturnsJUnit`, and
`Serializer_Identify_JUnitTestsuiteContent_ReturnsJUnit`.

**Reject unsupported format probes**: The unit shall return `Unknown` for null, empty,
whitespace, malformed XML, unrecognized XML roots, and `TestRun` elements in the wrong
namespace, satisfying `TestResults-Serializer-FormatIdentify`. This scenario is tested by
`Serializer_Identify_EmptyContent_ReturnsUnknown`,
`Serializer_Identify_NullContent_ReturnsUnknown`,
`Serializer_Identify_WhitespaceContent_ReturnsUnknown`,
`Serializer_Identify_InvalidXml_ReturnsUnknown`,
`Serializer_Identify_UnrecognizedXmlFormat_ReturnsUnknown`, and
`Serializer_Identify_TestRunInWrongNamespace_ReturnsUnknown`.

**Dispatch deserialization to the correct implementation**: The unit shall convert supported
TRX and JUnit inputs into the shared model, satisfying
`TestResults-Serializer-FormatConversion`. This scenario is tested by
`Serializer_Deserialize_TrxContent_ReturnsTestResults`,
`Serializer_Deserialize_JUnitContent_ReturnsTestResults`,
`Serializer_Deserialize_RealTrxExample_ReturnsTestResults`,
`Serializer_Deserialize_TrxWithMultipleOutcomes_ParsesCorrectly`, and
`Serializer_Deserialize_JUnitWithSystemOutput_ParsesCorrectly`.

**Guard invalid deserialize inputs**: The unit shall reject null, blank, and unknown-format
content at the deserialize entry point, satisfying
`TestResults-Serializer-FormatConversion`. This scenario is tested by
`Serializer_Deserialize_NullContents_ThrowsArgumentNullException`,
`Serializer_Deserialize_EmptyContents_ThrowsArgumentException`,
`Serializer_Deserialize_WhitespaceContents_ThrowsArgumentException`, and
`Serializer_Deserialize_UnknownFormat_ThrowsInvalidOperationException`.

**Preserve core data during conversion**: The unit shall preserve essential test data through
cross-format workflows, satisfying `TestResults-Serializer-RoundTrip`. This scenario is
tested by `Serializer_TrxSerializedResults_RoundTripsViaJUnit_PreservesCoreTestData`.
