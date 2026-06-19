## FileAssert

### Purpose

FileAssert is a document assertion tool that validates generated artifacts against acceptance
criteria defined in `.fileassert.yaml`. It is chosen to provide objective, automated evidence
that pipeline-produced HTML, PDF, and NuGet package files are structurally correct and contain
expected content, acting as the primary OTS evidence mechanism for Pandoc and WeasyPrint.

### Features Used

- **File existence and size checks**: asserts that each expected output file exists and is
  non-trivial in size.
- **HTML structural validation**: queries the HTML DOM with XPath to confirm the document
  contains required elements such as a `<title>` and expected text content.
- **PDF metadata and content validation**: extracts PDF metadata fields (Title, Author, Subject)
  and asserts minimum page count and expected text content in the rendered PDF body.
- **Zip archive entry validation**: inspects NuGet package (`.nupkg`) zip archives to confirm
  that required documentation files are present with the expected path patterns.
- **Tag-based test filtering**: accepts positional tag arguments so the pipeline can run only the
  assertions relevant to a given build phase (for example, `fileassert design` or
  `fileassert user-guide`).

### Integration Pattern

FileAssert is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is invoked
multiple times during the pipeline, once per document group, with positional tag arguments
selecting the relevant subset of assertions from `.fileassert.yaml`. It runs after the
corresponding Pandoc/WeasyPrint step and before ReqStream so that document-validation results
are available as OTS evidence. The assertion criteria are maintained in `.fileassert.yaml` at
the repository root. No initialization or disposal steps are required; the tool reads the
configuration file, evaluates each assertion, and exits with a non-zero code if any assertion
fails.
