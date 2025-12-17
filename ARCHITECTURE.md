# TestResults Library Architecture

## Overview

The TestResults library is a lightweight C# library designed to programmatically create test result files.
The library supports both TRX (Test Results) and JUnit XML formats.
TRX files are XML-based test result files commonly used by Visual Studio, Azure DevOps, and other Microsoft testing
tools, while JUnit XML is widely used across various CI/CD systems and testing frameworks.

## Design Philosophy

The library follows these core principles:

- **Simplicity**: Provide a straightforward API for creating test result files without unnecessary complexity
- **Type Safety**: Use strongly-typed C# objects to represent test results
- **Lightweight**: Minimize external dependencies to keep the library easy to integrate
- **Multi-Target Support**: Support multiple .NET versions (.NET 8, 9, and 10) for broad compatibility

## Architecture Components

### Core Domain Model

The library uses a simple object model to represent test results:

#### `TestResults`

The root container for all test results. Key properties:

- `Name`: The name of the test run
- `Results`: A collection of individual test results

#### `TestResult`

Represents a single test result. Key properties:

- `Name`: The test name
- `ClassName`: The fully qualified name of the test class
- `CodeBase`: The test assembly name
- `Outcome`: The test outcome (see `TestOutcome` enum)
- `Duration`: Test execution duration
- `StartTime`: When the test started
- `EndTime`: When the test finished
- `ErrorMessage`: Error message if the test failed
- `ErrorStackTrace`: Stack trace if the test failed
- `StdOut`: Standard output captured during test execution
- `StdErr`: Standard error output captured during test execution

#### `TestOutcome`

An enumeration representing possible test outcomes:

- `Passed`: Test passed successfully
- `Failed`: Test failed
- `Inconclusive`: Test result was inconclusive
- `NotExecuted`: Test was not executed
- `Timeout`: Test exceeded timeout limit
- `Aborted`: Test was aborted
- `Unknown`: Test outcome is unknown

### Serialization Layer

#### `TrxSerializer`

The `TrxSerializer` class is responsible for converting between the domain model and TRX XML format:

- Uses .NET's built-in XML serialization capabilities
- Produces TRX files compatible with Visual Studio and Azure DevOps
- Can deserialize TRX XML files back into the domain model
- Handles proper formatting and schema compliance
- Implements helper methods for complex XML structure creation to maintain code clarity and reduce complexity

#### `JUnitSerializer`

The `JUnitSerializer` class is responsible for converting between the domain model and JUnit XML format:

- Uses .NET's built-in XML serialization capabilities
- Produces JUnit XML files compatible with various CI/CD systems and testing tools
- Can deserialize JUnit XML files back into the domain model
- Groups test results by class name into test suites
- Maps test outcomes to JUnit semantics (failure, error, skipped)
- Implements helper methods for complex XML structure creation to maintain code clarity and reduce complexity

## Data Flow

```text
1. User creates TestResults instance
2. User adds TestResult objects to the Results collection
3. User calls TrxSerializer.Serialize() or JUnitSerializer.Serialize() to convert to desired format
4. User saves the XML string to a .trx or .xml file
```

## Design Patterns

- **Data Transfer Object (DTO)**: The `TestResults` and `TestResult` classes serve as DTOs for test data
- **Serializer Pattern**: The `TrxSerializer` and `JUnitSerializer` classes encapsulate all serialization/deserialization logic
- **Builder Pattern**: The API allows for fluent construction of test results
- **Helper Method Extraction**: Complex serialization/deserialization logic is broken down into focused helper methods, each handling a specific portion of the XML structure
- **Constant Extraction**: Repeated string literals are extracted as private constants to improve maintainability and reduce duplication

## File Organization

The library source code is organized in the `/src/DemaConsulting.TestResults/` directory:

- **Domain Model**: Core classes representing test results (`TestResults`, `TestResult`, `TestOutcome`)
- **Serialization**: I/O operations for converting to/from file formats (in the `IO/` subdirectory)
- **Additional Components**: May be added as the library evolves

## Extension Points

The library is designed to be extended in several ways:

1. **Custom Test Outcomes**: While the standard outcomes cover most scenarios, custom outcomes could be added
2. **Additional Metadata**: The model could be extended to support additional metadata fields
3. **Alternative Serializers**: Additional serializers could be added for other test result formats (NUnit, xUnit, etc.)

## Quality Attributes

### Reliability

- Warnings are treated as errors to ensure code quality
- Nullable reference types are enabled to prevent null reference exceptions
- Comprehensive unit tests validate core functionality

### Performance

- Minimal memory allocation during serialization
- No external dependencies to reduce overhead
- Lightweight object model

### Maintainability

- Clear separation of concerns between domain model and serialization
- Comprehensive XML documentation for all public APIs
- Consistent code style enforced through .editorconfig

### Security

- No file I/O operations within the library (caller controls where files are written)
- No network operations
- No external dependencies that could introduce vulnerabilities

## Future Considerations

Potential enhancements that could be considered:

1. **Additional Formats**: Support for other test result formats (NUnit XML, xUnit XML, etc.)
2. **Streaming**: Support for streaming large test result sets to avoid memory issues
3. **Validation**: Add schema validation to ensure generated files are well-formed
4. **Format Detection**: Automatic detection of input format when deserializing

## Dependencies

### Runtime Dependencies

- None currently

### Development Dependencies

- Microsoft.SourceLink.GitHub: For source linking in NuGet packages
- Microsoft.CodeAnalysis.NetAnalyzers: For .NET-specific static code analysis
- SonarAnalyzer.CSharp: For additional code quality and security analysis

### Test Dependencies

- MSTest: Testing framework
- Microsoft.NET.Test.Sdk: Test SDK
- coverlet.collector: Code coverage collection
