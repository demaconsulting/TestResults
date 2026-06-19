## ReqStream

### Purpose

ReqStream is a requirements traceability tool that processes requirements YAML files and TRX
test-result files to produce a requirements report, justifications document, and traceability
matrix. It is chosen to enforce mandatory test coverage: when run with `--enforce`, any
requirement lacking linked test evidence causes a build-breaking non-zero exit.

### Features Used

- **Requirements processing**: reads hierarchical requirements YAML files starting from
  `requirements.yaml`, which includes all subsystem and OTS requirement files.
- **TRX test-result consumption**: reads TRX result files produced by `dotnet test` to extract
  passing test names and link them to requirement `tests:` lists.
- **Requirements report generation**: writes a markdown requirements document to
  `docs/requirements_doc/generated/`.
- **Traceability matrix generation**: writes a markdown trace matrix to
  `docs/requirements_report/generated/`.
- **Tag-based filtering**: the `tags:` field on each requirement allows the pipeline to limit
  scope (for example, running only `ots`-tagged requirements in a pre-test phase).
- **Enforcement mode**: the `--enforce` flag exits non-zero when any requirement has no linked
  passing test, making unproven requirements a build-breaking condition.

### Integration Pattern

ReqStream is installed as a `dotnet` tool via the `dotnet-tools.json` manifest. It is invoked
after `dotnet test` produces TRX result files and after FileAssert validates document outputs.
The pipeline passes `requirements.yaml` as the root input, the TRX files as test evidence, and
`--enforce` to make coverage gaps fatal. Generated markdown files are then consumed by Pandoc to
produce the requirements HTML/PDF artifacts. No initialization or disposal steps are required
beyond ensuring TRX files exist at the declared paths before ReqStream runs.
