package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def Serializer {
        doc /* Identifies XML result formats and delegates deserialization. */

        metadata info : SoftwareInfo {
            description = "Identifies XML result formats and delegates deserialization.";
            sourceFiles = ("src/DemaConsulting.TestResults/IO/Serializer.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/IO/SerializerTests.cs");
            designFiles = ("docs/design/test-results-library/io/serializer.md");
            verificationFiles = ("docs/verification/test-results-library/io/serializer.md");
            requirementFiles = ("docs/reqstream/test-results-library/io/serializer.yaml");
        }
    }
}
