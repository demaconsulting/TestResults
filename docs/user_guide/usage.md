# Usage

## Quick Start

The following snippet shows the minimum steps to create test results and serialize them to TRX and JUnit formats:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create a container for all test results
var results = new TestResults { Name = "SomeTests" };

// Add a passing test
results.Results.Add(new TestResult
{
    Name = "Test1",
    ClassName = "SomeTestClass",
    CodeBase = "MyTestAssembly",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(1.5),
    StartTime = DateTime.UtcNow
});

// Add a failing test
results.Results.Add(new TestResult
{
    Name = "Test2",
    ClassName = "SomeTestClass",
    CodeBase = "MyTestAssembly",
    Outcome = TestOutcome.Failed,
    ErrorMessage = "Expected value to be 42 but was 0",
    ErrorStackTrace = "at SomeTestClass.Test2() in Test.cs:line 15"
});

// Save as TRX (Visual Studio format)
File.WriteAllText("results.trx", TrxSerializer.Serialize(results));

// Save as JUnit XML
File.WriteAllText("results.xml", JUnitSerializer.Serialize(results));
```

## Test Outcomes

The `TestOutcome` enum represents every possible state a test can be in.

**Successful outcomes:**

- `Passed` — test passed successfully
- `PassedButRunAborted` — test passed but the run was aborted
- `Warning` — test passed with warnings

**Failure outcomes:**

- `Failed` — test failed with an assertion error
- `Error` — test encountered an unexpected error
- `Timeout` — test exceeded the timeout limit
- `Aborted` — test was aborted before completion

**Not-executed outcomes:**

- `NotExecuted` — test was not executed (skipped)
- `NotRunnable` — test cannot be run
- `Pending` — test is pending execution

**Other outcomes:**

- `Inconclusive` — result could not be determined
- `Disconnected` — test was disconnected during execution
- `InProgress` — test is currently executing
- `Completed` — test completed execution

The `TestOutcome` enum also provides helper extension methods:

- `IsPassed()` — returns `true` for `Passed`, `PassedButRunAborted`, and `Warning`
- `IsFailed()` — returns `true` for `Failed`, `Error`, `Timeout`, and `Aborted`
- `IsExecuted()` — returns `true` for all outcomes except `NotExecuted`, `NotRunnable`, and `Pending`

## Creating TRX Format Results

TRX format is used by Visual Studio, MSTest, and Azure DevOps:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = new TestResults
{
    Name = "Unit Tests",
    UserName = Environment.UserName
};

results.Results.Add(new TestResult
{
    Name = "CalculatorTests.TestAddition",
    ClassName = "MyApp.Tests.CalculatorTests",
    CodeBase = "MyApp.Tests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromMilliseconds(250),
    StartTime = DateTime.UtcNow
});

results.Results.Add(new TestResult
{
    Name = "CalculatorTests.TestDivision",
    ClassName = "MyApp.Tests.CalculatorTests",
    CodeBase = "MyApp.Tests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromMilliseconds(180),
    StartTime = DateTime.UtcNow
});

File.WriteAllText("results.trx", TrxSerializer.Serialize(results));
```

## Creating JUnit XML Format Results

JUnit XML is widely supported by Jenkins, GitHub Actions, GitLab CI, CircleCI, and many other CI/CD systems:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = new TestResults { Name = "Integration Tests" };

results.Results.Add(new TestResult
{
    Name = "DatabaseConnectionTest",
    ClassName = "IntegrationTests.DatabaseTests",
    CodeBase = "MyApp.IntegrationTests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(1.2)
});

File.WriteAllText("junit-results.xml", JUnitSerializer.Serialize(results));
```

## Handling Test Failures

Set `ErrorMessage` and `ErrorStackTrace` on failed test results:

```csharp
results.Results.Add(new TestResult
{
    Name = "FailingTest",
    ClassName = "MyTests",
    CodeBase = "MyAssembly",
    Outcome = TestOutcome.Failed,
    ErrorMessage = "Expected 42 but got 0",
    ErrorStackTrace = "   at MyTests.FailingTest() in Tests.cs:line 42\n" +
                     "   at TestRunner.Execute()",
    SystemError = "Additional diagnostic information"
});
```

Both TRX and JUnit formats preserve `ErrorMessage` and `ErrorStackTrace` when serializing.

## Capturing Standard Output and Error

Tests often produce console output that should be captured alongside the result:

```csharp
results.Results.Add(new TestResult
{
    Name = "TestWithOutput",
    ClassName = "OutputTests",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Passed,
    SystemOutput = "Debug: Starting test\nInfo: Processing data\nDebug: Test completed",
    Duration = TimeSpan.FromMilliseconds(200)
});

results.Results.Add(new TestResult
{
    Name = "TestWithWarnings",
    ClassName = "OutputTests",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Warning,
    SystemOutput = "Test execution started",
    SystemError = "Warning: Deprecated API usage detected\nWarning: Performance below threshold",
    Duration = TimeSpan.FromMilliseconds(300)
});
```

## Setting Test Metadata

Provide detailed metadata for each test to improve traceability:

```csharp
var testResult = new TestResult
{
    // Unique identifiers
    TestId = Guid.NewGuid(),
    ExecutionId = Guid.NewGuid(),

    // Test identification
    Name = "CompleteTest",
    ClassName = "MyNamespace.MyTestClass",
    CodeBase = "MyAssembly.dll",

    // Execution details
    ComputerName = Environment.MachineName,
    StartTime = DateTime.UtcNow,
    Duration = TimeSpan.FromSeconds(2.5),

    // Test outcome
    Outcome = TestOutcome.Passed,

    // Output
    SystemOutput = "Test output information"
};
```

## Automatic Format Detection

The `Serializer` class can automatically detect whether a file is TRX or JUnit without knowing the format in advance:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Identify the format without deserializing
string content = File.ReadAllText("test-results.xml");
TestResultFormat format = Serializer.Identify(content);

if (format == TestResultFormat.Trx)
    Console.WriteLine("TRX (Visual Studio) format detected");
else if (format == TestResultFormat.JUnit)
    Console.WriteLine("JUnit XML format detected");
else
    Console.WriteLine("Unknown or invalid format");

// Or deserialize directly — format is detected automatically
TestResults results = Serializer.Deserialize(content);
Console.WriteLine($"Loaded {results.Results.Count} test results from {results.Name}");
```

`Serializer.Identify()` analyzes the XML root element and namespace to distinguish TRX from JUnit. It returns
`TestResultFormat.Unknown` for invalid or unrecognized content. `Serializer.Deserialize()` calls `Identify()`
internally and dispatches to the appropriate format-specific deserializer.

## Converting Between Formats

The library supports bidirectional conversion between TRX and JUnit:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Convert TRX to JUnit using automatic detection
TestResults results = Serializer.Deserialize(File.ReadAllText("test-results.xml"));
File.WriteAllText("converted-to-junit.xml", JUnitSerializer.Serialize(results));

// Convert JUnit to TRX using the specific deserializer
TestResults fromJUnit = JUnitSerializer.Deserialize(File.ReadAllText("original-junit-results.xml"));
File.WriteAllText("converted-to-trx.trx", TrxSerializer.Serialize(fromJUnit));

// Convert TRX to JUnit using the specific deserializer
TestResults fromTrx = TrxSerializer.Deserialize(File.ReadAllText("original-trx-results.trx"));
File.WriteAllText("converted-to-junit.xml", JUnitSerializer.Serialize(fromTrx));
```

Conversion preserves all compatible information between formats: test names, outcomes, durations, error messages,
stack traces, and output. For format-specific fidelity limitations see the FAQ section.

## Complete Working Example

The following example demonstrates all major features of the library:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create test results container
var results = new TestResults
{
    Name = "Demo Test Suite",
    UserName = Environment.UserName
};

// Passing test with timing
results.Results.Add(new TestResult
{
    Name = "TestStringManipulation",
    ClassName = "StringTests",
    CodeBase = "DemoTests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromMilliseconds(45),
    StartTime = DateTime.UtcNow,
    SystemOutput = "All string operations validated successfully"
});

// Failing test with error details
results.Results.Add(new TestResult
{
    Name = "TestDatabaseConnection",
    ClassName = "DatabaseTests",
    CodeBase = "DemoTests",
    Outcome = TestOutcome.Failed,
    Duration = TimeSpan.FromSeconds(1.5),
    StartTime = DateTime.UtcNow,
    ErrorMessage = "Connection timeout: Unable to connect to database",
    ErrorStackTrace = "   at DatabaseHelper.Connect() in DbHelper.cs:line 67\n" +
                     "   at DatabaseTests.TestDatabaseConnection() in Tests.cs:line 123",
    SystemOutput = "Attempting to connect to database...",
    SystemError = "Connection attempt 1 failed\nConnection attempt 2 failed"
});

// Skipped test
results.Results.Add(new TestResult
{
    Name = "SkippedTest",
    ClassName = "IntegrationTests",
    CodeBase = "DemoTests",
    Outcome = TestOutcome.NotExecuted,
    SystemOutput = "Test skipped: integration environment not available"
});

// Save in both formats
File.WriteAllText("demo-results.trx", TrxSerializer.Serialize(results));
File.WriteAllText("demo-results.xml", JUnitSerializer.Serialize(results));

// Round-trip: read back the TRX file
TestResults readBack = TrxSerializer.Deserialize(File.ReadAllText("demo-results.trx"));
Console.WriteLine($"Read back {readBack.Results.Count} results from TRX");
```
