# Introduction

This document describes the internal design of the TestResults Library. It provides a
structured account of the key components, their responsibilities, and how they interact to
deliver the library's capabilities.

## Purpose

The purpose of this document is to:

- Describe the design decisions and structure of the TestResults Library
- Provide a reference for developers contributing to or reviewing the library
- Establish traceability between requirements and the components that fulfil them
- Document the in-memory model units and IO subsystem in sufficient detail to support code review

## Scope

This document covers the design of the TestResults Library, organized by the following
system/subsystem/unit hierarchy:

- The **TestResults Library** (System): a .NET library for reading and writing test result
  files in multiple formats
- The **IO Subsystem**: the components responsible for reading and writing test result
  files in TRX and JUnit XML formats, comprising four units:
  - `Serializer` — format-detection facade
  - `SerializerHelpers` — internal UTF-8 writer helper used by both serializers
  - `TrxSerializer` — TRX (Visual Studio Test Results) format read/write
  - `JUnitSerializer` — JUnit XML format read/write
- The **three top-level units** forming the in-memory model (outside any subsystem):
  - `TestOutcome` — enumeration of all possible test outcomes
  - `TestResult` — single test execution result
  - `TestResults` — named collection of test results for a complete test run

This document does not cover installation, end-user usage patterns, or the CI/CD pipeline
configuration. Those topics are addressed in the [User Guide][user-guide] and the
[Requirements document][requirements-doc].

## Audience

This document is intended for:

- Software developers implementing features or fixing defects in the library
- Reviewers conducting formal design and code reviews
- Quality assurance engineers tracing requirements to implementation

Readers are assumed to be familiar with C# and .NET development, XML processing, and
the general concepts of unit-test result reporting.

## Relationship to Requirements and Code

Each component described here corresponds to one or more requirements defined in
`requirements.yaml`. Requirements identifiers are referenced inline where relevant to
make traceability explicit.

The source code in `src/DemaConsulting.TestResults/` is the authoritative implementation.
This document describes the intent and structure of that code; any discrepancy between
this document and the code should be resolved by updating this document to reflect the
actual implementation, or by raising a defect against the code.

## Software Structure

The TestResults Library is organized into one subsystem and three top-level units:

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
```

## Folder Layout

The source code is organized to mirror the design documentation structure:

```text
src/DemaConsulting.TestResults/
├── IO/                          — IO Subsystem
│   ├── Serializer.cs            — Format-detection facade (Serializer unit)
│   ├── SerializerHelpers.cs     — Internal UTF-8 writer helper (SerializerHelpers unit)
│   ├── TrxSerializer.cs         — TRX format read/write (TrxSerializer unit)
│   └── JUnitSerializer.cs       — JUnit XML format read/write (JUnitSerializer unit)
├── TestOutcome.cs               — Test outcome enumeration (TestOutcome unit)
├── TestResult.cs                — Single test result data (TestResult unit)
└── TestResults.cs               — Collection of test results (TestResults unit)
```

## Companion Artifact Structure

Each design unit has corresponding artifacts across requirements, source, tests, and
review-set entries, forming end-to-end traceability:

```text
requirements.yaml                              — System-level requirements (TestResults-*)
docs/reqstream/test-results-library/           — Unit-level requirements
├── io/
│   ├── io.yaml                                — IO Subsystem requirements
│   ├── serializer.yaml                        — Serializer unit requirements
│   ├── serializer-helpers.yaml                — SerializerHelpers unit requirements
│   ├── trx-serializer.yaml                    — TrxSerializer unit requirements
│   └── junit-serializer.yaml                  — JUnitSerializer unit requirements
├── test-outcome.yaml                          — TestOutcome unit requirements
├── test-result.yaml                           — TestResult unit requirements
└── test-results.yaml                          — TestResults unit requirements
docs/design/                                   — Design documentation (this document and sub-documents)
├── test-results-library/
│   ├── io/                                    — IO Subsystem design
│   ├── test-outcome.md                        — TestOutcome unit design
│   ├── test-result.md                         — TestResult unit design
│   └── test-results.md                        — TestResults unit design
src/DemaConsulting.TestResults/                — Source implementation
└── IO/
    ├── Serializer.cs
    ├── SerializerHelpers.cs
    ├── TrxSerializer.cs
    └── JUnitSerializer.cs
test/DemaConsulting.TestResults.Tests/         — Unit tests
└── IO/
    ├── IOTests.cs
    ├── SerializerTests.cs
    ├── SerializerHelpersTests.cs
    ├── TrxSerializerTests.cs
    └── JUnitSerializerTests.cs
```

[user-guide]: https://github.com/demaconsulting/TestResults/blob/main/docs/user_guide/introduction.md
[requirements-doc]: https://github.com/demaconsulting/TestResults/blob/main/requirements.yaml
