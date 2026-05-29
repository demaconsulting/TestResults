## BuildMark Integration Design

### Purpose

BuildMark is a CI documentation tool that queries the GitHub API to capture workflow run details
and renders them as a markdown build-notes document included in the release artifacts. It is
chosen because it integrates directly with the GitHub Actions environment, capturing metadata
(run ID, commit, branch, issues, and known defects) without manual input.

### Features Used

- **GitHub Actions metadata query**: retrieves workflow run details, trigger information, and
  linked issues from the GitHub API using the `GITHUB_TOKEN` environment variable.
- **Git integration**: records the commit SHA and branch name associated with the build.
- **Issue tracking**: lists GitHub issues linked to the pull request or commit being built.
- **Known-issues reporting**: collects open issues labeled as known defects and includes them in
  the build-notes document for traceability.
- **Markdown build-notes generation**: renders all captured metadata into a structured markdown
  document written to `docs/build_notes/generated/build_notes.md`.

### Integration Pattern

BuildMark is installed as a `dotnet` tool via the `dotnet-tools.json` manifest and restored
before the pipeline documentation step. It is invoked once per release pipeline run after the
build and test steps succeed. The tool reads `GITHUB_TOKEN`, `GITHUB_REPOSITORY`, and
`GITHUB_RUN_ID` from the GitHub Actions environment and writes its output to
`docs/build_notes/generated/build_notes.md`. That file is then consumed by Pandoc to produce
the build-notes HTML, which WeasyPrint converts to the release PDF. No runtime configuration
file is required beyond the standard GitHub Actions environment variables. Disposal is implicit:
the tool exits after writing its output file.
