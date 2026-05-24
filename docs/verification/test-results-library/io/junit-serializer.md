### JUnitSerializer

#### Verification Approach

The `JUnitSerializer` unit is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/IO/JUnitSerializerTests.cs`. Verification uses the
real serializer, real model types, and inline JUnit XML fixtures. No mocks are required
because the unit depends only on local model classes and .NET XML APIs. The tests provide
evidence for requirements `TestResults-JUnit-Serialize`, `TestResults-JUnit-Utf8Encoding`,
and `TestResults-JUnit-Deserialize`, while also supplying JUnit-side evidence for
`TestResults-Serializer-RoundTrip` and the documented JUnit fidelity limits.

#### Test Environment

N/A - standard test environment. The tests use inline XML and in-memory model instances and
require no external services or configuration.

#### Acceptance Criteria

- All `JUnitSerializerTests` pass with zero failures.
- Serialization produces valid JUnit XML with suite grouping, counters, timestamps, output,
  and UTF-8 declarations.
- Deserialization reconstructs supported JUnit data, including failure, error, skipped,
  output, timestamp, and multi-suite cases.
- Documented fidelity limits for timeout, aborted, and default-suite cases are verified.
- Null, blank, and malformed boundary inputs are handled as documented.

#### Test Scenarios

**Serialize supported JUnit cases**: The unit shall serialize passed, failed, errored,
skipped, grouped, usage-example, and empty-class-name results to valid JUnit XML,
satisfying `TestResults-JUnit-Serialize`. This scenario is tested by
`JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml`,
`JUnitSerializer_Serialize_FailedTest_IncludesFailureElement`,
`JUnitSerializer_Serialize_ErrorTest_IncludesErrorElement`,
`JUnitSerializer_Serialize_SkippedTest_IncludesSkippedElement`,
`JUnitSerializer_Serialize_TestWithOutput_IncludesSystemOutAndErr`,
`JUnitSerializer_Serialize_MultipleTestsInClasses_GroupsByClassName`,
`JUnitSerializer_Serialize_EmptyClassName_UsesDefaultSuite`,
`JUnitSerializer_Serialize_UsageExample_ProducesValidJUnitXml`,
`JUnitSerializer_Serialize_NotRunnableOutcome_IncludesSkippedElement`, and
`JUnitSerializer_Serialize_PendingOutcome_IncludesSkippedElement`.

**Emit timestamps and UTF-8 declarations**: The unit shall write UTC timestamps and UTF-8
XML declarations required by downstream JUnit consumers, satisfying
`TestResults-JUnit-Utf8Encoding` and the serializer design constraints. This scenario is
tested by `JUnitSerializer_Serialize_WithStartTime_EmitsUtcTimestampAttribute` and
`JUnitSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding`.

**Deserialize supported JUnit cases**: The unit shall reconstruct model data from common and
edge-case JUnit inputs, satisfying `TestResults-JUnit-Deserialize`. This scenario is tested
by `JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults`,
`JUnitSerializer_Deserialize_FailedTest_ReturnsFailureDetails`,
`JUnitSerializer_Deserialize_ErrorTest_ReturnsErrorDetails`,
`JUnitSerializer_Deserialize_SkippedTest_ReturnsSkippedStatus`,
`JUnitSerializer_Deserialize_TestWithOutput_ReturnsSystemOutput`,
`JUnitSerializer_Deserialize_MultipleTestSuites_ReturnsAllTests`,
`JUnitSerializer_Deserialize_DefaultSuite_ReturnsEmptyClassName`,
`JUnitSerializer_Deserialize_MissingTimeAttribute_DefaultsToZero`,
`JUnitSerializer_Deserialize_WithTimestamp_SetsStartTimeOnTestCases`,
`JUnitSerializer_Deserialize_InvalidTimestamp_DefaultsStartTime`, and
`JUnitSerializer_Deserialize_BareTestSuiteRoot_DeserializesCorrectly`.

**Preserve data within documented JUnit limits**: The unit shall preserve core test data
across JUnit round-trips while documenting known fidelity limits for timeout and aborted
outcomes, supporting `TestResults-Serializer-RoundTrip`. This scenario is tested by
`JUnitSerializer_Serialize_ThenDeserialize_PreservesTestData`,
`JUnitSerializer_Serialize_ThenDeserialize_TimeoutOutcomeBecomesError`, and
`JUnitSerializer_Serialize_ThenDeserialize_AbortedOutcomeBecomesError`.

**Reject invalid JUnit arguments**: The unit shall reject null or blank inputs at the public
API boundary, satisfying the error-handling expectations of
`TestResults-JUnit-Deserialize`. This scenario is tested by
`JUnitSerializer_Serialize_NullResults_ThrowsArgumentNullException`,
`JUnitSerializer_Deserialize_NullContents_ThrowsArgumentNullException`,
`JUnitSerializer_Deserialize_EmptyContents_ThrowsArgumentException`, and
`JUnitSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException`.
