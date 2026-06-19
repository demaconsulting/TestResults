## Pandoc

### Purpose

Pandoc is a document conversion tool that converts Markdown source files to HTML as part of the
documentation build pipeline. It is chosen because it supports a consistent template-driven
output with table of contents and numbered sections, producing the HTML that WeasyPrint
subsequently converts to PDF.

### Features Used

- **Markdown-to-HTML conversion**: reads one or more Markdown input files concatenated in
  declaration order and emits a single HTML document.
- **Document templating**: applies a custom HTML template (`docs/template/template.html`) to
  control the layout, fonts, and styling of the output.
- **Table of contents generation**: inserts a navigable table of contents derived from the
  heading structure of the combined input files.
- **Numbered sections**: automatically numbers headings so the compiled PDF matches the section
  numbering expected by compliance reviewers.

### Integration Pattern

Pandoc is installed as a `dotnet` tool wrapper (`DemaConsulting.PandocTool`) via the
`dotnet-tools.json` manifest. Each document collection under `docs/` defines a
`definition.yaml` file that lists the input Markdown files in reading order, specifies the
template, and enables table-of-contents and numbered-sections options. The pipeline invokes
Pandoc once per collection, passing the `definition.yaml` as the configuration source. Output
HTML files are written to `docs/{collection}/generated/`. No runtime disposal is required; the
tool converts its input and exits. The resulting HTML files are consumed by WeasyPrint to
produce the final PDF artifacts and by FileAssert to validate correct output.
