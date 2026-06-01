# Introduction

This document defines the software design for the TestResults repository. It describes the
local software items that make up the TestResultsLibrary system, explains how the IO
subsystem translates between the in-memory model and XML result formats, and records the
companion artifact paths used for requirements, design, source, tests, and verification.

## Purpose

The purpose of this document is to provide reviewable design information for each local
software item in TestResults. A reviewer should be able to understand how
TestResultsLibrary, the IO subsystem, and each unit satisfy their responsibilities and
requirements without reverse-engineering the code from source files alone.

## Scope

Local items covered by this document:

- **TestResultsLibrary**: full system design for the .NET library.
- **IO**: subsystem design for format identification and XML serialization.
- **Serializer**, **SerializerHelpers**, **TrxSerializer**, **JUnitSerializer**,
  **TestOutcome**, **TestResult**, and **TestResults**: unit-level design.

Out of scope are the test projects themselves, build and CI pipeline behavior, and the
internal design of external framework libraries.

OTS software items used in this repository:

- **BuildMark**, **FileAssert**, **Pandoc**, **ReqStream**, **ReviewMark**, **SarifMark**,
  **SonarMark**, **VersionMark**, **WeasyPrint**, **xUnit**, and **ApiMark**:
  integration and usage design for each is in `docs/design/ots.md` and `docs/design/ots/`.

## Software Structure

```text
TestResults Library (System)
├── IO (Subsystem)
│   ├── Serializer (Unit)
│   ├── SerializerHelpers (Unit)
│   ├── TrxSerializer (Unit)
│   └── JUnitSerializer (Unit)
├── TestOutcome (Unit)
├── TestResult (Unit)
└── TestResults (Unit)

OTS Software Items
├── BuildMark
├── FileAssert
├── Pandoc
├── ReqStream
├── ReviewMark
├── SarifMark
├── SonarMark
├── VersionMark
├── WeasyPrint
├── xUnit
└── ApiMark
```

## Folder Layout

```text
src/DemaConsulting.TestResults/
├── IO/
│   ├── Serializer.cs              - Identifies XML result formats and delegates deserialization.
│   ├── SerializerHelpers.cs       - Provides Utf8StringWriter for XML declarations.
│   ├── TrxSerializer.cs           - Serializes and deserializes TRX test result documents.
│   └── JUnitSerializer.cs         - Serializes and deserializes JUnit XML test result documents.
├── TestOutcome.cs                 - Defines supported test outcomes and classification helpers.
├── TestResult.cs                  - Represents one test case result with metadata and output.
└── TestResults.cs                 - Represents a complete test run and its ordered results.

test/DemaConsulting.TestResults.Tests/
├── TestResultsLibraryTests.cs     - End-to-end tests for the public library behavior.
├── TestOutcomeTests.cs            - Unit tests for outcome classification helpers.
├── TestResultTests.cs             - Unit tests for TestResult defaults and property retention.
├── TestResultsTests.cs            - Unit tests for TestResults defaults and collection behavior.
└── IO/
    ├── IOTests.cs                 - Subsystem tests for format identification and model translation.
    ├── SerializerTests.cs         - Unit tests for format detection and delegated deserialization.
    ├── SerializerHelpersTests.cs  - Unit tests for UTF-8 writer behavior.
    ├── TrxSerializerTests.cs      - Unit tests for TRX serialization and deserialization.
    ├── JUnitSerializerTests.cs    - Unit tests for JUnit XML serialization and deserialization.
    └── TrxExampleTests.cs         - Regression tests using embedded TRX example files.
```

## Companion Artifact Structure

```text
requirements.yaml
└── includes docs/reqstream/test-results-library.yaml

docs/reqstream/
├── test-results-library.yaml      - System requirements for TestResultsLibrary.
└── test-results-library/
    ├── test-outcome.yaml          - Unit requirements for TestOutcome.
    ├── test-result.yaml           - Unit requirements for TestResult.
    ├── test-results.yaml          - Unit requirements for TestResults.
    ├── io.yaml                    - Subsystem requirements for IO.
    └── io/
        ├── serializer.yaml        - Unit requirements for Serializer.
        ├── serializer-helpers.yaml - Unit requirements for SerializerHelpers.
        ├── trx-serializer.yaml    - Unit requirements for TrxSerializer.
        └── junit-serializer.yaml  - Unit requirements for JUnitSerializer.

docs/design/
├── test-results-library.md        - System design for TestResultsLibrary.
├── ots.md                         - OTS integration design overview.
├── ots/                           - Per-OTS-item integration design files.
└── test-results-library/
    ├── io.md                      - Subsystem design for IO.
    ├── test-outcome.md            - Unit design for TestOutcome.
    ├── test-result.md             - Unit design for TestResult.
    ├── test-results.md            - Unit design for TestResults.
    └── io/
        ├── serializer.md          - Unit design for Serializer.
        ├── serializer-helpers.md  - Unit design for SerializerHelpers.
        ├── trx-serializer.md      - Unit design for TrxSerializer.
        └── junit-serializer.md    - Unit design for JUnitSerializer.

docs/verification/
├── ots.md                         - OTS verification overview.
├── ots/                           - Per-OTS-item verification files.
└── test-results-library[...]      - Verification documents parallel to the design tree.

src/DemaConsulting.TestResults/
└── implementation for the local units and subsystem.

test/DemaConsulting.TestResults.Tests/
└── verification tests for the system, subsystem, and units.
```

## References

- [TestResults releases](https://github.com/demaconsulting/TestResults/releases)
- [MSTest and TRX overview](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest)
- [JUnit 5](https://junit.org/junit5/)
