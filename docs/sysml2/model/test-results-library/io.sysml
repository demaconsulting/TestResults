package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def IOSubsystem {
        doc /* Translates between the in-memory model and XML result formats. */

        metadata info : SoftwareInfo {
            testFiles = ("test/DemaConsulting.TestResults.Tests/IO/IOTests.cs");
            designFiles = ("docs/design/test-results-library/io.md");
            verificationFiles = ("docs/verification/test-results-library/io.md");
            requirementFiles = ("docs/reqstream/test-results-library/io.yaml");
        }

        part serializer : Serializer;
        part serializerHelpers : SerializerHelpers;
        part trxSerializer : TrxSerializer;
        part junitSerializer : JUnitSerializer;
    }
}
