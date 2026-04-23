# SerializerHelpers

Requirements: [serializer-helpers.yaml](../../../../reqstream/test-results-library/io/serializer-helpers.yaml)

The `SerializerHelpers` unit provides internal helper types shared by the IO subsystem's
serializer implementations. It is not part of the public API.

## Utf8StringWriter

The `Utf8StringWriter` class (`SerializerHelpers.cs`) is an internal helper that extends
`StringWriter` to report UTF-8 as its encoding.

When `XmlWriter` or `XmlSerializer` writes to a `StringWriter`, it reads the writer's
`Encoding` property to determine which XML declaration encoding to emit. The default
`StringWriter` reports UTF-16 (the .NET string encoding), which would cause serializers
to write `encoding="utf-16"` in the XML declaration even when the resulting string is
later converted to UTF-8 bytes.

`Utf8StringWriter` overrides the `Encoding` property to return `Encoding.UTF8`, so the
XML declaration correctly declares `encoding="UTF-8"`. It is used by both
`TrxSerializer.Serialize()` and `JUnitSerializer.Serialize()`.
