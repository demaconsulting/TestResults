### TrxSerializer

#### Verification Approach

The `TrxSerializer` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/IO/TrxSerializerTests.cs` and
`test/DemaConsulting.TestResults.Tests/IO/TrxExampleTests.cs`. Verification uses the real
serializer, the real model classes, inline XML fixtures, and embedded example TRX files. No
mocks are used because the unit depends only on local model types and .NET XML APIs. The
tests provide evidence for requirements `TestResults-Trx-Serialize`,
`TestResults-Trx-Utf8Encoding`, and `TestResults-Trx-Deserialize`, while also supporting the
TRX side of `TestResults-Serializer-RoundTrip`.

#### Test Environment

N/A - standard test environment. Tests use inline XML payloads and embedded resource files
`example1.trx` and `example2.trx` from the test assembly.

#### Acceptance Criteria

- All `TrxSerializerTests` and `TrxExampleTests` pass with zero failures.
- Serialized TRX contains the required result, definition, entry, summary, storage, and
  encoding information.
- Deserialization reconstructs run metadata, test metadata, output, and error details from
  valid TRX input.
- Invalid TRX structures and invalid arguments are rejected as documented.
- Serializing and then deserializing TRX preserves test data.

#### Test Scenarios

**Serialize valid TRX documents**: The unit shall serialize single-result and multi-result
runs to structurally valid TRX XML, satisfying `TestResults-Trx-Serialize`. This scenario is
tested by `TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml`,
`TrxSerializer_Serialize_MultipleTestResults_ProducesValidTrxXml`,
`TrxSerializer_Serialize_WithCodeBase_EmitsStorageAttributeOnUnitTest`, and
`TrxSerializer_Serialize_StackTraceWithoutMessage_IncludesStackTraceElement`.

**Emit UTF-8 XML declarations**: TRX output shall declare UTF-8 encoding, satisfying
`TestResults-Trx-Utf8Encoding`. This scenario is tested by
`TrxSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding`.

**Deserialize valid TRX inputs**: The unit shall reconstruct `TestResults` data from basic,
complex, and repository example TRX documents, satisfying `TestResults-Trx-Deserialize`.
This scenario is tested by `TrxSerializer_Deserialize_BasicTrxXml_ReturnsTestResults`,
`TrxSerializer_Deserialize_ComplexTrxXml_ReturnsTestResults`,
`TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults`, and
`TrxExampleTests_Deserialize_Example2Trx_ReturnsAllTestResults`.

**Preserve TRX round-trip fidelity**: TRX serialization followed by deserialization shall
preserve the test data written by the serializer, supporting
`TestResults-Serializer-RoundTrip`. This scenario is tested by
`TrxSerializer_Serialize_ThenDeserialize_PreservesTestData`.

**Reject invalid TRX inputs**: The unit shall reject null or blank inputs and malformed TRX
cross-reference structures, satisfying the error-handling expectations of
`TestResults-Trx-Deserialize`. This scenario is tested by
`TrxSerializer_Serialize_NullResults_ThrowsArgumentNullException`,
`TrxSerializer_Deserialize_NullContents_ThrowsArgumentNullException`,
`TrxSerializer_Deserialize_EmptyContents_ThrowsArgumentException`,
`TrxSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException`,
`TrxSerializer_Deserialize_DuplicateUnitTestId_ThrowsInvalidOperationException`, and
`TrxSerializer_Deserialize_NonExistentTestId_ThrowsInvalidOperationException`.
