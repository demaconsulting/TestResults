# Serializer

The `Serializer` class is a facade that auto-detects the format of a serialized test
result document and delegates reading to the appropriate format-specific serializer.
It exposes the `TestResultFormat` enumeration for format identification, the
`Serializer.Identify()` method for probing document format, and the
`Serializer.Deserialize()` method for reading a test result document regardless of
its format.

## TestResultFormat Enumeration

The `TestResultFormat` enumeration identifies the file format of a test result document.

| Value     | Description                                                       |
|-----------|-------------------------------------------------------------------|
| `Unknown` | Format could not be determined                                    |
| `Trx`     | Visual Studio Test Results (TRX) XML format                       |
| `JUnit`   | JUnit XML format                                                  |

## Format Identification

The `Serializer.Identify()` method determines the format of a serialized test result
document without fully deserializing it. This satisfies requirement
`TestResults-Serializer-FormatIdentify`.

The identification algorithm:

1. Returns `TestResultFormat.Unknown` immediately if the input is null or whitespace
2. Attempts to parse the full XML document; returns `TestResultFormat.Unknown` if
   parsing fails with an `XmlException` (the input is not valid XML)
3. Returns `TestResultFormat.Trx` if the root element name is `TestRun` **and** the
   namespace URI is `http://microsoft.com/schemas/VisualStudio/TeamTest/2010`
4. Returns `TestResultFormat.JUnit` if the root element name is `testsuites` or
   `testsuite` (case-sensitive, no namespace required)
5. Returns `TestResultFormat.Unknown` for any other document structure

Using the XML namespace for TRX detection makes identification unambiguous — a document
with a `TestRun` root element in any other namespace is not treated as TRX.

## Format Conversion

The `Serializer.Deserialize()` method provides a single entry point for reading test
result files regardless of their format. This satisfies requirement
`TestResults-Serializer-FormatConversion`.

The conversion algorithm:

1. Calls `Serializer.Identify()` to determine the format of the input document
2. Delegates to `TrxSerializer.Deserialize()` for `TestResultFormat.Trx`
3. Delegates to `JUnitSerializer.Deserialize()` for `TestResultFormat.JUnit`
4. Throws an exception for `TestResultFormat.Unknown`

This design means that callers do not need to know or specify the format — they simply
pass the raw content and receive a `TestResults` object.

## SerializerHelpers Dependency

The `Serializer.Deserialize()` method delegates writing to format-specific serializers
that depend on the [SerializerHelpers](serializer-helpers.md) unit for UTF-8 output
encoding. See [serializer-helpers.md](serializer-helpers.md) for details.
