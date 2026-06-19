## WeasyPrint

### Purpose

WeasyPrint is a PDF generation tool that converts HTML documents produced by Pandoc into PDF
as part of the documentation build pipeline. It is chosen because it faithfully renders the
custom HTML template with CSS page layout and produces PDF/A-compatible output suitable for
compliance archiving.

### Features Used

- **HTML-to-PDF conversion**: reads an HTML input file and writes a formatted PDF document.
- **PDF metadata embedding**: writes document metadata (Title, Author, Subject) into the PDF
  file headers, which FileAssert validates to confirm correct generation.
- **Multi-page document rendering**: renders documents of any length, preserving section
  headings, tables, code blocks, and Mermaid diagrams converted to inline SVG.

### Integration Pattern

WeasyPrint is installed as a `dotnet` tool wrapper (`DemaConsulting.WeasyPrintTool`) via the
`dotnet-tools.json` manifest. It is invoked after Pandoc generates each HTML document,
accepting the HTML input path and the target PDF output path as command-line arguments. All
final PDF artifacts are written to `docs/generated/`. FileAssert validates each PDF by checking
file existence, page count, metadata fields, and text content. No initialization or disposal
steps are required; the tool converts its input and exits.
