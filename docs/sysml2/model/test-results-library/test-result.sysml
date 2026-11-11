package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def TestResult {
        doc /* Represents one test case result with metadata and output. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.TestResults/TestResult.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/TestResultTests.cs");
            designFiles = ("docs/design/test-results-library/test-result.md");
            verificationFiles = ("docs/verification/test-results-library/test-result.md");
            requirementFiles = ("docs/reqstream/test-results-library/test-result.yaml");
        }
    }
}
