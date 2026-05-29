## SarifMark Integration Design

### Purpose

SarifMark is a SARIF report processing tool that converts CodeQL SARIF output into a
human-readable markdown code quality report. It is chosen to transform the machine-readable
SARIF format produced by GitHub's CodeQL code scanning into a document suitable for inclusion
in the release artifacts and compliance review.

### Features Used

- **SARIF file reading**: reads the SARIF file produced by the CodeQL code scanning step and
  extracts findings, rule descriptions, and severity information.
- **Markdown report generation**: renders the extracted findings as a structured markdown
  document included in `docs/code_quality/generated/`.
- **Enforcement mode**: the `--enforce` flag exits non-zero if SARIF findings exceed a
  configured threshold, making code-scanning failures build-breaking.

### Integration Pattern

SarifMark is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is invoked
during the CI pipeline after the CodeQL code scanning step produces its SARIF output file. The
tool reads the SARIF file path from its command-line arguments and writes its markdown output to
`docs/code_quality/generated/`. Pandoc then converts this markdown to HTML and WeasyPrint
produces the code quality PDF. No initialization or disposal steps are required; the tool reads
its input, writes its output, and exits.
