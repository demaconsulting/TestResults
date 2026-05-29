## ReviewMark Integration Design

### Purpose

ReviewMark is a file review tracking tool that reads `.reviewmark.yaml` and generates a review
plan and review report documenting file review coverage and currency. It is chosen to enforce
that all source, requirements, and design files are assigned to a named review-set and to
produce the evidence PDFs that demonstrate formal review compliance.

### Features Used

- **Review plan generation**: produces a markdown plan listing all review-sets, the files in
  each set, and the current review status of each file.
- **Review report generation**: produces a markdown report summarizing completed reviews and
  identifying files with stale or missing review evidence.
- **Evidence index scanning**: reads the evidence store index to determine which files have
  up-to-date review records.
- **Enforcement mode**: the `--enforce` flag exits non-zero if any file in `needs-review` is
  not covered by a review-set, making coverage gaps build-breaking.
- **Review elaboration**: the `--elaborate` flag expands a named review-set into a checklist
  artifact used during the formal review session.
- **Working directory override**: the `--working-dir` flag allows the tool to run from a
  directory other than the repository root, supporting CI matrix builds.

### Integration Pattern

ReviewMark is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. The
`.reviewmark.yaml` configuration at the repository root defines the `needs-review` patterns,
the evidence store location, and all named review-sets. The pipeline invokes ReviewMark to
generate `docs/code_review_plan/generated/plan.md` and
`docs/code_review_report/generated/report.md`. Pandoc then converts these to HTML and
WeasyPrint produces the PDF artifacts. No initialization or disposal steps are required; the
tool reads the configuration and evidence store, writes its output, and exits.
