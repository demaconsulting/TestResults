# FAQ

## What test result formats are supported?

The TestResults library supports two formats:

- **TRX (Test Results)**: XML format used by Visual Studio, MSTest, Azure DevOps, and other Microsoft testing tools
- **JUnit XML**: Widely supported format compatible with Jenkins, GitHub Actions, GitLab CI, CircleCI, and most
  other CI/CD systems

Both formats can be created from scratch, deserialized from existing files, or converted from one to the other.

## Can I convert between TRX and JUnit formats?

Yes. The library supports bidirectional conversion:

```csharp
// Automatic format detection and conversion
var results = Serializer.Deserialize(File.ReadAllText("test-results.xml"));
File.WriteAllText("converted.trx", TrxSerializer.Serialize(results));

// Format-specific deserializers
var trxResults   = TrxSerializer.Deserialize(File.ReadAllText("test.trx"));
var junitFromTrx = JUnitSerializer.Serialize(trxResults);

var junitResults = JUnitSerializer.Deserialize(File.ReadAllText("test.xml"));
var trxFromJunit = TrxSerializer.Serialize(junitResults);
```

## How do I automatically detect the format of a test result file?

Use `Serializer.Identify()` to determine the format without fully deserializing, or `Serializer.Deserialize()` to
detect and parse in a single step:

```csharp
var format = Serializer.Identify(File.ReadAllText("test-results.xml"));
if (format == TestResultFormat.Trx)
    Console.WriteLine("TRX format");
else if (format == TestResultFormat.JUnit)
    Console.WriteLine("JUnit format");

// Or deserialize directly
var results = Serializer.Deserialize(File.ReadAllText("test-results.xml"));
```

`Serializer.Identify()` returns `TestResultFormat.Unknown` for invalid or unrecognized content.

## What are the known format limitations?

### JUnit Round-Trip Fidelity

JUnit XML does not have distinct elements for every `TestOutcome` value. Two known limitations apply when
round-tripping through JUnit format:

- **`Timeout` and `Aborted` outcomes are not preserved.** Both are serialized as an `<error>` element (JUnit has no
  distinct timeout or aborted element) and deserialize back as `TestOutcome.Error`. Use the TRX format if precise
  outcome preservation is required.

- **`ClassName = "DefaultSuite"` round-trips with an empty `ClassName`.** Tests without a class name are grouped
  under the sentinel value `"DefaultSuite"` during serialization. On deserialization that sentinel is mapped back to
  an empty string, so a test whose `ClassName` is literally `"DefaultSuite"` will lose its class name after a JUnit
  round-trip.

- **`Inconclusive` outcomes are not preserved.** `Inconclusive` tests serialize as plain `<testcase>`
  elements with no outcome child element, which are indistinguishable from `Passed` tests on
  deserialization. `Inconclusive` therefore deserializes back as `TestOutcome.Passed` after a JUnit
  round-trip.

### TRX Round-Trip Fidelity

Round-trip fidelity is fully preserved for the TRX format. Serializing a `TestResults` object to TRX and
deserializing it back produces an identical object. Choose TRX when full fidelity is required.

## How do I handle test failures with error details?

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

## Is the library thread-safe?

The serializer classes (`TrxSerializer`, `JUnitSerializer`, and `Serializer`) are stateless and safe to call from
multiple threads simultaneously. The `TestResults` and `TestResult` DTOs are not thread-safe — use a lock when
adding results from multiple threads concurrently.

## Where are test result files saved?

The library does not perform any file I/O. You are responsible for choosing file paths, creating directories, and
writing the serialized XML string to disk. This gives you full control over file locations and error handling.

## Can I use this library with MSTest, NUnit, or xUnit?

The TestResults library is not a test framework — it is a serialization library. It is designed for scenarios where
you need to programmatically generate or convert test result files, such as custom test runners, result aggregation
tools, format conversion utilities, and CI/CD integration scripts. If you are using MSTest, NUnit, or xUnit, those
frameworks already generate their own result files; you typically need this library only if you are building tooling
that processes or converts those results.

## What .NET versions are supported?

The library targets .NET Standard 2.0, .NET 8.0, .NET 9.0, and .NET 10.0. It has zero runtime dependencies.

## How do I report bugs or request features?

- **Report a Bug**: <https://github.com/demaconsulting/TestResults/issues/new?labels=bug>
- **Request a Feature**: <https://github.com/demaconsulting/TestResults/issues/new?labels=enhancement>
- **Ask Questions**: <https://github.com/demaconsulting/TestResults/discussions>

## Next Steps

After integrating the TestResults library:

1. **Explore the examples** — try the code samples in the Usage and Advanced Usage sections with your own data
2. **Review the design** — the TestResults releases page includes compiled design documentation with architecture
   details
3. **Contribute** — see the contributing guide at
   <https://github.com/demaconsulting/TestResults/blob/main/CONTRIBUTING.md>
4. **Get support** — ask questions at <https://github.com/demaconsulting/TestResults/discussions>
