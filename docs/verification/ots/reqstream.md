## ReqStream Verification

### Verification Approach

ReqStream is verified through CI pipeline execution evidence. The tool is invoked with
`--enforce` as part of the standard CI pipeline; a successful exit (code 0) proves that all
requirements are covered by passing test evidence and that ReqStream itself is operational.
The tests `ReqStream_RequirementsProcessing`, `ReqStream_TraceMatrix`, `ReqStream_ReportExport`,
`ReqStream_TagsFiltering`, and `ReqStream_EnforcementMode` are named in the OTS requirements
and tracked by ReqStream to confirm each functional aspect was exercised.

### Test Environment

The standard CI pipeline environment with TRX result files produced by `dotnet test` and all
requirements YAML files present. ReqStream must run after the test step so that TRX files are
available as input.

### Acceptance Criteria

- The CI pipeline step invoking ReqStream with `--enforce` exits with code 0.
- The generated requirements document and trace matrix are written to their expected output
  paths under `docs/requirements_doc/generated/` and `docs/requirements_report/generated/`.
- The requirement `TestResults-OTS-ReqStream` is linked to all five named test identifiers in
  the ReqStream trace matrix.

### Test Scenarios

**Requirements processing**: ReqStream shall read hierarchical requirements YAML files and
produce a structured requirements document. This scenario is confirmed by
`ReqStream_RequirementsProcessing`.

**Trace matrix generation**: ReqStream shall produce a traceability matrix linking requirements
to test names. This scenario is confirmed by `ReqStream_TraceMatrix`.

**Report export**: ReqStream shall write its generated documents to the declared output paths.
This scenario is confirmed by `ReqStream_ReportExport`.

**Tags filtering**: ReqStream shall respect the `tags:` field on requirements to filter scope
during phased pipeline execution. This scenario is confirmed by `ReqStream_TagsFiltering`.

**Enforcement mode**: ReqStream shall exit non-zero when any requirement lacks test evidence,
making coverage gaps build-breaking. This scenario is confirmed by `ReqStream_EnforcementMode`.
