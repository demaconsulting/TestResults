# TestResults

The `TestResults` class represents a complete test run — a named collection of
`TestResult` objects along with run-level metadata such as a unique identifier,
a display name, and the user who initiated the run.

## TestResults Class

The `TestResults` class (`TestResults.cs`) is the top-level container returned by the IO
subsystem after deserializing a test result file, and passed to it when serializing.

### Properties

| Property   | Type                | Default          | Description                            |
|------------|---------------------|------------------|----------------------------------------|
| `Id`       | `Guid`              | `Guid.NewGuid()` | Uniquely identifies this test run      |
| `Name`     | `string`            | `string.Empty`   | Display name of the test run           |
| `UserName` | `string`            | `string.Empty`   | User or identity that initiated the run|
| `Results`  | `List<TestResult>`  | `[]`             | Ordered collection of test case results|

`Id` is auto-generated on construction so that every `TestResults` instance is uniquely
identifiable in TRX output without requiring callers to supply an identifier.

`Results` is initialized to an empty list, so callers can simply add items without
first checking for null.

## Related Requirements

Unit requirements are in
[docs/reqstream/test-results-library/test-results.yaml](../../reqstream/test-results-library/test-results.yaml).
