## XmlDocMarkdown Integration Design

### Purpose

XmlDocMarkdown is an API documentation generator that produces Markdown
documentation from the TestResults library assembly and its XML documentation file. It is chosen
because it generates per-type and per-member Markdown files that can be bundled into the NuGet
package, making all public types and members discoverable by consumers and automated agents.

### Features Used

- **Assembly-based API documentation generation**: reads the compiled library assembly
  (`DemaConsulting.TestResults.dll`) and its companion XML documentation file to extract all
  public type and member documentation comments.
- **Markdown output per type and member**: emits one Markdown file per public type plus an
  index file, organized under a `docs/` folder structure mirroring the assembly's namespace
  hierarchy.
- **NuGet package inclusion**: the generated Markdown files are included in the `docs/` folder
  of the NuGet package so that package consumers can read API documentation without visiting an
  external site.

### Integration Pattern

XmlDocMarkdown is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is
invoked after the Release build produces the library assembly and XML documentation file. The
tool writes its Markdown output into a staging folder that is then bundled into the NuGet
package by the `dotnet pack` step. FileAssert validates the resulting `.nupkg` archive to
confirm that the expected `docs/DemaConsulting.TestResults.md` and related files are present.
No initialization or disposal steps are required; the tool reads the assembly, writes Markdown,
and exits.
