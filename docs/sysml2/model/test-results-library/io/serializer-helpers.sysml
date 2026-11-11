package TestResultsLibrary {
    private import SoftwareItemMetadata::*;

    part def SerializerHelpers {
        doc /* Provides Utf8StringWriter for XML declarations. */

        metadata info : SoftwareInfo {
            sourceFiles = ("src/DemaConsulting.TestResults/IO/SerializerHelpers.cs");
            testFiles = ("test/DemaConsulting.TestResults.Tests/IO/SerializerHelpersTests.cs");
            designFiles = ("docs/design/test-results-library/io/serializer-helpers.md");
            verificationFiles = ("docs/verification/test-results-library/io/serializer-helpers.md");
            requirementFiles = ("docs/reqstream/test-results-library/io/serializer-helpers.yaml");
        }
    }
}
