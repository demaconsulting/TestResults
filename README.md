# TestResults

<!-- IMPORTANT: All links in this file must be absolute URLs.
     This file is distributed in packages and relative links will not resolve. -->

[![GitHub forks][badge-forks]][link-forks]
[![GitHub stars][badge-stars]][link-stars]
[![GitHub contributors][badge-contributors]][link-contributors]
[![License][badge-license]][link-license]
[![Build][badge-build]][link-build]
[![Quality Gate][badge-quality]][link-quality]
[![Security][badge-security]][link-security]
[![NuGet][badge-nuget]][link-nuget]

.NET library for TRX and JUnit test results

## Overview

A .NET library for reading and writing test result files in multiple formats. Provides an in-memory model for
test outcomes and results, supporting serialization to/from TRX (Visual Studio Test Results) and JUnit XML formats.

## Features

- ✨ **Simple API** - Intuitive and easy-to-use object model for test results
- 🎯 **Type-Safe** - Strongly-typed C# classes for test outcomes and results
- 🪶 **Lightweight** - Zero runtime dependencies
- 🔄 **Multi-Target** - Supports .NET Standard 2.0, .NET 8, 9, and 10
- 📦 **NuGet Ready** - Easy integration via NuGet package
- 📊 **Multiple Formats** - Supports both TRX and JUnit XML formats
- ✅ **Compatible** - Works with Visual Studio, Azure DevOps, and CI/CD systems
- 🔒 **Continuous Compliance** - Compliance evidence generated automatically on every CI run,
  following the [Continuous Compliance](https://github.com/demaconsulting/ContinuousCompliance)
  methodology

## Installation

```bash
dotnet add package DemaConsulting.TestResults
```

Or via Package Manager Console:

```powershell
Install-Package DemaConsulting.TestResults
```

## Usage

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Create a TestResults instance
var results = new TestResults { Name = "SomeTests" };

// Add test results
results.Results.Add(new TestResult
{
    Name = "Test1",
    ClassName = "SomeTestClass",
    CodeBase = "MyTestAssembly",
    Outcome = TestOutcome.Passed,
    Duration = TimeSpan.FromSeconds(1.5),
    StartTime = DateTime.UtcNow
});

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

The library can automatically detect and convert between formats:

```csharp
using DemaConsulting.TestResults.IO;

// Automatically detect and deserialize any supported format
var results = Serializer.Deserialize(File.ReadAllText("test-results.xml"));

// Convert JUnit to TRX
var junitResults = JUnitSerializer.Deserialize(File.ReadAllText("junit-results.xml"));
File.WriteAllText("converted.trx", TrxSerializer.Serialize(junitResults));
```

## Test Outcomes

The library supports the following test outcomes via the `TestOutcome` enum:

**Successful:** `Passed`, `PassedButRunAborted`, `Warning`

**Failure:** `Failed`, `Error`, `Timeout`, `Aborted`

**Skipped/Not Run:** `NotExecuted`, `NotRunnable`, `Pending`

**Other:** `Completed`, `Inconclusive`, `Disconnected`, `InProgress`

Helper extension methods: `IsPassed()`, `IsFailed()`, `IsExecuted()`

## Building

```pwsh
pwsh ./build.ps1
```

## User Guide

The TestResults User Guide is available on the
[TestResults releases page](https://github.com/demaconsulting/TestResults/releases).

## Contributing

See [CONTRIBUTING.md](https://github.com/demaconsulting/TestResults/blob/main/CONTRIBUTING.md) for guidelines.

## License

This project is licensed under the MIT License —
see [LICENSE](https://github.com/demaconsulting/TestResults/blob/main/LICENSE).

## Support

- [Report a bug or request a feature](https://github.com/demaconsulting/TestResults/issues)
- [Ask a question or start a discussion](https://github.com/demaconsulting/TestResults/discussions)

<!-- Badge References -->
[badge-forks]: https://img.shields.io/github/forks/demaconsulting/TestResults?style=plastic
[badge-stars]: https://img.shields.io/github/stars/demaconsulting/TestResults?style=plastic
[badge-contributors]: https://img.shields.io/github/contributors/demaconsulting/TestResults?style=plastic
[badge-license]: https://img.shields.io/github/license/demaconsulting/TestResults?style=plastic
[badge-build]: https://img.shields.io/github/actions/workflow/status/demaconsulting/TestResults/build_on_push.yaml?style=plastic
[badge-quality]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=alert_status
[badge-security]: https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=security_rating
[badge-nuget]: https://img.shields.io/nuget/v/DemaConsulting.TestResults?style=plastic

<!-- Link References -->
[link-forks]: https://github.com/demaconsulting/TestResults/network/members
[link-stars]: https://github.com/demaconsulting/TestResults/stargazers
[link-contributors]: https://github.com/demaconsulting/TestResults/graphs/contributors
[link-license]: https://github.com/demaconsulting/TestResults/blob/main/LICENSE
[link-build]: https://github.com/demaconsulting/TestResults/actions/workflows/build_on_push.yaml
[link-quality]: https://sonarcloud.io/dashboard?id=demaconsulting_TestResults
[link-security]: https://sonarcloud.io/dashboard?id=demaconsulting_TestResults
[link-nuget]: https://www.nuget.org/packages/DemaConsulting.TestResults
