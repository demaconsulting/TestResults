## VersionMark

### Verification Approach

VersionMark is verified through CI pipeline execution evidence. The tool is invoked during the
standard CI pipeline after all dotnet tools are restored; a successful run with the expected
versions markdown file present proves the tool is operational and integrated correctly. The
tests `VersionMark_CapturesVersions` and `VersionMark_GeneratesMarkdownReport` are named in the
OTS requirements and tracked by ReqStream.

### Test Environment

The standard CI pipeline environment with all dotnet tools restored via the `dotnet-tools.json`
manifest. No network access or external services are required.

### Acceptance Criteria

- The CI pipeline step invoking VersionMark exits with code 0.
- The versions markdown document is produced at `docs/build_notes/generated/versions.md`.
- The requirement `TestResults-OTS-VersionMark` is linked to both named test identifiers in the
  ReqStream trace matrix.

### Test Scenarios

**Captures versions**: VersionMark shall query each installed dotnet tool and capture its
reported version string. This scenario is confirmed by `VersionMark_CapturesVersions`.

**Generates markdown report**: VersionMark shall write a markdown document listing all captured
tool versions. This scenario is confirmed by `VersionMark_GeneratesMarkdownReport`.
