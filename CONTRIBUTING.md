# Contributing to TestResults

First off, thank you for considering contributing to TestResults! It's people like you that make TestResults such a
great tool.

## Code of Conduct

This project and everyone participating in it is governed by our
[Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report
unacceptable behavior through GitHub issues.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues as you might find out that you don't need to create one.
When you are creating a bug report, please include as many details as possible:

- **Use a clear and descriptive title** for the issue to identify the problem
- **Describe the exact steps which reproduce the problem** in as much detail as possible
- **Provide specific examples to demonstrate the steps**
- **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that
  behavior
- **Explain which behavior you expected to see instead and why**
- **Include code samples and test cases** if applicable

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion, please include:

- **Use a clear and descriptive title** for the issue to identify the suggestion
- **Provide a step-by-step description of the suggested enhancement** in as much detail as possible
- **Provide specific examples to demonstrate the steps** or provide examples of how the enhancement would be used
- **Describe the current behavior** and **explain which behavior you expected to see instead** and why
- **Explain why this enhancement would be useful** to most TestResults users

### Pull Requests

- Fill in the pull request template
- Follow the C# coding style used throughout the project
- Include unit tests for new functionality
- Update documentation to reflect any changes
- Ensure all tests pass and there are no linting errors

## Development Setup

### Prerequisites

- [.NET SDK 8.0, 9.0, or 10.0](https://dotnet.microsoft.com/download)
- A code editor ([Visual Studio](https://visualstudio.microsoft.com/),
  [Visual Studio Code](https://code.visualstudio.com/), or [JetBrains Rider](https://www.jetbrains.com/rider/))

### Getting Started

1. Fork the repository
2. Clone your fork:

   ```bash
   git clone https://github.com/YOUR-USERNAME/TestResults.git
   cd TestResults
   ```

3. Restore .NET tools:

   ```bash
   dotnet tool restore
   ```

4. Restore dependencies:

   ```bash
   dotnet restore
   ```

5. Build the project:

   ```bash
   dotnet build
   ```

6. Run tests:

   ```bash
   dotnet test
   ```

### Helper Scripts

The repository includes helper scripts to simplify common development tasks:

**Quick Build and Test:**

```bash
# On Windows
build.bat

# On Linux/macOS
./build.sh
```

**Quick Linting:**

```bash
# On Windows
lint.bat

# On Linux/macOS
./lint.sh
```

### Visual Studio Code Tasks

If you're using Visual Studio Code, preconfigured tasks are available:

- Press `Ctrl+Shift+B` (Windows/Linux) or `Cmd+Shift+B` (macOS) to access build tasks
- Press `Ctrl+Shift+P` and type "Tasks: Run Task" to access all tasks
- Available tasks: `build`, `test`, `clean`, `restore`, `lint`, `format`

### Making Changes

1. Create a new branch for your changes:

   ```bash
   git checkout -b feature/your-feature-name
   ```

2. Make your changes, following the coding guidelines below
3. Add or update tests as needed
4. Run the full test suite to ensure nothing is broken
5. Run pre-commit quality checks (see below)
6. Commit your changes with a clear and descriptive commit message
7. Push to your fork and submit a pull request

### Pre-Commit Quality Checks

Before committing your changes, ensure the following checks pass:

**Using Helper Scripts (Recommended):**

```bash
# On Windows
build.bat && lint.bat

# On Linux/macOS
./build.sh && ./lint.sh
```

**Manual Commands:**

```bash
# Build the project
dotnet build --configuration Release

# Run all tests
dotnet test --configuration Release

# Verify code formatting
dotnet format --verify-no-changes

# (Optional) Run spell checker if you modified documentation
cspell "**/*.{md,cs}"

# (Optional) Run markdown linter if you modified markdown files
markdownlint "**/*.md"

# (Optional) Run YAML linter if you modified YAML files
yamllint '**/*.{yml,yaml}'
```

All builds must complete with zero warnings, and all tests must pass.

## Coding Guidelines

### C# Style

- Follow standard C# naming conventions:
  - PascalCase for classes, methods, properties, and public fields
  - camelCase for local variables and private fields
  - Interface names should start with 'I'
- Use meaningful and descriptive names for variables, methods, and classes
- Add XML documentation comments for all public APIs
- Keep methods focused and concise (Single Responsibility Principle)
- Prefer composition over inheritance
- Use nullable reference types appropriately

### Code Quality

- Warnings are treated as errors - ensure your code produces no warnings
- Follow the existing code style in the project
- Use the .editorconfig settings provided in the repository
- Run static analysis and address any issues
- Maintain high code coverage with unit tests

### Testing

- Write unit tests for all new functionality
- Follow the AAA (Arrange, Act, Assert) pattern
- Use descriptive test names that explain what is being tested
- Test both success and failure scenarios
- Keep tests isolated and independent
- Mock external dependencies when appropriate

### Documentation

- Update README.md if you change functionality
- Add XML documentation comments for all public APIs
- Update ARCHITECTURE.md for significant architectural changes
- Include code examples for new features

## Commit Message Guidelines

- Use the present tense ("Add feature" not "Added feature")
- Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
- Limit the first line to 72 characters or less
- Reference issues and pull requests liberally after the first line
- Consider starting the commit message with an applicable emoji:
  - ✨ `:sparkles:` when adding a new feature
  - 🐛 `:bug:` when fixing a bug
  - 📝 `:memo:` when writing docs
  - 🎨 `:art:` when improving the format/structure of the code
  - ⚡ `:zap:` when improving performance
  - ✅ `:white_check_mark:` when adding tests
  - 🔒 `:lock:` when dealing with security

## Project Structure

```text
TestResults/
├── .config/                      # .NET tool configuration
├── .github/workflows/            # CI/CD workflows
├── .vscode/                      # VS Code configuration
│   └── tasks.json               # Build, test, and lint tasks
├── src/
│   └── DemaConsulting.TestResults/   # Main library
├── test/
│   └── DemaConsulting.TestResults.Tests/  # Unit tests
├── .editorconfig                 # Editor configuration
├── .gitignore                    # Git ignore rules
├── AGENTS.md                     # AI agent instructions
├── ARCHITECTURE.md               # Architecture documentation
├── CODE_OF_CONDUCT.md            # Code of conduct
├── CONTRIBUTING.md               # This file
├── build.bat / build.sh          # Build helper scripts
├── lint.bat / lint.sh            # Lint helper scripts
├── DemaConsulting.TestResults.slnx # Solution file
├── LICENSE                       # MIT license
└── README.md                     # Main documentation
```

## Release Process

Releases are managed by the project maintainers:

1. Version numbers follow [Semantic Versioning](https://semver.org/)
2. Releases are created through GitHub releases
3. NuGet packages are automatically published on release
4. Release notes are generated from commit messages

## Questions?

Feel free to open an issue with your question or reach out to the maintainers directly through GitHub.

## Recognition

Contributors will be recognized in the project README and release notes. We appreciate all contributions, no matter
how small!

Thank you for contributing to TestResults! 🎉
