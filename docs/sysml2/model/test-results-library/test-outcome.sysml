package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def TestOutcome {
        doc /* Defines supported test outcomes and classification helpers. */

        metadata info : SoftwareInfo {
            description = "Defines supported test outcomes and classification helpers.";
            sourceFiles = ("src/DemaConsulting.TestResults/TestOutcome.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/TestOutcomeTests.cs");
            designFiles = ("docs/design/test-results-library/test-outcome.md");
            verificationFiles = ("docs/verification/test-results-library/test-outcome.md");
            requirementFiles = ("docs/reqstream/test-results-library/test-outcome.yaml");
        }
    }
}
