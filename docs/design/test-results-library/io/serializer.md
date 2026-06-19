### Serializer

#### Purpose

The Serializer unit is the IO subsystem facade for callers that do not know the incoming
format. It identifies whether XML content is TRX or JUnit and delegates deserialization to
the corresponding format-specific serializer.

#### Data Model

**TestResultFormat**: `enum` - Defines the three public detection outcomes: `Unknown`, `Trx`,
and `JUnit`.

**TrxNamespaceUri**: `string` - Hard-coded XML namespace used to distinguish genuine TRX
`TestRun` roots from unrelated XML documents.

#### Key Methods

**Identify**: Detects the serialized test-result format from XML content.

- *Parameters*: `string contents` - Raw XML content to inspect.
- *Returns*: `TestResultFormat` - `Trx`, `JUnit`, or `Unknown`.
- *Preconditions*: None.
- *Postconditions*: Returns `Unknown` for null, whitespace, malformed XML, or unrecognized
  roots, and never throws for those cases.

The method parses the XML root, checks the exact TRX namespace plus `TestRun` root for TRX,
and checks `testsuites` or `testsuite` root names for JUnit.

**Deserialize**: Routes supported XML content to the correct format-specific deserializer.

- *Parameters*: `string contents` - Raw XML content to deserialize.
- *Returns*: `TestResults` - Populated in-memory model.
- *Preconditions*: `contents` must be non-null, non-whitespace, and represent a supported
  format.
- *Postconditions*: Returns the result of `TrxSerializer.Deserialize()` or
  `JUnitSerializer.Deserialize()` and throws for unknown formats.

The method keeps format selection in one place so callers can deserialize mixed result files
through a single entry point. Round-trip fidelity across TRX and JUnit is a subsystem-level
guarantee achieved through the combined behavior of TrxSerializer and JUnitSerializer; the
Serializer itself only provides the dispatch mechanism.

#### Error Handling

`Identify()` catches `XmlException` and converts invalid XML to `TestResultFormat.Unknown`.
`Deserialize()` validates `contents` before any XML parsing: passing `null` throws
`ArgumentNullException`, and passing an empty or whitespace-only string throws
`ArgumentException`. It throws `InvalidOperationException` when `Identify()` cannot classify
the document. Exceptions raised by the delegated serializer propagate to the caller.

#### Dependencies

- **TrxSerializer** - performs format-specific TRX deserialization.
- **JUnitSerializer** - performs format-specific JUnit deserialization.
- **TestResults** - common return type for deserialization.
- **System.Xml.Linq.XDocument** - parses XML roots for format identification.

#### Callers

N/A - entry-point unit, called directly by library consumers and exercised by system and
subsystem tests.
