## Pandoc

### Verification Approach

Pandoc is verified through document output validation using FileAssert. After Pandoc converts
each Markdown document collection to HTML, FileAssert asserts that the output HTML file exists,
contains a valid `<title>` element, and includes the expected document content.

FileAssert validates all nine Pandoc HTML outputs across the pipeline, serving two distinct
purposes. First, all nine assertions act as build guards: if any HTML file is missing, malformed,
or lacks expected content, the build fails immediately. Second, seven of the nine assertions
(Build Notes, Code Quality, Review Plan, Review Report, Design, Verification, and User Guide)
run before ReqStream and serve as requirements evidence for the OTS requirements. The remaining
two assertions (Requirements and Trace Matrix) run after ReqStream publishes the requirements
document, so they cannot be listed as requirement test evidence due to pipeline ordering
(temporal dependency).

In addition to basic conversion, the build pipeline depends on three document-structure behaviors
that Pandoc must support: template-driven output, table of contents generation, and numbered
sections. These behaviors are configured via `template: template.html`,
`table-of-contents: true`, and `number-sections: true` in each document collection's
`definition.yaml`. FileAssert validates the design and verification HTML outputs for structural
evidence of each behavior.

### Test Environment

The standard CI pipeline environment with Pandoc installed via the `dotnet-tools.json` manifest
and the Markdown source files present in the repository. No network access or external services
are required.

### Acceptance Criteria

- All nine FileAssert HTML assertions pass without error.
- Each generated HTML file exists at the expected path under `docs/{collection}/generated/`.
- Each HTML file contains a `<title>` element and the expected document title text.
- The design and verification HTML files each contain a `<nav id="TOC">` element confirming
  table of contents generation.
- The design and verification HTML files each contain at least one
  `<span class="header-section-number">` element confirming section numbering was applied.
- The requirement `TestResults-OTS-Pandoc` is linked to the seven test identifiers that run
  before ReqStream in the CI pipeline. `Pandoc_RequirementsHtml` and `Pandoc_TraceMatrixHtml`
  are excluded from requirement tracing because they run after ReqStream publishes the
  requirements document (temporal dependency).
- The requirements `TestResults-OTS-Pandoc-Template`, `TestResults-OTS-Pandoc-Toc`, and
  `TestResults-OTS-Pandoc-NumberSections` are each linked to the design and verification HTML
  test identifiers in the ReqStream trace matrix.

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

**Verification HTML generation**: Pandoc shall produce a valid HTML document from the
verification Markdown inputs. This scenario is confirmed by `Pandoc_VerificationHtml`.

**User guide HTML generation**: Pandoc shall produce a valid HTML document from the user guide
Markdown inputs. This scenario is confirmed by `Pandoc_UserGuideHtml`.

**Requirements HTML generation**: Pandoc shall produce a valid HTML document from the generated
requirements Markdown. This assertion acts as a build guard confirming the requirements document
was produced correctly. It runs after ReqStream and is not included as requirement test evidence.
This scenario is confirmed by `Pandoc_RequirementsHtml`.

**Trace matrix HTML generation**: Pandoc shall produce a valid HTML document from the generated
trace matrix Markdown. This assertion acts as a build guard confirming the trace matrix was
produced correctly. It runs after ReqStream and is not included as requirement test evidence.
This scenario is confirmed by `Pandoc_TraceMatrixHtml`.

**Template-driven output**: Pandoc shall apply the custom HTML template (`template.html`) and
produce a standalone document with a `<head>` section including a `<title>` element. Without
the template, Pandoc would emit an HTML fragment with no `<head>` or `<title>`. This scenario
is confirmed by the `//head/title` HTML assertion in `Pandoc_DesignHtml` and
`Pandoc_VerificationHtml`.

**Table of contents generation**: Pandoc shall insert a `<nav id="TOC">` element into the
output when the `table-of-contents: true` option is active. The design and verification
documents each contain multiple top-level sections, making TOC generation observable. This
scenario is confirmed by the `//nav[@id='TOC']` HTML assertion in `Pandoc_DesignHtml` and
`Pandoc_VerificationHtml`.

**Numbered section headings**: Pandoc shall prepend sequential numbers to section headings when
the `number-sections: true` option is active, inserting a
`<span class="header-section-number">` element inside each heading. This scenario is confirmed
by the `//span[@class='header-section-number']` HTML assertion in `Pandoc_DesignHtml` and
`Pandoc_VerificationHtml`.
