package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def JUnitSerializer {
        doc /* Serializes and deserializes JUnit XML test result documents. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.TestResults/IO/JUnitSerializer.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/IO/JUnitSerializerTests.cs");
            designFiles = ("docs/design/test-results-library/io/junit-serializer.md");
            verificationFiles = ("docs/verification/test-results-library/io/junit-serializer.md");
            requirementFiles = ("docs/reqstream/test-results-library/io/junit-serializer.yaml");
        }
    }
}
