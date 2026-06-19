## Pandoc

### Verification Approach

Pandoc is verified through document output validation using FileAssert. After Pandoc converts
each Markdown document collection to HTML, FileAssert asserts that the output HTML file exists,
contains a valid `<title>` element, and includes the expected document content. Passing all
six HTML assertions proves Pandoc executed correctly across all document types produced by the
repository. The tests are named in the OTS requirements and tracked by ReqStream.

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

- All six FileAssert HTML assertions pass without error.
- Each generated HTML file exists at the expected path under `docs/{collection}/generated/`.
- Each HTML file contains a `<title>` element and the expected document title text.
- The design and verification HTML files each contain a `<nav id="TOC">` element confirming
  table of contents generation.
- The design and verification HTML files each contain at least one
  `<span class="header-section-number">` element confirming section numbering was applied.
- The requirement `TestResults-OTS-Pandoc` is linked to all six named test identifiers in the
  ReqStream trace matrix.
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

**User guide HTML generation**: Pandoc shall produce a valid HTML document from the user guide
Markdown inputs. This scenario is confirmed by `Pandoc_UserGuideHtml`.

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
