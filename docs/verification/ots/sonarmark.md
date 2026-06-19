## SonarMark

### Verification Approach

SonarMark is verified through CI pipeline execution evidence. The tool is invoked during the
standard CI pipeline with a valid SonarCloud project key and authentication token; a successful
run with the expected markdown output file present proves the tool is operational and correctly
integrated. The tests `SonarMark_QualityGateRetrieval`, `SonarMark_IssuesRetrieval`,
`SonarMark_HotSpotsRetrieval`, and `SonarMark_MarkdownReportGeneration` are named in the OTS
requirements and tracked by ReqStream.

### Test Environment

The GitHub Actions CI pipeline environment with network access to the SonarCloud API and a
valid `SONAR_TOKEN` environment variable. SonarMark cannot be verified in an offline
environment because it requires live API access.

### Acceptance Criteria

- The CI pipeline step invoking SonarMark exits with code 0.
- The markdown code quality document is produced at the expected output path under
  `docs/code_quality/generated/`.
- The requirement `TestResults-OTS-SonarMark` is linked to all four named test identifiers in
  the ReqStream trace matrix.

### Test Scenarios

**Quality-gate retrieval**: SonarMark shall query SonarCloud and retrieve the quality-gate
status for the project. This scenario is confirmed by `SonarMark_QualityGateRetrieval`.

**Issues retrieval**: SonarMark shall retrieve open code issues from SonarCloud. This scenario
is confirmed by `SonarMark_IssuesRetrieval`.

**Hotspots retrieval**: SonarMark shall retrieve security hotspots from SonarCloud.
This scenario is confirmed by `SonarMark_HotSpotsRetrieval`.

**Markdown report generation**: SonarMark shall render quality-gate status, issues, and
hotspots as a structured markdown document. This scenario is confirmed by
`SonarMark_MarkdownReportGeneration`.
