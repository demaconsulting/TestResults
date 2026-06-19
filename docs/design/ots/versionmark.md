## VersionMark

### Purpose

VersionMark is a version-capture tool that reads version metadata for each `dotnet` tool used
in the CI pipeline and writes a versions markdown document included in the release artifacts.
It is chosen to provide a traceable record of the exact tool versions used to produce each
release build.

### Features Used

- **Version capture**: queries each installed `dotnet` tool by invoking its `--version` flag
  and captures the reported version string.
- **Markdown report generation**: writes a structured markdown document listing each tool name
  and its captured version to `docs/build_notes/generated/versions.md`.

### Integration Pattern

VersionMark is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is invoked
early in the CI pipeline after all other tools are restored, so that it captures the versions
actually used for the build. It writes its output to `docs/build_notes/generated/versions.md`,
which is included alongside the build-notes markdown when Pandoc assembles the build-notes HTML.
No initialization or disposal steps are required; the tool captures versions, writes its output,
and exits.
