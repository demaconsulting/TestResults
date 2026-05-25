## TestResult

### Purpose

The TestResult unit represents one executed or scheduled test case. It stores identity,
location, timing, output, outcome, and failure details in the format-neutral model shared by
all serializers.

### Data Model

**TestId**: `Guid` - Unique identifier for the test definition. Defaults to a new GUID so TRX
cross-references can be generated without caller-supplied IDs.

**ExecutionId**: `Guid` - Unique identifier for the specific execution instance. Defaults to a
new GUID for the same reason.

**Name**: `string` - Display name of the test case. Defaults to `string.Empty`.

**CodeBase**: `string` - Assembly or storage path associated with the test definition.
Defaults to `string.Empty`.

**ClassName**: `string` - Fully-qualified or logical class name used for grouping and
reporting. Defaults to `string.Empty`.

**ComputerName**: `string` - Name of the machine that ran the test. Defaults to
`Environment.MachineName`.

**StartTime**: `DateTime` - Execution start timestamp. Defaults to `DateTime.UtcNow` when the
instance is created.

**Duration**: `TimeSpan` - Execution duration. Defaults to `TimeSpan.Zero`.

**SystemOutput**: `string` - Captured standard output stream. Defaults to `string.Empty`.

**SystemError**: `string` - Captured standard error stream. Defaults to `string.Empty`.

**Outcome**: `TestOutcome` - Semantic result classification. Defaults to
`TestOutcome.NotExecuted`.

**ErrorMessage**: `string` - Failure or skip summary text. Defaults to `string.Empty`.

**ErrorStackTrace**: `string` - Failure stack trace text. Defaults to `string.Empty`.

### Key Methods

N/A - this unit is a data container with auto-properties and no behavioral methods.

### Error Handling

The class does not validate assignments or throw during ordinary use. Constructor-time
defaults keep string properties non-null, initialize identifiers automatically, and ensure the
instance always starts in a valid state for serialization.

### Dependencies

- **TestOutcome** - provides the outcome type stored in `Outcome`.
- **System.Environment** - supplies the default computer name.
- **System.Guid**, **System.DateTime**, and **System.TimeSpan** - provide identity and timing
  value types.

### Callers

- **TestResults** - stores ordered `TestResult` instances for a run.
- **TrxSerializer** - populates and consumes all execution metadata during TRX translation.
- **JUnitSerializer** - populates and consumes the subset of fields representable in JUnit
  XML.
- **Library consumers** - create and inspect individual test results through the public API.
