# OTS Dependencies

This repository uses Off-The-Shelf (OTS) software tools as integral parts of the CI pipeline and
documentation build process. Each tool satisfies a specific requirement and is integrated as a
`dotnet` tool or NuGet package. The OTS items are listed below with their integration roles;
detailed integration design for each is in the corresponding `docs/design/ots/` file.

## Selection Criteria

OTS items are selected based on the following criteria:

- **License compatibility**: only items published under a permissive open-source license (MIT,
  Apache 2.0, or equivalent) are accepted, ensuring compliance with the project's distribution requirements.
- **Ecosystem alignment**: items must be available as a `dotnet` tool or NuGet package so they integrate naturally
  with the .NET SDK toolchain and do not require separate runtime environments.
- **Maturity and community support**: items should have an active release history, publicly accessible source, and
  demonstrated use in comparable software projects.
- **Security track record**: no unresolved critical CVEs at the time of adoption; items from the DemaConsulting
  organization are under the same source-control and review processes as the host project.
- **Qualification feasibility**: the item must be verifiable through CI pipeline execution, document output
  assertions, or library test exercise without requiring access to vendor internal test suites.

## Version Management Policy

Pinned versions are recorded in `.config/dotnet-tools.json` (for `dotnet` tool items) and in project NuGet package
references. This ensures reproducible builds across all developer workstations and CI runners.

Dependabot is configured with a weekly NuGet scan; it raises pull requests automatically for any tool or package version
upgrade. All upgrade pull requests follow the standard review and CI gate process before merging.

Major version upgrades (where the tool's public API or CLI interface changes) trigger a design review to assess whether
the affected integration design documents require updating. Minor and patch upgrades that pass CI without changes are
merged without a design review.

## General Integration Approach

Most tool-type OTS items (BuildMark, FileAssert, Pandoc, ReqStream, ReviewMark, SarifMark,
SonarMark, VersionMark, and WeasyPrint) are installed as `dotnet` tools and invoked from the CI
pipeline. xUnit is integrated as a NuGet package dependency of the test project. ApiMark is
integrated as an MSBuild NuGet package dependency of the library project.

The pipeline execution model treats a successful CI run as primary integration evidence for most
pipeline tools. Document-output tools (Pandoc, WeasyPrint, and ApiMark) additionally produce
artifacts that FileAssert validates to prove correct execution and meaningful output.

## Qualification Strategy

OTS items are qualified for use through integration evidence collected during CI pipeline execution rather than internal
unit testing of the OTS source, because their source is external and not under local development control:

- **CI pipeline execution**: successful pipeline runs prove that pipeline-tool items (BuildMark, ReqStream, ReviewMark,
  SarifMark, SonarMark, and VersionMark) are installed, invoked correctly, and produce the expected outputs.
- **Document output validation**: FileAssert assertions on generated HTML and PDF files qualify document-generation
  items (Pandoc and WeasyPrint) by confirming correct content, structure, and minimum size requirements.
- **Artifact content validation**: FileAssert assertions on the NuGet package archive qualify ApiMark by confirming
  expected documentation files are present.
- **Self-validation**: FileAssert is qualified by invoking it with `--version` and `--help` flags to confirm installation
  and operational status.
- **Library test exercise**: xUnit is qualified indirectly; if the framework were non-functional, no library tests could
  pass and the CI gate would fail.

Qualification evidence is referenced by test name in each OTS requirements YAML file and linked to requirements by
ReqStream. A successful CI pipeline run with `--enforce` confirms that all referenced evidence exists and is current.

## OTS Items

| OTS Item | Role | Integration |
| --- | --- | --- |
| BuildMark | Build documentation | Dotnet tool — generates build-notes from GitHub Actions metadata |
| FileAssert | Document validation | Dotnet tool — validates HTML, PDF, and zip outputs against acceptance criteria |
| Pandoc | HTML generation | Dotnet tool wrapper — converts Markdown to HTML for each document collection |
| ReqStream | Requirements traceability | Dotnet tool — links requirements to test evidence and enforces coverage |
| ReviewMark | File review tracking | Dotnet tool — generates review plans and reports from `.reviewmark.yaml` |
| SarifMark | Code quality reporting | Dotnet tool — converts CodeQL SARIF output to a markdown report |
| SonarMark | Quality gate reporting | Dotnet tool — retrieves SonarCloud metrics and renders markdown reports |
| VersionMark | Version capture | Dotnet tool — captures and publishes tool-version information |
| WeasyPrint | PDF generation | Dotnet tool wrapper — converts HTML documents to PDF |
| xUnit | Test framework | NuGet packages — discovers and runs tests; produces TRX result files |
| ApiMark | API documentation | NuGet package (`ApiMark.MSBuild`) — generates Markdown API docs during build |

See *BuildMark Integration Design*, *FileAssert Integration Design*, *Pandoc Integration Design*,
*ReqStream Integration Design*, *ReviewMark Integration Design*, *SarifMark Integration Design*,
*SonarMark Integration Design*, *VersionMark Integration Design*, *WeasyPrint Integration Design*,
*xUnit Integration Design*, and *ApiMark Integration Design* for detailed per-item design.
