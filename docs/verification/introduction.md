# Introduction

This document describes how the TestResults repository verifies the TestResults Library
software system and its constituent subsystem and units.

## Purpose

This document defines the verification design for the TestResults Library so reviewers can
trace each local software item from its requirements and design artifacts to the automated
tests that provide objective evidence. It identifies the verification approach, test
environment, acceptance criteria, and scenario-level evidence for the system, the IO
subsystem, and each software unit implemented in this repository.

## Scope

This document covers the following local software items:

- **TestResults Library**: system-level verification of the public library behavior.
- **IO**: subsystem-level verification of format identification, serialization, and
  deserialization behavior.
- **TestOutcome**: unit verification of outcome classification behavior.
- **TestResult**: unit verification of single-test result data behavior.
- **TestResults**: unit verification of test-run collection behavior.
- **Serializer**: unit verification of format identification and dispatch behavior.
- **SerializerHelpers**: unit verification of the shared UTF-8 writer used by XML serializers.
- **TrxSerializer**: unit verification of TRX serialization and deserialization behavior.
- **JUnitSerializer**: unit verification of JUnit XML serialization and deserialization behavior.

This document excludes the internal structure of the test project itself, build-pipeline
implementation details, and generated compliance artifacts under any `generated/` folder.

- **BuildMark**, **FileAssert**, **Pandoc**, **ReqStream**, **ReviewMark**, **SarifMark**,
  **SonarMark**, **VersionMark**, **WeasyPrint**, **xUnit**, and **ApiMark**: OTS
  verification evidence for each is in `docs/verification/ots.md` and
  `docs/verification/ots/`.

## Companion Artifact Structure

Local items have parallel artifacts in:

- Requirements: `docs/reqstream/test-results-library.yaml`,
  `docs/reqstream/test-results-library/test-outcome.yaml`,
  `docs/reqstream/test-results-library/test-result.yaml`,
  `docs/reqstream/test-results-library/test-results.yaml`,
  `docs/reqstream/test-results-library/io.yaml`, and
  `docs/reqstream/test-results-library/io/*.yaml`
- Design: `docs/design/test-results-library.md`,
  `docs/design/test-results-library/test-outcome.md`,
  `docs/design/test-results-library/test-result.md`,
  `docs/design/test-results-library/test-results.md`,
  `docs/design/test-results-library/io.md`, and
  `docs/design/test-results-library/io/*.md`
- Verification: `docs/verification/test-results-library.md`,
  `docs/verification/test-results-library/test-outcome.md`,
  `docs/verification/test-results-library/test-result.md`,
  `docs/verification/test-results-library/test-results.md`,
  `docs/verification/test-results-library/io.md`, and
  `docs/verification/test-results-library/io/*.md`
- OTS verification: `docs/verification/ots.md` and `docs/verification/ots/*.md`
- Source: `src/DemaConsulting.TestResults/TestOutcome.cs`,
  `src/DemaConsulting.TestResults/TestResult.cs`,
  `src/DemaConsulting.TestResults/TestResults.cs`, and
  `src/DemaConsulting.TestResults/IO/*.cs`
- Tests: `test/DemaConsulting.TestResults.Tests/TestOutcomeTests.cs`,
  `test/DemaConsulting.TestResults.Tests/TestResultTests.cs`,
  `test/DemaConsulting.TestResults.Tests/TestResultsTests.cs`,
  `test/DemaConsulting.TestResults.Tests/TestResultsLibraryTests.cs`, and
  `test/DemaConsulting.TestResults.Tests/IO/*.cs`

Review-sets are defined in `.reviewmark.yaml`.

## References

- [TestResults releases](https://github.com/demaconsulting/TestResults/releases)
- [xUnit.net v3 documentation](https://xunit.net/docs/getting-started/v3/getting-started)
