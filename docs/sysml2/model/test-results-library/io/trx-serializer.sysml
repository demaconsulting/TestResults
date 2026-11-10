package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def TrxSerializer {
        doc /* Serializes and deserializes TRX test result documents. */

        metadata info : SoftwareInfo {
            description = "Serializes and deserializes TRX test result documents.";
            sourceFiles = ("src/DemaConsulting.TestResults/IO/TrxSerializer.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/IO/TrxSerializerTests.cs");
            designFiles = ("docs/design/test-results-library/io/trx-serializer.md");
            verificationFiles = ("docs/verification/test-results-library/io/trx-serializer.md");
            requirementFiles = ("docs/reqstream/test-results-library/io/trx-serializer.yaml");
        }
    }
}
