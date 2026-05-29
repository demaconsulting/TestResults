## FileAssert Verification

### Verification Approach

FileAssert is verified through self-validation: the tool is invoked with `--version` and
`--help` flags to confirm it is installed and responds correctly. This provides objective
evidence that the tool binary is present, loads successfully, and is the expected version. The
tests `FileAssert_VersionDisplay` and `FileAssert_HelpDisplay` are named in the OTS requirements
and tracked by ReqStream.

### Test Environment

The standard CI pipeline environment with the `dotnet-tools.json` manifest restored. No
network access, files, or external services are required for the self-validation tests.

### Acceptance Criteria

- `fileassert --version` exits with code 0 and prints a version string to standard output.
- `fileassert --help` exits with code 0 and prints usage information to standard output.
- The requirement `TestResults-OTS-FileAssert` is linked to both named test identifiers in the
  ReqStream trace matrix.

### Test Scenarios

**Version display**: FileAssert shall respond to `--version` with a version string and exit
code 0, confirming the tool is installed and operational. This scenario is confirmed by
`FileAssert_VersionDisplay`.

**Help display**: FileAssert shall respond to `--help` with usage information and exit code 0,
confirming the tool loads and parses arguments correctly. This scenario is confirmed by
`FileAssert_HelpDisplay`.
