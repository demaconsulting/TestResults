# AI Instructions for TestResults

This file provides specific context and instructions for AI coding agents to
interact effectively with this C# project.

## Project Overview

TestResults is a C# library for creating test result files. The library is designed to be:

- **Lightweight**: Minimal external dependencies
- **Simple**: Easy-to-use API for creating test result files
- **Type-Safe**: Strongly-typed C# objects
- **Cross-Platform**: Supports .NET 8, 9, and 10

The library supports both TRX and JUnit XML formats, with potential for additional formats in the future.

## Technologies and Dependencies

### Runtime

- **Language**: C# 12
- **.NET Frameworks**: .NET 8, 9, and 10
- **Primary Dependencies**: None currently

### Development Tools

- **Analyzers**: Microsoft.CodeAnalysis.NetAnalyzers and SonarAnalyzer.CSharp for static code analysis
- **Source Control**: SourceLink for debugging support
- **Testing**: MSTest framework with code coverage
- **CI/CD**: GitHub Actions for build and release automation
- **Code Quality**: SonarCloud for quality metrics
- **Dependency Management**: Dependabot for automated dependency updates

## Project Structure

The repository is organized as follows:

```text
/
├── .config/                              # .NET tool configurations
├── .github/workflows/                    # CI/CD pipeline configurations
├── src/DemaConsulting.TestResults/       # Main library source code
├── test/DemaConsulting.TestResults.Tests/ # Unit tests
├── .editorconfig                         # Code style enforcement
├── .cspell.json                          # Spell checking configuration
├── .markdownlint.json                    # Markdown linting rules
├── AGENTS.md                             # This file
├── ARCHITECTURE.md                       # Architecture documentation
├── CODE_OF_CONDUCT.md                    # Code of conduct
├── CONTRIBUTING.md                       # Contribution guidelines
├── DemaConsulting.TestResults.sln        # Visual Studio solution
└── README.md                             # Main documentation
```

Key directories:

- **src/** - Contains the main library implementation
- **test/** - Contains unit tests and test resources
- **.github/** - Contains CI/CD workflows and automation

## Development Commands

Use these commands to perform common development tasks:

### Initial Setup

```bash
# Restore .NET tools
dotnet tool restore

# Restore dependencies
dotnet restore
```

### Building and Testing

```bash
# Build the project
dotnet build

# Build in release mode
dotnet build --configuration Release

# Run all tests
dotnet test

# Run tests with code coverage
dotnet test --collect "XPlat Code Coverage"

# Build NuGet package
dotnet pack
```

### Quality Checks

```bash
# Run spell checker (requires npm install -g cspell)
cspell "**/*.{md,cs}"

# Run markdown linter (requires npm install -g markdownlint-cli)
markdownlint "**/*.md"

# Format code (if format tools are installed)
dotnet format
```

## Testing Guidelines

### Test Organization

- Tests are located under the `/test/DemaConsulting.TestResults.Tests/` folder
- Use the MSTest framework (`[TestClass]`, `[TestMethod]`, etc.)
- Test files should end with `.cs` and follow the naming convention `[Component]Tests.cs`

### Test Structure

- Use the AAA (Arrange, Act, Assert) pattern
- Keep tests focused on a single behavior
- Use descriptive test method names that explain what is being tested
- Example: `TestMethod_Scenario_ExpectedBehavior`

### Test Coverage

- All new features must have comprehensive unit tests
- Aim for high code coverage (>80%)
- Test both success and failure scenarios
- Include edge cases and boundary conditions

### Test Requirements

- All tests must pass before merging
- No warnings are allowed (warnings are treated as errors)
- Tests should be isolated and not depend on execution order
- Use appropriate assertions from MSTest (`Assert.AreEqual`, `Assert.ThrowsException`, etc.)

## Code Style and Conventions

### Naming Conventions

- **PascalCase**: Classes, methods, properties, public fields, namespaces
- **camelCase**: Local variables, private fields, parameters
- **Interface names**: Start with 'I' (e.g., `ITestResult`)
- **Test methods**: Use descriptive names (e.g., `Serialize_EmptyResults_ReturnsValidXml`)

### Code Organization

- One class per file (except for nested/related classes)
- Organize using statements (System.* first, then others alphabetically)
- Use file-scoped namespaces when appropriate
- Group class members by type (fields, properties, constructors, methods)

### Best Practices

- **Nullable Reference Types**: Enabled at project level; use appropriately
- **Warnings as Errors**: All warnings must be resolved
- **XML Documentation**: Required for all public APIs
- **Immutability**: Prefer readonly fields and get-only properties where appropriate
- **LINQ**: Use LINQ for collection operations when it improves readability
- **String Interpolation**: Prefer over string concatenation
- **Object Initializers**: Use for cleaner object construction

### Code Quality

- Follow .editorconfig settings for consistent formatting
- Use meaningful variable and method names
- Keep methods focused and concise (Single Responsibility Principle)
- Avoid code duplication (DRY principle)
- Comment only when necessary to explain "why", not "what"
- Use static analysis recommendations from analyzers

## Quality Standards

### Static Analysis

- Microsoft.CodeAnalysis.NetAnalyzers is enabled in all projects for .NET-specific analysis
- SonarAnalyzer.CSharp is enabled in all projects for additional code quality checks
- All analyzer warnings must be addressed or explicitly suppressed with justification
- Code style rules are enforced via .editorconfig

### Documentation

- All public APIs must have XML documentation comments
- Include `<summary>`, `<param>`, `<returns>`, and `<exception>` tags as appropriate
- Keep documentation clear, concise, and accurate
- Update README.md and ARCHITECTURE.md for significant changes

### Spelling and Markdown

- Use cspell for spell checking code and documentation
- Follow markdownlint rules for markdown files
- Add technical terms to .cspell.json dictionary as needed

## CI/CD Pipeline

### Build Process

The build pipeline includes:

1. **Checkout**: Clone the repository
2. **Setup**: Install .NET SDKs (8, 9, 10)
3. **Restore**: Restore tools and dependencies
4. **SonarCloud Start**: Begin SonarCloud analysis
5. **Build**: Compile in Release mode
6. **Test**: Run all tests with code coverage
7. **SonarCloud End**: Complete SonarCloud analysis
8. **SBOM**: Generate Software Bill of Materials
9. **Package**: Create NuGet packages

### Quality Gates

All builds must pass:

- Compilation with zero warnings
- All unit tests passing
- SonarCloud quality gate
- Code coverage thresholds

## Common Tasks for AI Agents

### Adding a New Feature

1. Read ARCHITECTURE.md to understand the design
2. Create or update the domain model classes
3. Update serialization logic if needed
4. Add comprehensive unit tests
5. Update XML documentation
6. Update README.md with usage examples
7. Run all tests and ensure they pass

### Fixing a Bug

1. Reproduce the bug with a failing test
2. Fix the bug with minimal changes
3. Ensure the test now passes
4. Verify no other tests are broken
5. Update documentation if the bug fix changes behavior

### Improving Code Quality

1. Run static analysis and address warnings
2. Review code coverage reports
3. Refactor duplicated code
4. Improve naming and clarity
5. Add missing documentation
6. Verify all tests still pass

### Updating Dependencies

1. Check dependency versions in .csproj files
2. Update to latest stable versions when appropriate
3. Test thoroughly after updates
4. Update documentation if APIs changed

## Boundaries and Guardrails

### What AI Agents Should NEVER Do

- Modify files within `/obj/` or `/bin/` directories
- Commit secrets, API keys, or sensitive configuration data
- Make significant architectural changes without discussion
- Remove or disable existing tests
- Ignore or suppress analyzer warnings without justification
- Add external runtime dependencies (the library must remain zero-dependency)

### What AI Agents Should ALWAYS Do

- Run tests before committing changes
- Follow existing code style and patterns
- Update documentation for user-facing changes
- Add tests for new functionality
- Resolve all warnings and analyzer suggestions
- Keep changes minimal and focused

### What AI Agents Should ASK About

- Adding new public APIs or changing existing ones
- Significant refactoring or architectural changes
- Adding development dependencies
- Changing build or release processes
- Questions about requirements or expected behavior

## Performance Considerations

- The library is lightweight and should remain so
- Avoid unnecessary allocations during serialization
- Use efficient string handling (StringBuilder for concatenation)
- Profile performance for large test result sets

## Security Considerations

- No file I/O operations within the library (caller controls file operations)
- No network operations
- Validate all inputs to public APIs
- Be careful with XML serialization to avoid XXE attacks
- Keep dependencies minimal to reduce security surface area

## Tips for Effective Changes

1. **Understand First**: Read existing code and documentation before making changes
2. **Test Driven**: Write tests first or alongside code changes
3. **Small Steps**: Make small, incremental changes that are easy to review
4. **Run Often**: Build and test frequently during development
5. **Document**: Update documentation as you change code
6. **Review**: Self-review your changes before submitting

## Getting Help

- Review [ARCHITECTURE.md](ARCHITECTURE.md) for design details
- Check [CONTRIBUTING.md](CONTRIBUTING.md) for contribution guidelines
- Look at existing code for patterns and examples
- Run `dotnet build` and `dotnet test` to verify changes
- Use `dotnet --info` to check your environment

## Version Support

- The library targets .NET 8, 9, and 10
- Use APIs available in all target frameworks
- Test on multiple framework versions when possible
- Check for framework-specific issues in CI/CD logs
