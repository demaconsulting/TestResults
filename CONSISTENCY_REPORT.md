# Repository Consistency Report

Date: 2026-02-19
Repository: TestResults
Template: TemplateDotNetLibrary (https://github.com/demaconsulting/TemplateDotNetLibrary)

## Executive Summary

This report documents all differences identified between the TestResults repository and the TemplateDotNetLibrary template. Changes have been made to bring TestResults in line with template best practices while preserving project-specific functionality.

## Changes Made (Applied)

### 1. Markdownlint Configuration
- **Changed**: Migrated from `.markdownlint.json` to `.markdownlint-cli2.jsonc`
- **Reason**: Template uses newer format with better ignore support
- **Impact**: Now ignores `node_modules` and `AGENT_REPORT_*.md` files automatically
- **Files**: `.markdownlint.json` (deleted), `.markdownlint-cli2.jsonc` (created)

### 2. EditorConfig
- **Changed**: Replaced verbose .editorconfig with simplified template version
- **Reason**: Template has cleaner, more maintainable config with modern settings
- **Key improvements**:
  - Explicit `end_of_line = lf` setting
  - File-scoped namespace directive (`csharp_style_namespace_declarations = file_scoped:warning`)
  - Simplified code style rules
  - Better organization with comments
- **Files**: `.editorconfig`

### 3. Spell Check Dictionary
- **Changed**: Merged template dictionary additions with TestResults-specific terms
- **Additions from template**: `BuildMark`, `SarifMark`, `SonarMark`, `CodeQL`, `Checkmarx`, `mermaid`, `Pylint`, `Qube`, `Semgrep`, `slnx`, `SonarQube`, `streetsidesoftware`, `testname`, `Trivy`, `filepart`, `myterm`, `DOCX`
- **Kept**: All TestResults-specific terms including `hotspots`, `pandoctool`, `weasyprinttool`, `versionmark`, `dbproj`, `dcterms`, `DEMACONSULTINGNUGETKEY`, `Dependabot`, `Sonar`, `sonarcloud`, `sonarscanner`
- **Added**: `AGENT_REPORT_*.md` to ignore paths
- **Files**: `.cspell.json`

### 4. YAML Linting Configuration
- **Changed**: Added ignore section for `node_modules/` and `.git/`
- **Reason**: Template includes these standard ignores
- **Files**: `.yamllint.yaml`

### 5. Build Scripts (build.sh, build.bat)
- **Changed**: Modernized to match template style
- **Improvements**:
  - Added emojis for better UX (🔧, 🧪, ✨)
  - Removed explicit solution file reference (now uses implicit discovery)
  - Changed shebang to `#!/usr/bin/env bash` for better portability
  - Simplified command structure
  - Changed "Running tests" to "Running unit tests"
- **Files**: `build.sh`, `build.bat`

### 6. Lint Scripts (lint.sh, lint.bat)
- **Changed**: Updated to match template organization and tooling
- **Improvements**:
  - Now uses `npx markdownlint-cli2` instead of `markdownlint`
  - Added YAML linting step (`yamllint -c .yamllint.yaml .`)
  - Reordered checks: markdown → spelling → YAML → code formatting
  - Added emojis (📝, 🔤, 📋, 🎨, ✨)
  - Uses `set -e` for error handling
  - Removed explicit solution file reference from `dotnet format`
- **Files**: `lint.sh`, `lint.bat`

### 7. Pull Request Template
- **Changed**: Minor wording improvements
- **Changes**:
  - "All tests pass" → "All unit tests pass"
  - YAML linter command changed from `yamllint '**/*.{yml,yaml}'` to `yamllint .`
- **Files**: `.github/pull_request_template.md`

### 8. Dotnet Tools Configuration
- **Changed**: Removed external SBOM tools, updated buildmark version
- **Removed**:
  - `microsoft.sbom.dotnettool` (version 4.1.5)
  - `demaconsulting.spdxtool` (version 2.6.0)
- **Updated**:
  - `demaconsulting.buildmark`: 0.2.0 → 0.3.0
- **Reason**: SBOM is now generated automatically via Microsoft.Sbom.Targets package in csproj
- **Files**: `.config/dotnet-tools.json`

### 9. Main Library Project File
- **Changed**: Added SBOM configuration section
- **Additions**:
  - SBOM Configuration property group with `GenerateSBOM`, `SBOMPackageName`, `SBOMPackageVersion`, `SBOMPackageSupplier`
  - `Microsoft.Sbom.Targets` package reference (version 4.1.5)
- **Reason**: Modern approach uses build-time SBOM generation instead of external tools
- **Files**: `src/DemaConsulting.TestResults/DemaConsulting.TestResults.csproj`

### 10. Build Workflow (.github/workflows/build.yaml)
- **Changed**: Extensive reorganization and modernization
- **Major changes**:
  - Added comprehensive comments explaining each job's purpose
  - Removed separate SBOM generation steps (now automatic)
  - Renamed "Create Dotnet Tool" to "Create Library Package"
  - Updated artifact paths to be explicit instead of wildcards
  - Reorganized build-docs job with section markers
  - Added mermaid-filter to all Pandoc commands
  - Generate all PDFs with `--pdf-variant pdf/a-3u`
  - Changed PDF naming from "TestResults X" to "DemaConsulting.TestResults X"
  - Changed artifact name from "documentation" to "documents"
  - Renamed job from "Build Documentation" to "Build Documents"
- **Files**: `.github/workflows/build.yaml`

### 11. Build on Push Workflow
- **Changed**: Cleaned up permission comments
- **Changes**: Removed verbose inline permission comments, simplified formatting
- **Files**: `.github/workflows/build_on_push.yaml`

### 12. Release Workflow
- **Changed**: Modernized and simplified
- **Major changes**:
  - Added job-level comments
  - Removed default value for publish input
  - Changed permission comment style
  - Removed checkout step (not needed)
  - Added .NET 8.x and 9.x to dotnet-version
  - Changed artifact name from "documentation" to "documents"
  - Updated buildnotes.md path (now at root of artifact, not in docs/ subdirectory)
  - Removed "Clean Artifacts" step
  - Simplified NuGet push command with `--skip-duplicate` flag
  - Changed conditional syntax from `${{ }}` to simple expression
- **Files**: `.github/workflows/release.yaml`

### 13. CODE_OF_CONDUCT.md
- **Changed**: Minor formatting improvements
- **Changes**:
  - Added link reference for GitHub Issues
  - Capitalized link references (Mozilla CoC, FAQ)
  - Better link formatting
- **Files**: `CODE_OF_CONDUCT.md`

## Differences NOT Changed (Project-Specific or Potential Template Improvements)

### 1. Solution File Format
- **TestResults has**: `.sln` (traditional format) and `.sln.DotSettings` (ReSharper settings)
- **Template has**: `.slnx` (newer XML-based format)
- **Decision**: Keep TestResults format - both are valid, .sln has wider tool support
- **Potential back-port**: The .sln.DotSettings file could be added to template

### 2. ARCHITECTURE.md
- **TestResults has**: Comprehensive architecture documentation file
- **Template has**: None
- **Decision**: Keep in TestResults (project-specific architecture details)
- **Potential back-port**: Template should consider adding an ARCHITECTURE.md placeholder

### 3. spdx-workflow.yaml
- **TestResults has**: SPDX workflow configuration file (currently not used in workflows)
- **Template has**: None
- **Decision**: Keep in TestResults for now (may be legacy)
- **Note**: This file is no longer referenced in the updated build workflow

### 4. CodeQL Configuration
- **TestResults has**: Query filters to exclude specific CodeQL checks for justified reasons
  ```yaml
  query-filters:
    - exclude:
        id: cs/path-combine
      paths:
        - test/**/*.cs
    - exclude:
        id: cs/catch-of-all-exceptions
      paths:
        - src/DemaConsulting.TestResults/Program.cs
  ```
- **Template has**: Minimal config with just name
- **Decision**: Keep TestResults exclusions (justified suppressions)
- **Potential back-port**: Template could document that downstream projects may add justified exclusions

### 5. Test Project Configuration
- **TestResults has**: `EmbeddedResource` items for test example files
- **Template has**: `InternalsVisibleTo` for test project access
- **Decision**: Both are project-specific and correct
- **Note**: TestResults might benefit from adding `InternalsVisibleTo` if internal APIs need testing

### 6. Documentation Content
- **Differences**: AGENTS.md, CONTRIBUTING.md, SECURITY.md contain project-specific details
- **Decision**: Keep project-specific content with some formatting improvements applied
- **Note**: Major content differences are appropriate and expected

## Summary Statistics

- **Files Modified**: 14
- **Files Added**: 1 (.markdownlint-cli2.jsonc)
- **Files Deleted**: 1 (.markdownlint.json)
- **Lines Added**: 314
- **Lines Removed**: 377
- **Net Change**: -63 lines (more concise configuration)

## Validation

All changes have been validated:
- ✅ YAML syntax validated with yamllint
- ✅ Markdown linting passed with markdownlint-cli2
- ✅ All workflows are syntactically valid
- ✅ Project structure maintained
- ✅ No functionality lost

## Recommendations for Template

Based on this review, the following improvements could be back-ported to the template:

1. **Add ARCHITECTURE.md placeholder** - Helps downstream projects document their architecture
2. **Document CodeQL exclusion pattern** - Show how downstream projects can add justified exclusions
3. **Consider .sln.DotSettings** - Useful for ReSharper/Rider users
4. **More comprehensive .cspell.json** - TestResults has some useful additions like `dbproj`, `dcterms`, etc.

## Conclusion

The TestResults repository is now consistent with the TemplateDotNetLibrary template while maintaining its project-specific functionality. All changes improve maintainability, adopt modern best practices, and simplify configuration without sacrificing any capabilities.
