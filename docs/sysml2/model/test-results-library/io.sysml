package TestResultsLibrary {
    part def IOSubsystem {
        doc /* Translates between the in-memory model and XML result formats. */

        comment testRef /* Test: test/DemaConsulting.TestResults.Tests/IO/IOTests.cs */
        comment designRef /* Design: docs/design/test-results-library/io.md */
        comment verificationRef /* Verification: docs/verification/test-results-library/io.md */
        comment reqRef /* Requirements: docs/reqstream/test-results-library/io.yaml */

        part serializer : Serializer;
        part serializerHelpers : SerializerHelpers;
        part trxSerializer : TrxSerializer;
        part junitSerializer : JUnitSerializer;
    }
}
