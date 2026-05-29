## IO

### Verification Approach

The IO subsystem is verified with xUnit v3 tests in
`test/DemaConsulting.TestResults.Tests/IO/IOTests.cs`. These tests exercise the subsystem
through its public integration points by using the real `Serializer`, `TrxSerializer`, and
`JUnitSerializer` implementations with inline XML payloads and in-memory `TestResults`
objects. No mocking is required at the subsystem boundary because the subsystem depends only
on local model classes and .NET XML APIs. The scenarios cover subsystem requirements
`TestResults-IO-Identify`, `TestResults-IO-Deserialize`, and `TestResults-IO-Serialize`.

### Test Environment

N/A - standard test environment. The subsystem tests run under `dotnet test` with inline XML
strings and do not require external services, files, or special configuration.

### Acceptance Criteria

- All IO subsystem tests in `IOTests.cs` pass with zero failures.
- TRX and JUnit inputs are identified correctly at the subsystem entry point.
- Supported TRX and JUnit documents deserialize to `TestResults` objects.
- In-memory `TestResults` objects serialize to both supported output formats.

### Test Scenarios

**Identify TRX content**: The subsystem shall recognize TRX XML by its root element and
namespace, satisfying `TestResults-IO-Identify`. This scenario is tested by
`IO_Identify_TrxContent_ReturnsTrx`.

**Identify JUnit content**: The subsystem shall recognize JUnit XML at the format boundary,
satisfying `TestResults-IO-Identify`. This scenario is tested by
`IO_Identify_JUnitContent_ReturnsJUnit`.

**Deserialize supported formats**: The subsystem shall translate both TRX and JUnit inputs
into the shared in-memory model, satisfying `TestResults-IO-Deserialize`. This scenario is
tested by `IO_Deserialize_TrxContent_ReturnsTestResults` and
`IO_Deserialize_JUnitContent_ReturnsTestResults`.

**Serialize supported formats**: The subsystem shall emit both TRX and JUnit XML from the
shared in-memory model, satisfying `TestResults-IO-Serialize`. This scenario is tested by
`IO_Serialize_TestResults_ProducesTrxContent` and
`IO_Serialize_TestResults_ProducesJUnitContent`.

**Cross-format round-trip**: The subsystem shall preserve test count, test names, class names,
and pass/fail/skip outcomes when results are converted from TRX to JUnit format, satisfying
`TestResults-IO-Deserialize`. This scenario is tested by
`IO_TrxSerializedResults_RoundTripsViaJUnit_PreservesCoreTestData`.

**Identify null content returns unknown**: The subsystem shall return `Unknown` when null
content is supplied to `Identify`, satisfying `TestResults-IO-Identify`. This scenario is
tested by `IO_Identify_NullContent_ReturnsUnknown`.

**Identify invalid XML returns unknown**: The subsystem shall return `Unknown` for
non-XML content supplied to `Identify`, satisfying `TestResults-IO-Identify`. This scenario
is tested by `IO_Identify_InvalidXmlContent_ReturnsUnknown`.

**Identify unrecognized XML returns unknown**: The subsystem shall return `Unknown` for
valid XML with an unrecognized root or namespace, satisfying `TestResults-IO-Identify`. This
scenario is tested by `IO_Identify_UnknownXmlContent_ReturnsUnknown`.

**Deserialize null content throws**: The subsystem shall throw `ArgumentNullException` when
null content is supplied to `Deserialize`, satisfying `TestResults-IO-Deserialize`. This
scenario is tested by `IO_Deserialize_NullContent_ThrowsArgumentNullException`.

**Deserialize whitespace content throws**: The subsystem shall throw `ArgumentException` when
whitespace-only content is supplied to `Deserialize`, satisfying `TestResults-IO-Deserialize`.
This scenario is tested by `IO_Deserialize_WhitespaceContent_ThrowsArgumentException`.

**Deserialize unknown format throws**: The subsystem shall throw `InvalidOperationException`
when content with an unrecognized format is supplied to `Deserialize`, satisfying
`TestResults-IO-Deserialize`. This scenario is tested by
`IO_Deserialize_UnknownContent_ThrowsInvalidOperationException`.
