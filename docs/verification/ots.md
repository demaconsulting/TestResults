# OTS Software Item Verification

This document describes the overall verification strategy for the OTS software tools used in the
TestResults repository. Detailed verification evidence for each OTS item is in the corresponding
`docs/verification/ots/` file.

## OTS Verification Strategy

OTS items are verified through integration evidence rather than internal unit tests, because
their source is external and not under local development control. Three evidence categories are
used:

- **CI pipeline execution evidence**: the tool is invoked in the standard CI pipeline; a
  successful run with no errors proves the tool is operational and integrated correctly. This
  applies to BuildMark, ReqStream, ReviewMark, SarifMark, SonarMark, and VersionMark.
- **Document output validation**: FileAssert asserts that each generated HTML or PDF file
  exists, is structurally valid, contains the expected metadata and text, and meets minimum size
  and page-count requirements. This applies to Pandoc and WeasyPrint.
- **Artifact content validation**: FileAssert asserts that the NuGet package archive contains
  the expected documentation files. This applies to ApiMark.
- **Self-validation**: the tool is invoked with `--version` or `--help` flags to confirm it is
  installed and operational. This applies to FileAssert itself and forms part of the evidence for
  ReviewMark.
- **Library test exercise**: the OTS test framework (xUnit) is verified through the passing
  library tests it executes; if xUnit were non-functional, those tests could not pass.

Test evidence is referenced by test name in each OTS requirements YAML file and linked to
requirements by ReqStream. A successful CI pipeline run with `--enforce` proves all referenced
tests exist and are passing.

## OTS Items

| OTS Item | Verification Category | Evidence Source |
| --- | --- | --- |
| BuildMark | CI pipeline execution | Pipeline run success |
| FileAssert | Self-validation | `FileAssert_VersionDisplay`, `FileAssert_HelpDisplay` |
| Pandoc | Document output validation | FileAssert HTML assertions |
| ReqStream | CI pipeline execution | Pipeline run success with `--enforce` |
| ReviewMark | CI pipeline execution + self-validation | Pipeline run; ReviewMark assertions |
| SarifMark | CI pipeline execution | Pipeline run success |
| SonarMark | CI pipeline execution | Pipeline run success |
| VersionMark | CI pipeline execution | Pipeline run success |
| WeasyPrint | Document output validation | FileAssert PDF assertions |
| xUnit | Library test exercise | Passing library tests |
| ApiMark | Artifact content validation | FileAssert NuGet package assertions |

See the individual verification files under *OTS Software Item Verification* for per-item
acceptance criteria and test scenario details.
