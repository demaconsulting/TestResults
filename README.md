# SPDX Model

![GitHub forks](https://img.shields.io/github/forks/demaconsulting/TestResults?style=plastic)
![GitHub Repo stars](https://img.shields.io/github/stars/demaconsulting/TestResults?style=plastic)
![GitHub contributors](https://img.shields.io/github/contributors/demaconsulting/TestResults?style=plastic)
![GitHub](https://img.shields.io/github/license/demaconsulting/TestResults?style=plastic)
![Build](https://github.com/demaconsulting/SpdxModel/actions/workflows/build_on_push.yaml/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=demaconsulting_TestResults)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=demaconsulting_TestResults&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=demaconsulting_TestResults)

The TestResults library supports saving test results to TRX files.


## Usage

The following code-snippet shows how to create a TRX test-results file.

```csharp
// Create a TestResults instance
var results = new TestResults{Name = "SomeTests"};

// Add some results
results.Results.Add(
    new TestResult{
        Name = "Test1",
        ClassName = "SomeTestClass",
        CodeBase = "MyTestAssembly",
        Outcome = TestOutcome.Passed
    });

// Save the results to file
File.WriteAllText(
    "results.trx",
    TrxSerializer.Serialize(results));
```
