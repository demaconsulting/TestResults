## SarifMark Verification

### Verification Approach

SarifMark is verified through CI pipeline execution evidence. The tool is invoked during the
standard CI pipeline after the CodeQL code scanning step; a successful run with the expected
markdown output file present proves the tool is operational and integrated correctly. The tests
`SarifMark_SarifReading`, `SarifMark_MarkdownReportGeneration`, and `SarifMark_Enforcement` are
named in the OTS requirements and tracked by ReqStream.

### Test Environment

The GitHub Actions CI pipeline environment with the CodeQL SARIF output file available. SarifMark
requires a valid SARIF input file; the test cannot be fully exercised in a local environment
without a SARIF file produced by an actual CodeQL scan.

### Acceptance Criteria

- The CI pipeline step invoking SarifMark exits with code 0.
- The markdown code quality document is produced at the expected output path under
  `docs/code_quality/generated/`.
- The requirement `TestResults-OTS-SarifMark` is linked to all three named test identifiers in
  the ReqStream trace matrix.

### Test Scenarios

**SARIF reading**: SarifMark shall read a SARIF input file and extract findings and rule
descriptions. This scenario is confirmed by `SarifMark_SarifReading`.

**Markdown report generation**: SarifMark shall render extracted findings as a structured
markdown document. This scenario is confirmed by `SarifMark_MarkdownReportGeneration`.

**Enforcement mode**: SarifMark shall exit non-zero when SARIF findings exceed the configured
threshold. This scenario is confirmed by `SarifMark_Enforcement`.
