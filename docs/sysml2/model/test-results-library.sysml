package TestResultsLibrary {
    doc
    /*
     * TestResultsLibrary is a .NET library for reading and writing test result files in
     * TRX and JUnit XML formats.
     */

    part def TestResultsLibrarySystem {
        doc /* The TestResultsLibrary system as a whole. */

        comment testRef /* Test: test/DemaConsulting.TestResults.Tests/TestResultsLibraryTests.cs */
        comment designRef /* Design: docs/design/test-results-library.md */
        comment verificationRef /* Verification: docs/verification/test-results-library.md */
        comment reqRef /* Requirements: docs/reqstream/test-results-library.yaml */

        part io : IOSubsystem;
        part testOutcome : TestOutcome;
        part testResult : TestResult;
        part testResults : TestResults;
    }
}
