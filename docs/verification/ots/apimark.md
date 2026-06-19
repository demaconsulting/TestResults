## ApiMark

### Verification Approach

ApiMark is verified through artifact content validation using FileAssert. After the NuGet package
is built, FileAssert asserts that the `.nupkg` zip archive contains
`api/DemaConsulting.TestResults.md`, `api/api.md`, and at least six documentation files under
`api/**/*.md`. Passing this assertion proves ApiMark executed correctly and produced meaningful
output that was bundled into the package. The test is named in the OTS requirements and tracked by
ReqStream.

### Test Environment

The standard CI pipeline environment after the Release build produces the library assembly, XML
documentation file, and NuGet package. No network access or external services are required.

### Acceptance Criteria

- The FileAssert NuGet package assertion passes without error.
- The `.nupkg` archive contains `api/DemaConsulting.TestResults.md` (exactly one match).
- The `.nupkg` archive contains `api/api.md` (exactly one match).
- The `.nupkg` archive contains at least six files matching `api/**/*.md`.
- The requirement `TestResults-OTS-ApiMark` is linked to the named test identifier in the
  ReqStream trace matrix.

### Test Scenarios

**NuGet package contains API documentation**: ApiMark shall generate Markdown API documentation and
the NuGet package shall contain the expected documentation files, satisfying
`TestResults-OTS-ApiMark`. This scenario is confirmed by
`ApiMark_NuGetPackageContainsDocs`.
