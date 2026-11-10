package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    doc
    /*
     * TestResultsLibrary is a .NET library for reading and writing test result files in
     * TRX and JUnit XML formats.
     */

    part def TestResultsLibrarySystem {
        doc /* The TestResultsLibrary system as a whole. */

        metadata info : SoftwareInfo {
            description = "The TestResultsLibrary system as a whole.";
            testFiles = ("test/DemaConsulting.TestResults.Tests/TestResultsLibraryTests.cs");
            designFiles = ("docs/design/test-results-library.md");
            verificationFiles = ("docs/verification/test-results-library.md");
            requirementFiles = ("docs/reqstream/test-results-library.yaml");
        }

        part io : IOSubsystem;
        part testOutcome : TestOutcome;
        part testResult : TestResult;
        part testResults : TestResults;
    }
}
