## FileAssert Verification

### Verification Approach

FileAssert is verified through two complementary approaches:

- **Self-validation tests**: The tool is invoked with `--version` and `--help` flags to confirm
  it is installed and responds correctly, and with `--validate` to confirm its built-in assertion
  engine is functional. These tests provide objective evidence that the tool binary is present,
  loads successfully, and can perform assertions.
- **CI pipeline evidence**: FileAssert is invoked multiple times during each CI run to validate
  HTML, PDF, and NuGet package artifacts produced by Pandoc, WeasyPrint, and ApiMark. A
  passing pipeline run — in which all FileAssert assertion steps exit with code 0 — constitutes
  evidence that the document-validation capability is operating correctly.

### Test Environment

The standard CI pipeline environment with the `dotnet-tools.json` manifest restored. The
`--version` and `--help` tests require no external resources. The `--validate` self-validation
test exercises the tool's internal assertion engine without any external files.

### Acceptance Criteria

- `fileassert --version` exits with code 0 and prints a version string to standard output.
- `fileassert --help` exits with code 0 and prints usage information to standard output.
- `fileassert --validate` exits with code 0, with `FileAssert_Exists` and `FileAssert_Contains` passing,
  confirming the assertion engine is operational.
- All FileAssert assertion steps in the CI pipeline exit with code 0 in each successful build.
- Requirements `TestResults-OTS-FileAssert-Operational` and `TestResults-OTS-FileAssert-Validates`
  are each linked to their named test identifiers in the ReqStream trace matrix.

### Test Scenarios

**Version display**: FileAssert shall respond to `--version` with a version string and exit
code 0, confirming the tool is installed and operational. This scenario is confirmed by
`FileAssert_VersionDisplay`.

**Help display**: FileAssert shall respond to `--help` with usage information and exit code 0,
confirming the tool loads and parses arguments correctly. This scenario is confirmed by
`FileAssert_HelpDisplay`.

**File existence assertion**: FileAssert shall assert that files matching a glob pattern exist,
confirming the core file-validation capability. This scenario is confirmed by `FileAssert_Exists`.

**Content assertion**: FileAssert shall assert that file content contains expected text,
confirming text-based validation capability. This scenario is confirmed by `FileAssert_Contains`.
