## IO

### Overview

The IO subsystem translates between the format-neutral in-memory model and external XML test
result documents. Its boundary starts at string-based XML content and ends at populated
`TestResults` objects or serialized TRX/JUnit XML strings. The subsystem contains four units:
`Serializer`, `SerializerHelpers`, `TrxSerializer`, and `JUnitSerializer`. It does not own
file-system access, transport, or test execution; those concerns stay outside the subsystem.

### Interfaces

**Format identification facade**: Entry point for callers that do not know the input format.

- *Type*: In-process .NET public API.
- *Role*: Provider.
- *Contract*: `Serializer.Identify(string)` returns `Unknown`, `Trx`, or `JUnit`, and
  `Serializer.Deserialize(string)` routes supported content to the correct serializer.
- *Constraints*: `Identify()` treats null, whitespace, malformed XML, and unknown roots as
  `Unknown`; `Deserialize()` throws when the content is null, whitespace, or unsupported.

**TRX XML translation**: Conversion between the shared model and Microsoft TRX XML.

- *Type*: XML document interface.
- *Role*: Provider.
- *Contract*: `TrxSerializer.Serialize(TestResults)` and `TrxSerializer.Deserialize(string)`
  convert between the model and `TestRun` documents.
- *Constraints*: Deserialization requires consistent `UnitTestResult` to `UnitTest`
  cross-references and rejects duplicate `UnitTest` identifiers.

**JUnit XML translation**: Conversion between the shared model and JUnit XML.

- *Type*: XML document interface.
- *Role*: Provider.
- *Contract*: `JUnitSerializer.Serialize(TestResults)` and
  `JUnitSerializer.Deserialize(string)` convert between the model and `testsuites` or
  `testsuite` documents.
- *Constraints*: Results are grouped by `ClassName` during serialization, and empty class
  names are represented with the sentinel value `DefaultSuite`.

### Design

1. `Serializer.Identify()` parses the XML root element and distinguishes the exact TRX
   namespace from JUnit root names.
2. `Serializer.Deserialize()` delegates supported content to `TrxSerializer` or
   `JUnitSerializer`, keeping format selection out of the model layer.
3. `TrxSerializer` maps `TestRun`, `Results`, `TestDefinitions`, `TestEntries`, and
   `ResultSummary` structures to and from `TestResults` and `TestResult` instances.
4. `JUnitSerializer` groups test cases by `ClassName`, calculates suite counters and
   timestamps, and maps JUnit outcome elements to the shared `TestOutcome` values.
5. `SerializerHelpers` provides the shared `Utf8StringWriter` used by both serializers so
   all emitted XML declares UTF-8 consistently.
