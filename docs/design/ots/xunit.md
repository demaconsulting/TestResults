## xUnit

### Purpose

xUnit v3 is the unit-testing framework used to discover and run all test methods in the
TestResults test project and to produce TRX result files consumed by ReqStream for requirements
traceability. It is chosen for its xUnit v3 runner architecture, parallel test execution, and
first-class .NET SDK integration.

### Features Used

- **Test discovery and execution**: the `xunit.v3` package discovers test methods decorated
  with `[Fact]` and `[Theory]` attributes and executes them in the configured parallelism mode.
- **TRX result file production**: the `xunit.runner.visualstudio` runner adapter produces TRX
  result files when `dotnet test` is invoked with `--logger trx`, which ReqStream then reads
  for requirements coverage.
- **.NET test SDK integration**: `Microsoft.NET.Test.Sdk` wires xUnit into the standard
  `dotnet test` toolchain so that tests can be run from the command line, Visual Studio, and CI
  pipelines without additional configuration.

### Integration Pattern

xUnit is referenced as NuGet package dependencies (`xunit.v3`, `xunit.runner.visualstudio`, and
`Microsoft.NET.Test.Sdk`) in the test project `test/DemaConsulting.TestResults.Tests/`. There is
no separate installation step: package restore during `dotnet test` is sufficient. Tests are
executed by running `dotnet test --configuration Release --logger trx`, which produces TRX files
in `TestResults/` output directories. Those TRX files are then passed to ReqStream as test
evidence. No initialization or disposal beyond standard `dotnet test` invocation is required.
