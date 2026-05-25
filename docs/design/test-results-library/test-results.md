## TestResults

### Purpose

The TestResults unit represents a complete test run. It groups ordered `TestResult`
instances with run-level identity and metadata so serializers and callers can treat a whole
run as one logical object.

### Data Model

**Id**: `Guid` - Unique identifier for the run. Defaults to a new GUID so TRX output always
has a stable `TestRun/@id`.

**Name**: `string` - Human-readable run or suite name. Defaults to `string.Empty`.

**UserName**: `string` - Identity that initiated the run. Defaults to `string.Empty`.

**Results**: `List<TestResult>` - Ordered collection of individual test results. Defaults to an
empty list so callers can add results without null checks.

### Key Methods

N/A - this unit is a data container with auto-properties and no behavioral methods.

### Error Handling

The class does not perform validation or throw during ordinary use. Default values ensure the
collection is always initialized and run metadata is always present in a serializable form.

### Dependencies

- **TestResult** - provides the element type stored in `Results`.
- **System.Guid** - provides the run identifier type and default GUID generation.

### Callers

- **Serializer** - returns `TestResults` from format-agnostic deserialization.
- **TrxSerializer** - serializes the run to TRX and reconstructs it from TRX input.
- **JUnitSerializer** - serializes the run to JUnit XML and reconstructs it from JUnit input.
- **Library consumers** - build and inspect complete test runs through the public API.
