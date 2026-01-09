# TestResults Library - Developer Guide

A comprehensive guide to using the TestResults library for creating and managing test result files in C#.

## Quick Navigation

This section provides quick links to major sections when viewing this guide on GitHub. A complete table of contents is available in the generated HTML and PDF versions.

- [Introduction](#introduction)
- [Prerequisites](#prerequisites)
- [Adding to Your Project](#adding-to-your-project)
- [Creating Test Results](#creating-test-results)
- [Advanced Usage](#advanced-usage)
- [FAQ](#faq)

## Introduction

### What is TestResults?

The TestResults library is a lightweight C# library designed for programmatically creating test result files. It
provides a simple, type-safe API for generating test results in both TRX (Test Results) and JUnit XML formats.

### When to Use TestResults

This library is ideal for:

- **Custom Test Runners**: Building your own test execution framework that needs to output standard test result files
- **Format Conversion**: Converting test results between TRX and JUnit formats for different CI/CD systems
- **Test Report Aggregation**: Combining test results from multiple sources into a unified report
- **CI/CD Integration**: Creating test reports that integrate with Visual Studio, Azure DevOps, GitHub Actions,
  Jenkins, and other CI/CD platforms
- **Test Result Generation**: Generating test result files from non-standard testing scenarios or custom validation
  tools

### Supported Formats

- **TRX (Test Results)**: XML-based format used by Visual Studio, Azure DevOps, and Microsoft testing tools
- **JUnit XML**: Widely-supported format compatible with Jenkins, GitHub Actions, and many other CI/CD systems

### Key Features

- ✨ **Simple API**: Intuitive object model with minimal learning curve
- 🎯 **Type-Safe**: Strongly-typed C# classes with compile-time safety
- 🪶 **Zero Dependencies**: No runtime dependencies to minimize conflicts
- 🔄 **Bidirectional Conversion**: Read and convert between TRX and JUnit formats
- 📦 **Easy Integration**: Available as a NuGet package
- ✅ **Standards Compliant**: Generates files compatible with industry-standard tools

## Prerequisites

### Required Tools

- **.NET SDK**: Version 8.0, 9.0, or 10.0
- **Development Environment**: Visual Studio 2022, Visual Studio Code, or JetBrains Rider (any modern C# IDE)

### Required Knowledge

- Basic understanding of C# programming
- Familiarity with .NET project structure
- Understanding of test result concepts (test cases, outcomes, assertions)

### Compatibility

The TestResults library targets:

- .NET 8.0
- .NET 9.0
- .NET 10.0

It can be used in any .NET project type: console applications, class libraries, ASP.NET Core applications, etc.

## Adding to Your Project

### Using .NET CLI

The recommended way to install the TestResults library is using the .NET CLI:

```bash
dotnet add package DemaConsulting.TestResults
```

This command adds the latest stable version to your project file (.csproj).

#### Installing a Specific Version

To install a specific version:

```bash
dotnet add package DemaConsulting.TestResults --version 1.0.0
```

### Using Visual Studio Package Manager Console

If you're using Visual Studio, you can install the package via the Package Manager Console:

```powershell
Install-Package DemaConsulting.TestResults
```

For a specific version:

```powershell
Install-Package DemaConsulting.TestResults -Version 1.0.0
```

### Using Visual Studio NuGet Package Manager GUI

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages..."
3. Click the "Browse" tab
4. Search for "DemaConsulting.TestResults"
5. Select the package and click "Install"

### Manual Package Reference

You can also manually add the package reference to your .csproj file:

```xml
<ItemGroup>
  <PackageReference Include="DemaConsulting.TestResults" Version="1.0.0" />
</ItemGroup>
```

After adding the reference manually, run:

```bash
dotnet restore
```

## Creating Test Results

### Basic Test Result Creation

Here's a simple example showing the core concepts:

```csharp
using System;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create a container for all test results
var testResults = new TestResults
{
    Name = "MyTestSuite"
};

// Create a passing test
var passingTest = new TestResult
{
    Name = "TestAddition",
    ClassName = "MathTests",
    CodeBase = "MyTestAssembly",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(0.5)
};

// Add the test to the results
testResults.Results.Add(passingTest);

// Serialize to TRX format
string trxContent = TrxSerializer.Serialize(testResults);

// Save to file
File.WriteAllText("test-results.trx", trxContent);

Console.WriteLine("Test results saved to test-results.trx");
```

### Creating TRX Format Results

TRX (Test Results) format is used by Visual Studio and Azure DevOps:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create test results container
var results = new TestResults
{
    Name = "Unit Tests",
    UserName = Environment.UserName
};

// Add multiple test results
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

// Serialize to TRX and save
string trxXml = TrxSerializer.Serialize(results);
File.WriteAllText("results.trx", trxXml);

Console.WriteLine($"Created TRX file with {results.Results.Count} test results");
```

### Creating JUnit Format Results

JUnit XML format is widely supported by CI/CD systems like Jenkins and GitHub Actions:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create test results container
var results = new TestResults
{
    Name = "Integration Tests"
};

// Add test results
results.Results.Add(new TestResult
{
    Name = "DatabaseConnectionTest",
    ClassName = "IntegrationTests.DatabaseTests",
    CodeBase = "MyApp.IntegrationTests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(1.2)
});

results.Results.Add(new TestResult
{
    Name = "ApiEndpointTest",
    ClassName = "IntegrationTests.ApiTests",
    CodeBase = "MyApp.IntegrationTests",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(0.8)
});

// Serialize to JUnit XML and save
string junitXml = JUnitSerializer.Serialize(results);
File.WriteAllText("junit-results.xml", junitXml);

Console.WriteLine($"Created JUnit XML file with {results.Results.Count} test results");
```

### Handling Different Test Outcomes

The library supports various test outcomes to represent different test states:

```csharp
using System;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = new TestResults { Name = "Comprehensive Test Suite" };

// Passed test
results.Results.Add(new TestResult
{
    Name = "SuccessfulTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromMilliseconds(100)
});

// Failed test with error details
results.Results.Add(new TestResult
{
    Name = "FailedTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Failed,
    ErrorMessage = "Expected value to be 42 but was 0",
    ErrorStackTrace = "   at TestClass.FailedTest() in Tests.cs:line 15\n" +
                     "   at System.RuntimeMethodHandle.InvokeMethod()",
    Duration = TimeSpan.FromMilliseconds(150)
});

// Inconclusive test
results.Results.Add(new TestResult
{
    Name = "InconclusiveTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Inconclusive,
    Duration = TimeSpan.FromMilliseconds(50)
});

// Test that timed out
results.Results.Add(new TestResult
{
    Name = "SlowTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Timeout,
    ErrorMessage = "Test exceeded timeout of 5000ms",
    Duration = TimeSpan.FromMilliseconds(5000)
});

// Test that was not executed
results.Results.Add(new TestResult
{
    Name = "SkippedTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.NotExecuted
});

// Aborted test
results.Results.Add(new TestResult
{
    Name = "AbortedTest",
    ClassName = "TestClass",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Aborted,
    ErrorMessage = "Test run was aborted by user"
});

// Save as TRX
File.WriteAllText("comprehensive-results.trx", TrxSerializer.Serialize(results));
```

### Available Test Outcomes

The `TestOutcome` enum includes the following values:

- **`Passed`**: Test passed successfully
- **`Failed`**: Test failed with an assertion error
- **`Error`**: Test encountered an error during execution
- **`Timeout`**: Test exceeded the timeout limit
- **`Aborted`**: Test was aborted before completion
- **`Inconclusive`**: Test result was inconclusive
- **`NotExecuted`**: Test was not executed (skipped)
- **`PassedButRunAborted`**: Test passed but the test run was aborted
- **`NotRunnable`**: Test cannot be run
- **`Disconnected`**: Test was disconnected during execution
- **`Warning`**: Test passed with warnings
- **`Completed`**: Test completed execution
- **`InProgress`**: Test is currently executing
- **`Pending`**: Test is pending execution

### Capturing Standard Output and Error

Tests often produce console output that should be captured:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = new TestResults { Name = "Tests with Output" };

// Test with standard output
results.Results.Add(new TestResult
{
    Name = "TestWithOutput",
    ClassName = "OutputTests",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Passed,
    SystemOutput = "Debug: Starting test\n" +
                  "Info: Processing data\n" +
                  "Debug: Test completed successfully",
    Duration = TimeSpan.FromMilliseconds(200)
});

// Test with standard error
results.Results.Add(new TestResult
{
    Name = "TestWithWarnings",
    ClassName = "OutputTests",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Warning,
    SystemOutput = "Test execution started",
    SystemError = "Warning: Deprecated API usage detected\n" +
                 "Warning: Performance below threshold",
    Duration = TimeSpan.FromMilliseconds(300)
});

// Failed test with both output and error
results.Results.Add(new TestResult
{
    Name = "TestWithFailure",
    ClassName = "OutputTests",
    CodeBase = "TestAssembly",
    Outcome = TestOutcome.Failed,
    SystemOutput = "Test setup completed\nExecuting test logic",
    SystemError = "Error: Connection refused",
    ErrorMessage = "Failed to connect to test database",
    ErrorStackTrace = "   at DatabaseHelper.Connect() in Helper.cs:line 42",
    Duration = TimeSpan.FromMilliseconds(500)
});

// Save results
File.WriteAllText("output-results.trx", TrxSerializer.Serialize(results));
```

### Setting Test Metadata

Provide detailed metadata for each test:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = new TestResults
{
    Id = Guid.NewGuid(),
    Name = "Full Metadata Example",
    UserName = "TestUser"
};

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
    
    // Output and errors (if any)
    SystemOutput = "Test output information",
    SystemError = "",
    ErrorMessage = "",
    ErrorStackTrace = ""
};

results.Results.Add(testResult);

// Save as both formats
File.WriteAllText("metadata-results.trx", TrxSerializer.Serialize(results));
File.WriteAllText("metadata-results.xml", JUnitSerializer.Serialize(results));
```

### Converting Between Formats

The library makes it easy to convert test results between TRX and JUnit formats:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Convert JUnit to TRX
string junitXml = File.ReadAllText("original-junit-results.xml");
TestResults resultsFromJUnit = JUnitSerializer.Deserialize(junitXml);
string trxXml = TrxSerializer.Serialize(resultsFromJUnit);
File.WriteAllText("converted-to-trx.trx", trxXml);
Console.WriteLine("Converted JUnit to TRX format");

// Convert TRX to JUnit
string trxXml2 = File.ReadAllText("original-trx-results.trx");
TestResults resultsFromTrx = TrxSerializer.Deserialize(trxXml2);
string junitXml2 = JUnitSerializer.Serialize(resultsFromTrx);
File.WriteAllText("converted-to-junit.xml", junitXml2);
Console.WriteLine("Converted TRX to JUnit format");
```

### Complete Working Example

Here's a comprehensive example that demonstrates all major features:

```csharp
using System;
using System.IO;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

namespace TestResultsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create test results container
            var results = new TestResults
            {
                Name = "Demo Test Suite",
                UserName = Environment.UserName
            };

            // Add a variety of test results
            AddPassingTests(results);
            AddFailingTests(results);
            AddSpecialCaseTests(results);

            // Save results in both formats
            SaveResults(results);

            // Demonstrate format conversion
            DemonstrateConversion();

            Console.WriteLine("Demo completed successfully!");
        }

        static void AddPassingTests(TestResults results)
        {
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

            results.Results.Add(new TestResult
            {
                Name = "TestMathOperations",
                ClassName = "MathTests",
                CodeBase = "DemoTests",
                Outcome = TestOutcome.Passed,
                Duration = TimeSpan.FromMilliseconds(30),
                StartTime = DateTime.UtcNow
            });
        }

        static void AddFailingTests(TestResults results)
        {
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

            results.Results.Add(new TestResult
            {
                Name = "TestApiValidation",
                ClassName = "ApiTests",
                CodeBase = "DemoTests",
                Outcome = TestOutcome.Error,
                Duration = TimeSpan.FromMilliseconds(200),
                StartTime = DateTime.UtcNow,
                ErrorMessage = "NullReferenceException: Object reference not set",
                ErrorStackTrace = "   at ApiValidator.Validate() in Validator.cs:line 34"
            });
        }

        static void AddSpecialCaseTests(TestResults results)
        {
            results.Results.Add(new TestResult
            {
                Name = "TestWithTimeout",
                ClassName = "PerformanceTests",
                CodeBase = "DemoTests",
                Outcome = TestOutcome.Timeout,
                Duration = TimeSpan.FromSeconds(10),
                StartTime = DateTime.UtcNow,
                ErrorMessage = "Test exceeded maximum duration of 10 seconds"
            });

            results.Results.Add(new TestResult
            {
                Name = "SkippedTest",
                ClassName = "IntegrationTests",
                CodeBase = "DemoTests",
                Outcome = TestOutcome.NotExecuted,
                SystemOutput = "Test skipped: Integration environment not available"
            });

            results.Results.Add(new TestResult
            {
                Name = "InconclusiveTest",
                ClassName = "ExperimentalTests",
                CodeBase = "DemoTests",
                Outcome = TestOutcome.Inconclusive,
                Duration = TimeSpan.FromMilliseconds(100),
                StartTime = DateTime.UtcNow,
                SystemOutput = "Test result could not be determined due to external factors"
            });
        }

        static void SaveResults(TestResults results)
        {
            // Save as TRX format
            string trxContent = TrxSerializer.Serialize(results);
            File.WriteAllText("demo-results.trx", trxContent);
            Console.WriteLine($"Saved {results.Results.Count} test results to demo-results.trx");

            // Save as JUnit XML format
            string junitContent = JUnitSerializer.Serialize(results);
            File.WriteAllText("demo-results.xml", junitContent);
            Console.WriteLine($"Saved {results.Results.Count} test results to demo-results.xml");
        }

        static void DemonstrateConversion()
        {
            // Read TRX and convert to JUnit
            string trxXml = File.ReadAllText("demo-results.trx");
            TestResults fromTrx = TrxSerializer.Deserialize(trxXml);
            string convertedJUnit = JUnitSerializer.Serialize(fromTrx);
            File.WriteAllText("converted-from-trx.xml", convertedJUnit);
            Console.WriteLine("Converted TRX to JUnit format");

            // Read JUnit and convert to TRX
            string junitXml = File.ReadAllText("demo-results.xml");
            TestResults fromJUnit = JUnitSerializer.Deserialize(junitXml);
            string convertedTrx = TrxSerializer.Serialize(fromJUnit);
            File.WriteAllText("converted-from-junit.trx", convertedTrx);
            Console.WriteLine("Converted JUnit to TRX format");
        }
    }
}
```

## Advanced Usage

### Using TestOutcome Extension Methods

The library provides extension methods to check test outcome categories:

```csharp
using System;
using System.Linq;
using DemaConsulting.TestResults;

var results = new TestResults { Name = "Test Analysis" };

// Add various test results
results.Results.Add(new TestResult { Name = "Test1", Outcome = TestOutcome.Passed });
results.Results.Add(new TestResult { Name = "Test2", Outcome = TestOutcome.Failed });
results.Results.Add(new TestResult { Name = "Test3", Outcome = TestOutcome.NotExecuted });
results.Results.Add(new TestResult { Name = "Test4", Outcome = TestOutcome.Warning });

// Analyze results using extension methods
int passedCount = results.Results.Count(r => r.Outcome.IsPassed());
int failedCount = results.Results.Count(r => r.Outcome.IsFailed());
int executedCount = results.Results.Count(r => r.Outcome.IsExecuted());

Console.WriteLine($"Passed: {passedCount}");      // Includes Passed, Warning, PassedButRunAborted
Console.WriteLine($"Failed: {failedCount}");      // Includes Failed, Error, Timeout, Aborted
Console.WriteLine($"Executed: {executedCount}");  // All except NotExecuted, NotRunnable, Pending

// Calculate success rate
double successRate = executedCount > 0 
    ? (double)passedCount / executedCount * 100 
    : 0;
Console.WriteLine($"Success Rate: {successRate:F2}%");
```

### Aggregating Test Results from Multiple Sources

Combine test results from different test suites or runs:

```csharp
using System;
using System.IO;
using System.Linq;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Read multiple test result files
var unitTestsXml = File.ReadAllText("unit-tests.xml");
var integrationTestsXml = File.ReadAllText("integration-tests.xml");
var e2eTestsTrx = File.ReadAllText("e2e-tests.trx");

// Deserialize each file
var unitTests = JUnitSerializer.Deserialize(unitTestsXml);
var integrationTests = JUnitSerializer.Deserialize(integrationTestsXml);
var e2eTests = TrxSerializer.Deserialize(e2eTestsTrx);

// Create combined results
var combinedResults = new TestResults
{
    Name = "All Tests Combined",
    UserName = Environment.UserName
};

// Add all results to the combined collection
combinedResults.Results.AddRange(unitTests.Results);
combinedResults.Results.AddRange(integrationTests.Results);
combinedResults.Results.AddRange(e2eTests.Results);

// Save the combined results
File.WriteAllText("combined-results.trx", TrxSerializer.Serialize(combinedResults));
File.WriteAllText("combined-results.xml", JUnitSerializer.Serialize(combinedResults));

Console.WriteLine($"Combined {combinedResults.Results.Count} tests from 3 sources");
```

### Custom Test Result Filtering

Filter and save subsets of test results:

```csharp
using System;
using System.IO;
using System.Linq;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Load test results
var allTestsXml = File.ReadAllText("all-tests.xml");
var allTests = JUnitSerializer.Deserialize(allTestsXml);

// Filter for failed tests only
var failedResults = new TestResults
{
    Name = "Failed Tests Only",
    Results = allTests.Results
        .Where(r => r.Outcome.IsFailed())
        .ToList()
};

// Save failed tests report
File.WriteAllText("failed-tests.xml", JUnitSerializer.Serialize(failedResults));

// Filter by test class
var databaseTestResults = new TestResults
{
    Name = "Database Tests",
    Results = allTests.Results
        .Where(r => r.ClassName.Contains("DatabaseTests"))
        .ToList()
};

File.WriteAllText("database-tests.xml", JUnitSerializer.Serialize(databaseTestResults));

// Filter by duration (find slow tests)
var slowTests = new TestResults
{
    Name = "Slow Tests (>1 second)",
    Results = allTests.Results
        .Where(r => r.Duration.TotalSeconds > 1)
        .OrderByDescending(r => r.Duration)
        .ToList()
};

File.WriteAllText("slow-tests.xml", JUnitSerializer.Serialize(slowTests));

Console.WriteLine($"Found {failedResults.Results.Count} failed tests");
Console.WriteLine($"Found {slowTests.Results.Count} slow tests");
```

### Generating Summary Reports

Create summary statistics from test results:

```csharp
using System;
using System.IO;
using System.Linq;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Load test results
var resultsXml = File.ReadAllText("test-results.xml");
var results = JUnitSerializer.Deserialize(resultsXml);

// Calculate statistics
var total = results.Results.Count;
var passed = results.Results.Count(r => r.Outcome.IsPassed());
var failed = results.Results.Count(r => r.Outcome.IsFailed());
var skipped = results.Results.Count(r => !r.Outcome.IsExecuted());
var totalDuration = TimeSpan.FromTicks(results.Results.Sum(r => r.Duration.Ticks));
var avgDuration = results.Results.Count > 0
    ? TimeSpan.FromTicks(results.Results.Average(r => r.Duration.Ticks))
    : TimeSpan.Zero;

// Generate summary report
var summary = $@"
Test Execution Summary
=====================
Total Tests:    {total}
Passed:         {passed} ({(total > 0 ? (double)passed / total * 100 : 0):F1}%)
Failed:         {failed} ({(total > 0 ? (double)failed / total * 100 : 0):F1}%)
Skipped:        {skipped} ({(total > 0 ? (double)skipped / total * 100 : 0):F1}%)

Timing:
Total Duration: {totalDuration:hh\\:mm\\:ss\\.fff}
Avg Duration:   {avgDuration:mm\\:ss\\.fff}

Status: {(failed == 0 ? "SUCCESS" : "FAILURE")}
";

Console.WriteLine(summary);
File.WriteAllText("test-summary.txt", summary);
```

## FAQ

### What test result formats are supported?

The TestResults library currently supports two formats:

- **TRX (Test Results)**: XML format used by Visual Studio, MSTest, Azure DevOps, and other Microsoft testing tools
- **JUnit XML**: Widely supported format compatible with Jenkins, GitHub Actions, GitLab CI, CircleCI, and many other
  CI/CD systems

Both formats can be created from scratch or converted from one to the other.

### Can I convert between TRX and JUnit formats?

Yes! The library provides bidirectional conversion between formats:

```csharp
// TRX to JUnit
var trxResults = TrxSerializer.Deserialize(File.ReadAllText("test.trx"));
var junitXml = JUnitSerializer.Serialize(trxResults);

// JUnit to TRX
var junitResults = JUnitSerializer.Deserialize(File.ReadAllText("test.xml"));
var trxXml = TrxSerializer.Serialize(junitResults);
```

The conversion preserves all compatible information between formats, including test names, outcomes, durations, error
messages, and output.

### How do I set custom properties or metadata?

Currently, the library supports the standard metadata fields available in `TestResults` and `TestResult` classes:

- Test identification (Name, ClassName, CodeBase)
- Execution details (StartTime, Duration, ComputerName)
- Results (Outcome, ErrorMessage, ErrorStackTrace)
- Output (SystemOutput, SystemError)
- Unique identifiers (Id, TestId, ExecutionId)

For additional custom properties, you may need to extend the classes or use the SystemOutput field to include extra
information.

### What test outcomes are available?

The `TestOutcome` enum provides comprehensive outcome values:

**Success States:**

- `Passed` - Test passed successfully
- `Warning` - Test passed with warnings
- `PassedButRunAborted` - Test passed but run was aborted
- `Completed` - Test completed

**Failure States:**

- `Failed` - Test failed
- `Error` - Test encountered an error
- `Timeout` - Test exceeded time limit
- `Aborted` - Test was aborted

**Not Executed States:**

- `NotExecuted` - Test was not executed/skipped
- `NotRunnable` - Test cannot be run
- `Pending` - Test is pending

**Other States:**

- `Inconclusive` - Result was inconclusive
- `Disconnected` - Test was disconnected
- `InProgress` - Test is running

### How do I handle test failures with error details?

Set the `ErrorMessage` and `ErrorStackTrace` properties on failed tests:

```csharp
var failedTest = new TestResult
{
    Name = "MyFailingTest",
    ClassName = "MyTests",
    CodeBase = "MyAssembly",
    Outcome = TestOutcome.Failed,
    ErrorMessage = "Expected 42 but got 0",
    ErrorStackTrace = "   at MyTests.MyFailingTest() in Tests.cs:line 50\n" +
                     "   at TestRunner.Execute()",
    SystemError = "Additional diagnostic information"
};
```

Both TRX and JUnit formats preserve this information when serializing.

### Is the library thread-safe?

The `TestResults` and `TestResult` classes are simple data transfer objects (DTOs) and are not inherently thread-safe.
If you need to add test results from multiple threads, you should use appropriate synchronization:

```csharp
var results = new TestResults { Name = "Parallel Tests" };
var lockObject = new object();

Parallel.ForEach(testCases, testCase =>
{
    var result = ExecuteTest(testCase);
    
    lock (lockObject)
    {
        results.Results.Add(result);
    }
});
```

The serializer classes (`TrxSerializer` and `JUnitSerializer`) are stateless and safe to use from multiple threads
simultaneously.

### Where are the test result files saved?

The library **does not** perform any file I/O operations. You are responsible for:

- Choosing where to save files
- Creating directories if needed
- Writing the serialized content to files
- Reading files for deserialization

This design gives you full control over file locations and error handling:

```csharp
// You control the file path
var outputPath = Path.Combine("TestResults", "results.trx");
Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

// You perform the file I/O
var xml = TrxSerializer.Serialize(results);
File.WriteAllText(outputPath, xml);
```

### Can I use this library with MSTest, NUnit, or xUnit?

The TestResults library is **not** a test framework replacement. It's designed for scenarios where you need to
programmatically **generate** test result files, such as:

- Custom test runners
- Test result aggregation tools
- Format conversion utilities
- CI/CD integration scripts

If you're using MSTest, NUnit, or xUnit, those frameworks already generate their own test result files, and you
typically won't need this library unless you're building tools that process or convert those results.

### How do I handle large numbers of test results?

For large test suites, consider these approaches:

**Batch Processing:**

```csharp
const int batchSize = 1000;
var allResults = GetAllTestResults(); // Your test results

for (int i = 0; i < allResults.Count; i += batchSize)
{
    var batch = new TestResults
    {
        Name = $"TestBatch_{i / batchSize + 1}",
        Results = allResults.Skip(i).Take(batchSize).ToList()
    };
    
    var xml = TrxSerializer.Serialize(batch);
    File.WriteAllText($"results-batch-{i / batchSize + 1}.trx", xml);
}
```

**Memory-Efficient Processing:**

```csharp
// Process results as they're generated instead of collecting all in memory
var results = new TestResults { Name = "Streaming Tests" };

foreach (var testCase in testCases)
{
    var result = ExecuteTest(testCase);
    results.Results.Add(result);
    
    // Periodically flush to file if needed
    if (results.Results.Count >= 100)
    {
        AppendToFile("results.trx", results);
        results.Results.Clear();
    }
}
```

### What .NET versions are supported?

The library targets:

- .NET 8.0
- .NET 9.0
- .NET 10.0

It uses modern C# features and follows current .NET best practices. The library has **zero runtime dependencies**,
making it easy to integrate into any .NET project.

### How do I report bugs or request features?

- **Report a Bug**: [Create an issue on GitHub](https://github.com/demaconsulting/TestResults/issues/new?labels=bug)
- **Request a Feature**: [Create an issue on GitHub](https://github.com/demaconsulting/TestResults/issues/new?labels=enhancement)
- **Ask Questions**: [Start a discussion on GitHub](https://github.com/demaconsulting/TestResults/discussions)

### Where can I find more information?

- **GitHub Repository**: <https://github.com/demaconsulting/TestResults>
- **NuGet Package**: <https://www.nuget.org/packages/DemaConsulting.TestResults>
- **Architecture Documentation**: [ARCHITECTURE.md](https://github.com/demaconsulting/TestResults/blob/main/ARCHITECTURE.md)
- **Contributing Guide**: [CONTRIBUTING.md](https://github.com/demaconsulting/TestResults/blob/main/CONTRIBUTING.md)

## Next Steps

Now that you've learned how to use the TestResults library, you can:

1. **Integrate into your project**: Add the NuGet package and start generating test results
2. **Explore the examples**: Try the code samples in this guide with your own data
3. **Review the architecture**: Read [ARCHITECTURE.md](https://github.com/demaconsulting/TestResults/blob/main/ARCHITECTURE.md)
   for design details
4. **Contribute**: Check [CONTRIBUTING.md](https://github.com/demaconsulting/TestResults/blob/main/CONTRIBUTING.md)
   to contribute improvements
5. **Get support**: Ask questions in [GitHub Discussions](https://github.com/demaconsulting/TestResults/discussions)

Happy testing! 🎉
