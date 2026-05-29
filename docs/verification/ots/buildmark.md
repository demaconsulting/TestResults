## BuildMark Verification

### Verification Approach

BuildMark is verified through CI pipeline execution evidence. The tool is invoked as part of the
standard release pipeline; a successful run with no errors and with the expected output file
present proves the tool is operational. The tests `BuildMark_MarkdownReportGeneration`,
`BuildMark_GitIntegration`, `BuildMark_IssueTracking`, and `BuildMark_KnownIssuesReporting` are
named in the OTS requirements and tracked by ReqStream to confirm each functional aspect was
exercised.

### Test Environment

The GitHub Actions CI pipeline environment with access to the GitHub API, a valid `GITHUB_TOKEN`
environment variable, and an active repository with workflow run history. BuildMark cannot be
verified in a purely local offline environment because it requires live GitHub API access.

### Acceptance Criteria

- The CI pipeline step invoking BuildMark completes without a non-zero exit code.
- The output file `docs/build_notes/generated/build_notes.md` is produced and contains a
  non-trivial amount of content.
- The requirements `TestResults-OTS-BuildMark` is linked to all four named test identifiers in
  the ReqStream trace matrix.

### Test Scenarios

**Markdown report generation**: BuildMark shall produce a markdown build-notes document from the
current workflow run metadata, satisfying `TestResults-OTS-BuildMark`. This scenario is
confirmed by `BuildMark_MarkdownReportGeneration`.

**Git integration**: BuildMark shall record the commit SHA and branch associated with the build.
This scenario is confirmed by `BuildMark_GitIntegration`.

**Issue tracking**: BuildMark shall list GitHub issues linked to the triggering commit or pull
request. This scenario is confirmed by `BuildMark_IssueTracking`.

**Known-issues reporting**: BuildMark shall collect and report open issues labeled as known
defects. This scenario is confirmed by `BuildMark_KnownIssuesReporting`.
