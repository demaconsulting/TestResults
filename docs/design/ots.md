# OTS Software Items

This repository uses Off-The-Shelf (OTS) software tools as integral parts of the CI pipeline and
documentation build process. Each tool satisfies a specific requirement and is integrated as a
`dotnet` tool or NuGet package. The OTS items are listed below with their integration roles;
detailed integration design for each is in the corresponding `docs/design/ots/` file.

## OTS Integration Strategy

Most tool-type OTS items (BuildMark, FileAssert, Pandoc, ReqStream, ReviewMark, SarifMark,
SonarMark, VersionMark, and WeasyPrint) are installed as `dotnet` tools and invoked from the CI
pipeline. xUnit is integrated as a NuGet package dependency of the test project. ApiMark is
integrated as an MSBuild NuGet package dependency of the library project.

The pipeline execution model treats a successful CI run as primary integration evidence for most
pipeline tools. Document-output tools (Pandoc, WeasyPrint, and ApiMark) additionally produce
artifacts that FileAssert validates to prove correct execution and meaningful output.

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
| ApiMark | API documentation | NuGet package (`DemaConsulting.ApiMark.MSBuild`) — generates Markdown API docs during build |

See *BuildMark Integration Design*, *FileAssert Integration Design*, *Pandoc Integration Design*,
*ReqStream Integration Design*, *ReviewMark Integration Design*, *SarifMark Integration Design*,
*SonarMark Integration Design*, *VersionMark Integration Design*, *WeasyPrint Integration Design*,
*xUnit Integration Design*, and *ApiMark Integration Design* for detailed per-item design.
