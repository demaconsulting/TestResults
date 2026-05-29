## ReviewMark Verification

### Verification Approach

ReviewMark is verified through CI pipeline execution evidence combined with self-validation.
The tool is invoked during the standard CI pipeline to generate the review plan and review
report; a successful run proves it is operational and correctly integrated. Additional
self-validation tests (`ReviewMark_VersionDisplay`, `ReviewMark_HelpDisplay`) confirm the tool
binary loads and responds to basic flags. All eight named tests are tracked by ReqStream to
confirm each functional aspect was exercised.

### Test Environment

The standard CI pipeline environment with `.reviewmark.yaml` present at the repository root and
the evidence store index accessible at its configured URL. The `ReviewMark_IndexScan`,
`ReviewMark_Enforce`, and `ReviewMark_Elaborate` tests require a fully populated `.reviewmark.yaml`
and the CI network access to the evidence store.

### Acceptance Criteria

- The CI pipeline step invoking ReviewMark exits with code 0.
- The review plan is written to `docs/code_review_plan/generated/plan.md`.
- The review report is written to `docs/code_review_report/generated/report.md`.
- `reviewmark --version` and `reviewmark --help` exit with code 0.
- The requirements `TestResults-OTS-ReviewMark-Operational`, `TestResults-OTS-ReviewMark-PlanGeneration`,
  and `TestResults-OTS-ReviewMark-ReportGeneration` are each linked to their named test identifiers
  in the ReqStream trace matrix.

### Test Scenarios

**Version display**: ReviewMark shall respond to `--version` with a version string and exit
code 0. This scenario is confirmed by `ReviewMark_VersionDisplay`.

**Help display**: ReviewMark shall respond to `--help` with usage information and exit code 0.
This scenario is confirmed by `ReviewMark_HelpDisplay`.

**Review plan generation**: ReviewMark shall generate a review plan from `.reviewmark.yaml`.
This scenario is confirmed by `ReviewMark_ReviewPlanGeneration`.

**Review report generation**: ReviewMark shall generate a review report from `.reviewmark.yaml`
and the evidence store. This scenario is confirmed by `ReviewMark_ReviewReportGeneration`.

**Index scan**: ReviewMark shall scan the evidence store index and incorporate review currency
data into the report. This scenario is confirmed by `ReviewMark_IndexScan`.

**Working directory override**: ReviewMark shall accept a `--working-dir` argument and operate
correctly from the specified directory. This scenario is confirmed by
`ReviewMark_WorkingDirectoryOverride`.

**Enforcement mode**: ReviewMark shall exit non-zero when files in `needs-review` are not
covered by a review-set. This scenario is confirmed by `ReviewMark_Enforce`.

**Review elaboration**: ReviewMark shall expand a named review-set into a review checklist when
invoked with `--elaborate`. This scenario is confirmed by `ReviewMark_Elaborate`.
