# TestResults

The `TestResults` class represents a complete test run — a named collection of
`TestResult` objects along with run-level metadata.

## TestResults Class

The `TestResults` class (`TestResults.cs`) represents a complete test run — a named
collection of `TestResult` objects along with run-level metadata.

### Properties

| Property   | Type                | Default          | Description                            |
|------------|---------------------|------------------|----------------------------------------|
| `Id`       | `Guid`              | `Guid.NewGuid()` | Uniquely identifies this test run      |
| `Name`     | `string`            | `string.Empty`   | Display name of the test run           |
| `UserName` | `string`            | `string.Empty`   | User or identity that initiated the run|
| `Results`  | `List<TestResult>`  | `[]`             | Ordered collection of test case results|

`Id` is auto-generated on construction for the same reasons as `TestResult.TestId` — it
ensures every run is uniquely identifiable in TRX output without requiring callers to
supply an identifier.

`Results` is initialized to an empty list, so callers can simply add items without
first checking for null.
