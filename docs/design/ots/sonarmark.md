## SonarMark Integration Design

### Purpose

SonarMark is a quality reporting tool that retrieves quality-gate status and metrics data from
SonarCloud and renders them as a markdown document included in the release artifacts. It is
chosen to surface static analysis quality data from SonarCloud in a format that can be reviewed
alongside other compliance artifacts.

### Features Used

- **Quality-gate retrieval**: queries the SonarCloud API for the current quality-gate status
  (pass/fail) for the configured project key.
- **Issues retrieval**: retrieves the list of open code issues reported by SonarCloud analysis.
- **Hotspots retrieval**: retrieves security hotspots from SonarCloud.
- **Markdown report generation**: combines quality-gate status, issues, and hotspots into a
  structured markdown document written to `docs/code_quality/generated/`.

### Integration Pattern

SonarMark is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is invoked
during the CI pipeline with the SonarCloud project key and a `SONAR_TOKEN` environment variable
providing authentication. The tool writes its markdown output to `docs/code_quality/generated/`.
Pandoc then converts the collected quality markdown to HTML and WeasyPrint produces the code
quality PDF. No initialization or disposal steps are required; the tool queries the API, writes
its output, and exits. SonarMark depends on network access to the SonarCloud API and a valid
authentication token.
