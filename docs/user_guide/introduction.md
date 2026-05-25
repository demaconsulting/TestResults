# Introduction

This guide describes how to install, configure, and use the TestResults library in .NET projects.

## Purpose

The TestResults library is a lightweight C# library for programmatically creating, reading, and writing test result
files in TRX and JUnit XML formats. It provides a simple, type-safe in-memory model for test outcomes and results,
and supports bidirectional serialization to and from TRX (Visual Studio Test Results) and JUnit XML formats.

This guide exists to help developers integrate the TestResults library into their .NET applications, including custom
test runners, format conversion utilities, test report aggregation tools, and CI/CD integration scripts.

## Scope

This guide covers the following topics:

- Installing the TestResults NuGet package
- Creating and populating test result objects
- Serializing results to TRX and JUnit XML formats
- Reading and deserializing existing test result files
- Converting between TRX and JUnit formats
- Advanced usage patterns including result aggregation and filtering
- Frequently asked questions and known limitations

This guide assumes familiarity with C# and .NET development. It does not cover the internal design of the library or
the specifications of the TRX and JUnit XML formats. Refer to the TestResults Software Design document for design
details.

## References

- [TestResults releases](https://github.com/demaconsulting/TestResults/releases)
