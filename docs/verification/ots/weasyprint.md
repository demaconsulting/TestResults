## WeasyPrint

### Verification Approach

WeasyPrint is verified through document output validation using FileAssert. After WeasyPrint
converts each HTML document to PDF, FileAssert asserts that the PDF file exists, contains at
least one page, has the expected metadata fields (Title, Author, Subject), and includes the
expected text content.

FileAssert validates all nine WeasyPrint PDF outputs across the pipeline, serving two distinct
purposes. First, all nine assertions act as build guards: if any PDF file is missing, malformed,
or lacks expected content, the build fails immediately. Second, seven of the nine assertions
(Build Notes, Code Quality, Review Plan, Review Report, Design, Verification, and User Guide)
run before ReqStream and serve as requirements evidence for the OTS requirements. The remaining
two assertions (Requirements and Trace Matrix) run after ReqStream publishes the requirements
document, so they cannot be listed as requirement test evidence due to pipeline ordering
(temporal dependency).

### Test Environment

The standard CI pipeline environment with WeasyPrint installed via the `dotnet-tools.json`
manifest and the HTML source files produced by Pandoc. No network access or external services
are required.

### Acceptance Criteria

- All nine FileAssert PDF assertions pass without error.
- Each generated PDF file exists at the expected path under `docs/generated/`.
- Each PDF contains at least one page and the expected metadata fields.
- The requirement `TestResults-OTS-WeasyPrint` is linked to the seven test identifiers that run
  before ReqStream in the CI pipeline. `WeasyPrint_RequirementsPdf` and
  `WeasyPrint_TraceMatrixPdf` are excluded from requirement tracing because they run after
  ReqStream publishes the requirements document (temporal dependency).

### Test Scenarios

**Build notes PDF generation**: WeasyPrint shall produce a valid PDF from the build-notes HTML.
This scenario is confirmed by `WeasyPrint_BuildNotesPdf`.

**Code quality PDF generation**: WeasyPrint shall produce a valid PDF from the code quality
HTML. This scenario is confirmed by `WeasyPrint_CodeQualityPdf`.

**Review plan PDF generation**: WeasyPrint shall produce a valid PDF from the review plan HTML.
This scenario is confirmed by `WeasyPrint_ReviewPlanPdf`.

**Review report PDF generation**: WeasyPrint shall produce a valid PDF from the review report
HTML. This scenario is confirmed by `WeasyPrint_ReviewReportPdf`.

**Design PDF generation**: WeasyPrint shall produce a valid PDF from the software design HTML.
This scenario is confirmed by `WeasyPrint_DesignPdf`.

**Verification PDF generation**: WeasyPrint shall produce a valid PDF from the verification
HTML. This scenario is confirmed by `WeasyPrint_VerificationPdf`.

**User guide PDF generation**: WeasyPrint shall produce a valid PDF from the user guide HTML.
This scenario is confirmed by `WeasyPrint_UserGuidePdf`.

**Requirements PDF generation**: WeasyPrint shall produce a valid PDF from the requirements HTML.
This assertion acts as a build guard confirming the requirements document was produced correctly.
It runs after ReqStream and is not included as requirement test evidence. This scenario is
confirmed by `WeasyPrint_RequirementsPdf`.

**Trace matrix PDF generation**: WeasyPrint shall produce a valid PDF from the trace matrix HTML.
This assertion acts as a build guard confirming the trace matrix was produced correctly. It runs
after ReqStream and is not included as requirement test evidence. This scenario is confirmed by
`WeasyPrint_TraceMatrixPdf`.
