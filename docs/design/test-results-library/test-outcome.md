## TestOutcome

### Purpose

The TestOutcome unit defines the complete set of execution outcomes that the library can
represent and provides the classification rules used by serializers and consumers when they
need to count passed, failed, or not-executed tests.

### Data Model

**Outcome Values**: `TestOutcome` - Enumerates `Error`, `Failed`, `Timeout`, `Aborted`,
`Inconclusive`, `PassedButRunAborted`, `NotRunnable`, `NotExecuted`, `Disconnected`,
`Warning`, `Passed`, `Completed`, `InProgress`, and `Pending`.

**Classification Rules**: `TestOutcomeExtensions` - Encodes the invariants that only
`Passed`, `PassedButRunAborted`, and `Warning` are passed; only `Failed`, `Error`,
`Timeout`, and `Aborted` are failed; and only `NotRunnable`, `NotExecuted`, and `Pending`
count as not executed.

### Key Methods

**IsPassed**: Classifies an outcome as passed or not passed.

- *Parameters*: `TestOutcome outcome` - Outcome value to classify.
- *Returns*: `bool` - `true` only for `Passed`, `PassedButRunAborted`, and `Warning`.
- *Preconditions*: None.
- *Postconditions*: Returns a deterministic passed/not-passed classification without
  mutating state.

The method uses a switch expression so serializers can count passed tests without duplicating
outcome tables.

**IsFailed**: Classifies an outcome as failed or not failed.

- *Parameters*: `TestOutcome outcome` - Outcome value to classify.
- *Returns*: `bool` - `true` only for `Failed`, `Error`, `Timeout`, and `Aborted`.
- *Preconditions*: None.
- *Postconditions*: Returns a deterministic failed/not-failed classification without
  mutating state.

The method centralizes failure semantics used by TRX summary counters and by callers that
aggregate failures.

**IsExecuted**: Classifies whether a test was actually executed.

- *Parameters*: `TestOutcome outcome` - Outcome value to classify.
- *Returns*: `bool` - `false` only for `NotRunnable`, `NotExecuted`, and `Pending`.
- *Preconditions*: None.
- *Postconditions*: Returns a deterministic executed/not-executed classification without
  mutating state.

The method lets JUnit serialization map all not-executed outcomes to `skipped` and lets TRX
summary counters distinguish executed tests from scheduled or skipped ones.

### Error Handling

The unit does not throw exceptions during normal classification. The enum constrains ordinary
callers to the defined outcome set, and the extension methods return a deterministic boolean
result for every supplied enum value.

### Dependencies

N/A - this unit is a standalone enum and static helper class with no local software-item
dependencies.

### Callers

- **TestResult** - stores the selected outcome for each test execution.
- **TrxSerializer** - reads and writes TRX outcome values and computes summary counters.
- **JUnitSerializer** - maps model outcomes to JUnit `failure`, `error`, and `skipped`
  elements.
- **Library consumers** - inspect outcome values and extension methods through the public API.
