package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def TestResults {
        doc /* Represents a complete test run and its ordered results. */

        metadata info : SoftwareInfo {
            description = "Represents a complete test run and its ordered results.";
            sourceFiles = ("src/DemaConsulting.TestResults/TestResults.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/TestResultsTests.cs");
            designFiles = ("docs/design/test-results-library/test-results.md");
            verificationFiles = ("docs/verification/test-results-library/test-results.md");
            requirementFiles = ("docs/reqstream/test-results-library/test-results.yaml");
        }
    }
}
