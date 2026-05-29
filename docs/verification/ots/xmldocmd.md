## XmlDocMarkdown Verification

### Verification Approach

XmlDocMarkdown is verified through artifact content validation using FileAssert. After the NuGet
package is built, FileAssert asserts that the `.nupkg` zip archive contains the expected
`docs/DemaConsulting.TestResults.md` file and at least six documentation files under
`docs/**/*.md`. Passing this assertion proves XmlDocMarkdown executed correctly and produced
meaningful output that was bundled into the package. The test is named in the OTS requirements
and tracked by ReqStream.

### Test Environment

The standard CI pipeline environment after the Release build produces the library assembly, XML
documentation file, and NuGet package. No network access or external services are required.

### Acceptance Criteria

- The FileAssert NuGet package assertion passes without error.
- The `.nupkg` archive contains `docs/DemaConsulting.TestResults.md` (exactly one match).
- The `.nupkg` archive contains at least six files matching `docs/**/*.md`.
- The requirement `TestResults-OTS-XmlDocMd` is linked to the named test identifier in the
  ReqStream trace matrix.

### Test Scenarios

**NuGet package contains API documentation**: XmlDocMarkdown shall generate Markdown API
documentation and the NuGet package shall contain the expected documentation files, satisfying
`TestResults-OTS-XmlDocMd`. This scenario is confirmed by
`XmlDocMd_NuGetPackageContainsDocs`.
