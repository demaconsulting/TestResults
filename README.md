# TestResults Library

![GitHub forks](https://img.shields.io/github/forks/demaconsulting/TestResults?style=plastic)
![GitHub Repo stars](https://img.shields.io/github/stars/demaconsulting/TestResults?style=plastic)
![GitHub contributors](https://img.shields.io/github/contributors/demaconsulting/TestResults?style=plastic)
![GitHub](https://img.shields.io/github/license/demaconsulting/TestResults?style=plastic)
![Build](https://github.com/demaconsulting/TestResults/actions/workflows/build_on_push.yaml/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=demaconsulting_TestResults)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=demaconsulting_TestResults)
[![NuGet](https://img.shields.io/nuget/v/DemaConsulting.TestResults?style=plastic)](https://www.nuget.org/packages/DemaConsulting.TestResults)

A lightweight C# library for programmatically creating test result files in TRX and JUnit formats.

## Features

- ✨ **Simple API** - Intuitive and easy-to-use object model
- 🎯 **Type-Safe** - Strongly-typed C# classes for test results
- 🪶 **Lightweight** - Minimal external dependencies
- 🔄 **Multi-Target** - Supports .NET 8, 9, and 10
- 📦 **NuGet Ready** - Easy integration via NuGet package
- 📊 **Multiple Formats** - Supports both TRX and JUnit XML formats
- ✅ **Compatible** - Works with Visual Studio, Azure DevOps, and CI/CD systems

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package DemaConsulting.TestResults
```

Or via Package Manager Console:

```powershell
Install-Package DemaConsulting.TestResults
```

## Quick Start

### Creating Test Result Files

The following code-snippet shows how to create test result files in both TRX and JUnit XML formats:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create a TestResults instance
var results = new TestResults { Name = "SomeTests" };

// Add some results
results.Results.Add(
    new TestResult
    {
        Name = "Test1",
        ClassName = "SomeTestClass",
        CodeBase = "MyTestAssembly",
        Outcome = TestOutcome.Passed,
        Duration = TimeSpan.FromSeconds(1.5),
        StartTime = DateTime.UtcNow
    });

results.Results.Add(
    new TestResult
    {
        Name = "Test2",
        ClassName = "SomeTestClass",
        CodeBase = "MyTestAssembly",
        Outcome = TestOutcome.Failed,
        ErrorMessage = "Expected value to be 42 but was 0",
        ErrorStackTrace = "at SomeTestClass.Test2() in Test.cs:line 15"
    });

// Save the results to a TRX file (Visual Studio format)
File.WriteAllText("results.trx", TrxSerializer.Serialize(results));

// Save the results to a JUnit XML file
File.WriteAllText("results.xml", JUnitSerializer.Serialize(results));
```

### Converting Between Formats

The library supports reading and converting between TRX and JUnit formats:

```csharp
using DemaConsulting.TestResults.IO;

// Read JUnit XML file
var junitXml = File.ReadAllText("junit-results.xml");
var results = JUnitSerializer.Deserialize(junitXml);

// Convert to TRX format
var trxXml = TrxSerializer.Serialize(results);
File.WriteAllText("converted.trx", trxXml);

// Or convert TRX to JUnit
var trxXml2 = File.ReadAllText("test-results.trx");
var results2 = TrxSerializer.Deserialize(trxXml2);
var junitXml2 = JUnitSerializer.Serialize(results2);
File.WriteAllText("converted.xml", junitXml2);
```

## Advanced Usage

### Capturing Standard Output

```csharp
var result = new TestResult
{
    Name = "TestWithOutput",
    ClassName = "MyTests",
    CodeBase = "MyAssembly",
    Outcome = TestOutcome.Passed,
    SystemOutput = "Debug information\nTest completed successfully"
};
```

### Handling Test Failures

```csharp
var failedResult = new TestResult
{
    Name = "FailingTest",
    ClassName = "MyTests",
    CodeBase = "MyAssembly",
    Outcome = TestOutcome.Failed,
    ErrorMessage = "Assertion failed: Expected 100, got 50",
    ErrorStackTrace = "at MyTests.FailingTest() in Tests.cs:line 42",
    SystemError = "Additional error details"
};
```

## Test Outcomes

The library supports the following test outcomes:

- `Passed` - Test passed successfully
- `Failed` - Test failed
- `Inconclusive` - Test result was inconclusive
- `NotExecuted` - Test was not executed
- `Timeout` - Test exceeded timeout limit
- `Aborted` - Test was aborted
- `Unknown` - Test outcome is unknown

## Use Cases

This library is useful when you need to:

- Generate TRX or JUnit XML files from custom test runners
- Convert test results between formats (TRX ↔ JUnit)
- Create test reports programmatically
- Aggregate test results from multiple sources
- Build custom testing tools that integrate with Visual Studio, Azure DevOps, or CI/CD systems

## Documentation

- [Architecture][architecture] - Learn about the library's architecture and design
- [Contributing][contributing] - Guidelines for contributing to the project
- [Code of Conduct][code-of-conduct] - Our code of conduct for contributors

## Building from Source

```bash
# Clone the repository
git clone https://github.com/demaconsulting/TestResults.git
cd TestResults

# Restore tools
dotnet tool restore

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test
```

## Requirements

- .NET 8.0, 9.0, or 10.0

## Contributing

We welcome contributions! Please see our [Contributing Guide][contributing] for details.

## License

This project is licensed under the MIT License - see the [LICENSE][license] file for details.

## Support

- 📝 [Report a Bug](https://github.com/demaconsulting/TestResults/issues/new?labels=bug)
- 💡 [Request a Feature](https://github.com/demaconsulting/TestResults/issues/new?labels=enhancement)
- 💬 [Ask a Question](https://github.com/demaconsulting/TestResults/discussions)

## Acknowledgments

Developed and maintained by [DEMA Consulting](https://github.com/demaconsulting).

[architecture]: https://github.com/demaconsulting/TestResults/blob/main/ARCHITECTURE.md
[contributing]: https://github.com/demaconsulting/TestResults/blob/main/CONTRIBUTING.md
[code-of-conduct]: https://github.com/demaconsulting/TestResults/blob/main/CODE_OF_CONDUCT.md
[license]: https://github.com/demaconsulting/TestResults/blob/main/LICENSE
