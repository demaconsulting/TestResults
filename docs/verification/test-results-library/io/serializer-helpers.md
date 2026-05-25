### SerializerHelpers

#### Verification Approach

The `SerializerHelpers` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/IO/SerializerHelpersTests.cs`, with additional
consumer evidence in the UTF-8 declaration tests for `TrxSerializer` and `JUnitSerializer`.
Verification uses the real internal `Utf8StringWriter` type and no mocks because the unit is
self-contained. The direct requirement covered by this verification is
`TestResults-SerializerHelpers-Utf8Encoding`.

#### Test Environment

N/A - standard test environment. The helper test runs entirely in process and requires no
external files or services.

#### Acceptance Criteria

- All `SerializerHelpersTests` pass with zero failures.
- `Utf8StringWriter.Encoding` reports UTF-8.
- The helper remains compatible with the XML serializers that consume it.

#### Test Scenarios

**Report UTF-8 from the shared writer**: The unit shall expose a string writer that reports
UTF-8 as its encoding, satisfying `TestResults-SerializerHelpers-Utf8Encoding`. This
scenario is tested by `Utf8StringWriter_Encoding_Always_ReturnsUtf8`.

**Support serializer XML declarations**: The helper shall provide the encoding behavior needed
by both XML serializers so they emit `encoding="utf-8"` declarations. This scenario is
indirectly evidenced by `TrxSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding` and
`JUnitSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding`.
