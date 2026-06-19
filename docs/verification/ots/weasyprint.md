## WeasyPrint

### Verification Approach

WeasyPrint is verified through document output validation using FileAssert. After WeasyPrint
converts each HTML document to PDF, FileAssert asserts that the PDF file exists, contains at
least one page, has the expected metadata fields (Title, Author, Subject), and includes the
expected text content. Passing all eight PDF assertions proves WeasyPrint executed correctly
across all document types. Six of these tests run before ReqStream and are named in the OTS
requirements and tracked by ReqStream. The remaining two (Requirements and Trace Matrix PDFs)
run after ReqStream due to pipeline ordering and cannot be listed as requirement test evidence.

### Test Environment

The standard CI pipeline environment with WeasyPrint installed via the `dotnet-tools.json`
manifest and the HTML source files produced by Pandoc. No network access or external services
are required.

### Acceptance Criteria

- All eight FileAssert PDF assertions pass without error.
- Each generated PDF file exists at the expected path under `docs/generated/`.
- Each PDF contains at least one page and the expected metadata fields.
- The requirement `TestResults-OTS-WeasyPrint` is linked to the six test identifiers that run
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

**User guide PDF generation**: WeasyPrint shall produce a valid PDF from the user guide HTML.
This scenario is confirmed by `WeasyPrint_UserGuidePdf`.

**Requirements PDF generation**: WeasyPrint shall produce a valid PDF from the requirements HTML.
This scenario is confirmed by `WeasyPrint_RequirementsPdf`.

**Trace matrix PDF generation**: WeasyPrint shall produce a valid PDF from the trace matrix HTML.
This scenario is confirmed by `WeasyPrint_TraceMatrixPdf`.
