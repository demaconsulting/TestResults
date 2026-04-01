# System Design

## Overview

The TestResults library is a .NET library for reading and writing test result
files in multiple formats. It provides a format-agnostic in-memory model and
format-specific serialization implementations.

## System Architecture

The TestResults library uses a layered architecture:

```text
┌─────────────────────────────────────────────────┐
│                 Calling Code                    │
├─────────────────────────────────────────────────┤
│              IO Subsystem (Serialization)        │
│  ┌─────────────┐  ┌──────────────┐  ┌────────┐  │
│  │  Serializer │  │TrxSerializer │  │JUnit   │  │
│  │  (facade)   │  │              │  │Ser.    │  │
│  └─────────────┘  └──────────────┘  └────────┘  │
├─────────────────────────────────────────────────┤
│              Model Layer                        │
│  ┌─────────────┐  ┌──────────────┐  ┌────────┐  │
│  │ TestResults │  │  TestResult  │  │TestOut │  │
│  │ (collection)│  │  (one test)  │  │come    │  │
│  └─────────────┘  └──────────────┘  └────────┘  │
└─────────────────────────────────────────────────┘
```

## Software Items

The TestResults system contains the following software items:

| Item | Type | Description |
| ---- | ---- | ----------- |
| TestResults Library | System | Complete test result I/O library |
| IO | Subsystem | Serialization and format detection |
| Serializer | Unit | Format-detection facade |
| TrxSerializer | Unit | TRX format read/write |
| JUnitSerializer | Unit | JUnit XML format read/write |
| TestOutcome | Unit | Test outcome enumeration |
| TestResult | Unit | Single test result data |
| TestResults | Unit | Collection of test results |

## External Interfaces

The TestResults library exposes the following public API entry points:

- `Serializer.Identify(string)`: Detects format of a test result file
- `Serializer.Deserialize(string)`: Reads a test result file into the model
- `TrxSerializer.Serialize(TestResults)`: Writes TRX format
- `TrxSerializer.Deserialize(string)`: Reads TRX format
- `JUnitSerializer.Serialize(TestResults)`: Writes JUnit XML format
- `JUnitSerializer.Deserialize(string)`: Reads JUnit XML format

## Supported Formats

| Format | Description | Standard |
| ------ | ----------- | -------- |
| TRX | Visual Studio Test Results | Microsoft proprietary |
| JUnit XML | JUnit test results | Apache JUnit |

## Related Requirements

System-level requirements are in [docs/reqstream/system.yaml](../reqstream/system.yaml).
Platform requirements are in
[docs/reqstream/platform-requirements.yaml](../reqstream/platform-requirements.yaml).
