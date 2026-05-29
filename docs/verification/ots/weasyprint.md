## WeasyPrint Verification

### Verification Approach

WeasyPrint is verified through document output validation using FileAssert. After WeasyPrint
converts each HTML document to PDF, FileAssert asserts that the PDF file exists, contains at
least one page, has the expected metadata fields (Title, Author, Subject), and includes the
expected text content. Passing all six PDF assertions proves WeasyPrint executed correctly
across all document types. The tests are named in the OTS requirements and tracked by ReqStream.

### Test Environment

The standard CI pipeline environment with WeasyPrint installed via the `dotnet-tools.json`
manifest and the HTML source files produced by Pandoc. No network access or external services
are required.

### Acceptance Criteria

- All six FileAssert PDF assertions pass without error.
- Each generated PDF file exists at the expected path under `docs/generated/`.
- Each PDF contains at least one page and the expected metadata fields.
- The requirement `TestResults-OTS-WeasyPrint` is linked to all six named test identifiers in
  the ReqStream trace matrix.

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
