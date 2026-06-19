### SerializerHelpers

#### Purpose

The SerializerHelpers unit provides shared infrastructure used by both XML serializers. Its
single responsibility is to make string-based XML serialization report UTF-8 rather than the
default UTF-16 declaration produced by `StringWriter`.

#### Data Model

**Utf8StringWriter**: `internal sealed class` - Specialized `StringWriter` implementation used
by both serializer units when saving XML to a string.

**Encoding**: `Encoding` - Overridden property that always returns `Encoding.UTF8` so the XML
prolog declares `encoding="utf-8"`.

#### Key Properties

**Encoding**: Read-only property override that reports UTF-8 as the writer encoding.

- *Returns*: `Encoding` - Always `Encoding.UTF8`.
- *Preconditions*: None.
- *Postconditions*: Any XML writer using this instance emits a UTF-8 declaration in the XML
  prolog.

The property exists because .NET `StringWriter` defaults to UTF-16, which would advertise the
wrong encoding for XML strings that callers later persist as UTF-8.

#### Error Handling

N/A - the unit exposes a deterministic property override with no inputs, branching, or local
error recovery.

#### Dependencies

- **System.IO.StringWriter** - base class for string-backed text writing.
- **System.Text.Encoding** - provides the UTF-8 encoding instance returned by the override.

#### Callers

- **TrxSerializer** - uses `Utf8StringWriter` when saving TRX XML to a string.
- **JUnitSerializer** - uses `Utf8StringWriter` when saving JUnit XML to a string.
