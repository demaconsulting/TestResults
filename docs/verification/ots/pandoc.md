## Pandoc

### Verification Approach

Pandoc is verified through document output validation using FileAssert. After Pandoc converts
each Markdown document collection to HTML, FileAssert asserts that the output HTML file exists,
contains a valid `<title>` element, and includes the expected document content. Passing all
six HTML assertions proves Pandoc executed correctly across all document types produced by the
repository. The tests are named in the OTS requirements and tracked by ReqStream.

### Test Environment

The standard CI pipeline environment with Pandoc installed via the `dotnet-tools.json` manifest
and the Markdown source files present in the repository. No network access or external services
are required.

### Acceptance Criteria

- All six FileAssert HTML assertions pass without error.
- Each generated HTML file exists at the expected path under `docs/{collection}/generated/`.
- Each HTML file contains a `<title>` element and the expected document title text.
- The requirement `TestResults-OTS-Pandoc` is linked to all six named test identifiers in the
  ReqStream trace matrix.

### Test Scenarios

**Build notes HTML generation**: Pandoc shall produce a valid HTML document from the build-notes
Markdown inputs. This scenario is confirmed by `Pandoc_BuildNotesHtml`.

**Code quality HTML generation**: Pandoc shall produce a valid HTML document from the code
quality Markdown inputs. This scenario is confirmed by `Pandoc_CodeQualityHtml`.

**Review plan HTML generation**: Pandoc shall produce a valid HTML document from the review plan
Markdown inputs. This scenario is confirmed by `Pandoc_ReviewPlanHtml`.

**Review report HTML generation**: Pandoc shall produce a valid HTML document from the review
report Markdown inputs. This scenario is confirmed by `Pandoc_ReviewReportHtml`.

**Design HTML generation**: Pandoc shall produce a valid HTML document from the software design
Markdown inputs. This scenario is confirmed by `Pandoc_DesignHtml`.

**User guide HTML generation**: Pandoc shall produce a valid HTML document from the user guide
Markdown inputs. This scenario is confirmed by `Pandoc_UserGuideHtml`.
