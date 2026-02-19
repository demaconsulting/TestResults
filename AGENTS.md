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
- **Requirements Management**: DemaConsulting.ReqStream for requirements tracking and traceability
- **CI/CD**: GitHub Actions for build and release automation
- **Code Quality**: SonarCloud for quality metrics
- **Dependency Management**: Dependabot for automated dependency updates

## Project Structure

The repository is organized as follows:

```text
/
├── .config/                              # .NET tool configurations
├── .github/workflows/                    # CI/CD pipeline configurations
├── docs/                                 # Documentation source files
│   ├── guide/                           # Developer guide
│   ├── requirements/                     # Requirements specification
│   └── tracematrix/                      # Requirements trace matrix
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
├── README.md                             # Main documentation
└── requirements.yaml                     # Requirements definitions
```

Key directories:

- **src/** - Contains the main library implementation
- **test/** - Contains unit tests and test resources
- **docs/** - Contains documentation source files and templates
- **.github/** - Contains CI/CD workflows and automation

Key files:

- **requirements.yaml** - Defines the functional, quality, and documentation requirements
  for the library. All requirements should be traceable to test cases.

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

# Format code
dotnet format

# Verify formatting
dotnet format --verify-no-changes
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
- **Test Naming Convention**: `ClassName_MethodUnderTest_Scenario_ExpectedBehavior`
  - `ClassName`: The name of the class being tested (e.g., `TrxSerializer`, `TestOutcome`)
  - `MethodUnderTest`: The method or property being tested (e.g., `Serialize`, `IsPassed`)
  - `Scenario`: The specific scenario or input condition (e.g., `BasicTestResults`, `PassedOutcome`)
  - `ExpectedBehavior`: The expected result (e.g., `ProducesValidTrxXml`, `ReturnsTrue`)
  - Example: `TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml`

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
- **Test methods**: Follow `ClassName_MethodUnderTest_Scenario_ExpectedBehavior` pattern
  - Example: `TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml`

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

1. **Quality Checks**: Run markdown linter, spell checker, and YAML linter
2. **Checkout**: Clone the repository
3. **Setup**: Install .NET SDKs (8, 9, 10)
4. **Restore**: Restore tools and dependencies
5. **SonarCloud Start**: Begin SonarCloud analysis (Linux only)
6. **Build**: Compile in Release mode on both Windows and Linux
7. **Test**: Run all tests with code coverage and generate TRX test result files
8. **SonarCloud End**: Complete SonarCloud analysis (Linux only)
9. **SBOM**: Generate Software Bill of Materials
10. **Package**: Create NuGet packages
11. **Build Documentation**:
    - Import requirements from requirements.yaml
    - Import TRX test results
    - Export requirements and trace matrix documents
    - Enforce requirements coverage
    - Generate PDF documentation (Developer Guide, Requirements, Trace Matrix)

### Quality Gates

All builds must pass:

- Compilation with zero warnings
- All unit tests passing
- SonarCloud quality gate
- Code coverage thresholds
- Requirements coverage validation

## Requirements Management

The project uses DemaConsulting.ReqStream for requirements management and traceability.

### Requirements File

The `requirements.yaml` file defines all functional, quality, and documentation requirements for the library. Each requirement:

- Has a unique identifier (e.g., REQ-FUNC-001)
- Has a descriptive title
- Is mapped to one or more test cases
- Is organized into hierarchical sections

### Requirements Process

When working on this project:

1. **Review Requirements**: Check `requirements.yaml` to understand existing requirements
2. **Add New Requirements**: When adding significant new functionality, add corresponding requirements to `requirements.yaml`
3. **Update Test Mappings**: Ensure all requirements reference the appropriate test cases
4. **Verify Coverage**: The CI/CD pipeline enforces that all requirements are covered by passing tests

### Requirements Documentation

The build process generates:

- **Requirements Specification PDF**: Complete requirements document
- **Trace Matrix PDF**: Shows mapping between requirements and test cases with pass/fail status

## Common Tasks for AI Agents

### Adding a New Feature

1. Read ARCHITECTURE.md to understand the design
2. Review requirements.yaml to see if a requirement exists for this feature
3. If adding significant new functionality, add appropriate requirements to requirements.yaml
4. Create or update the domain model classes
5. Update serialization logic if needed
6. Add comprehensive unit tests
7. Update requirements.yaml to map requirements to new test cases
8. Update XML documentation
9. Update README.md with usage examples
10. Run all tests and ensure they pass
11. Complete pre-finalization quality checks (see below)

### Fixing a Bug

1. Reproduce the bug with a failing test
2. Fix the bug with minimal changes
3. Ensure the test now passes
4. Verify no other tests are broken
5. Update documentation if the bug fix changes behavior
6. Complete pre-finalization quality checks (see below)

### Improving Code Quality

1. Run static analysis and address warnings
2. Review code coverage reports
3. Refactor duplicated code
4. Improve naming and clarity
5. Add missing documentation
6. Verify all tests still pass
7. Complete pre-finalization quality checks (see below)

### Updating Dependencies

1. Check dependency versions in .csproj files
2. Update to latest stable versions when appropriate
3. Test thoroughly after updates
4. Update documentation if APIs changed
5. Complete pre-finalization quality checks (see below)

## Pre-Finalization Quality Checks

Before marking any task as complete and finalizing your session, you **MUST** run the following quality checks in this order:

### 1. Build and Test Validation

```bash
# Build the project
dotnet build --configuration Release

# Run all tests
dotnet test --configuration Release
```

All builds must complete with zero warnings and all tests must pass.

### 2. Code Review

Use the **code_review** tool to get automated feedback on your changes:

- Review all comments and suggestions
- Address relevant feedback
- If significant changes are made, run code_review again

### 3. Security Scanning

Use the **codeql_checker** tool to scan for security vulnerabilities:

- This tool must be run after code_review is complete
- Investigate all alerts discovered
- Fix any alerts that require localized changes
- Re-run codeql_checker after fixes to verify
- Include a Security Summary with any unfixed vulnerabilities

### Quality Check Workflow

The complete workflow before task completion:

1. Make code changes
2. Run build and tests → Fix any issues
3. Run code_review tool → Address relevant feedback
4. Run codeql_checker tool → Fix security issues
5. If significant changes were made, repeat steps 2-4
6. Report progress with final commit
7. Complete task

**Note**: Only proceed to finalize your task after all quality checks pass and all issues are addressed.

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
- Complete all pre-finalization quality checks (build, test, code_review, codeql_checker) before marking work as complete

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

## Custom GitHub Copilot Agents

This project has specialized GitHub Copilot agents to help with specific tasks. These agents
are configured in the `.github/agents/` directory and can be invoked for their areas of expertise.

### Available Agents

#### Code Quality Agent (`@copilot[code-quality-agent]`)

Ensures code quality through linting and static analysis:

- Running and fixing linting issues (markdown, YAML, spell check, code formatting)
- Ensuring static analysis passes with zero warnings
- Verifying code security
- Enforcing quality gates before merging

**Use this agent for**: Code reviews, linting fixes, static analysis, quality gate enforcement.

#### Repo Consistency Agent (`@copilot[repo-consistency-agent]`)

Ensures this repository remains consistent with the TemplateDotNetLibrary template:

- Reviewing repository structure against the template
- Identifying drift from template standards
- Recommending updates to bring the project back in sync

**Use this agent for**: Periodic consistency reviews, checking for template drift, adopting new template patterns.

#### Requirements Agent (`@copilot[requirements-agent]`)

Develops requirements and ensures appropriate test coverage:

- Creating and reviewing requirements in `requirements.yaml`
- Ensuring requirements have appropriate test coverage
- Differentiating requirements from design details

**Use this agent for**: Adding new requirements, reviewing requirement quality, test linkage strategy.

#### Software Developer (`@copilot[software-developer]`)

Writes production code with emphasis on testability and clarity:

- Implementing production code features
- Code refactoring for testability and maintainability
- Implementing library APIs and functionality

**Use this agent for**: Feature implementation, bug fixes, refactoring production code.

#### Technical Writer (`@copilot[technical-writer]`)

Creates and maintains clear, accurate, and complete documentation:

- Creating or updating project documentation (README, guides, CONTRIBUTING, etc.)
- Ensuring documentation accuracy and completeness
- Markdown and spell checking compliance

**Use this agent for**: Documentation updates, API comments, usage examples, and documentation fixes.

#### Test Developer (`@copilot[test-developer]`)

Writes unit and integration tests following the AAA pattern:

- Creating unit tests for individual components
- Improving test coverage
- Refactoring existing tests for clarity

**Use this agent for**: Writing new tests, improving test coverage, refactoring test code.

### Using Custom Agents

Invoke agents in issues and pull requests by mentioning them:

```markdown
@copilot[technical-writer] Please update the README with examples for the new feature.
```

```markdown
@copilot[code-quality-agent] Review this PR for code quality standards.
```

```markdown
@copilot[repo-consistency-agent] Please check if this repository is consistent with the template.
```

For detailed information about each agent, see their individual files in the `.github/agents/` directory.
