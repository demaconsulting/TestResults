// Copyright(c) 2025 DEMA Consulting
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using DemaConsulting.TestResults.IO;
using Xunit;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for Serializer class
/// </summary>
public sealed class SerializerTests
{
    /// <summary>
    ///     Test that Identify correctly identifies TRX format
    /// </summary>
    [Fact]
    public void Serializer_Identify_TrxContent_ReturnsTrx()
    {
        // Arrange: create a sample TRX content
        var trxContent = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" id="da21858f-2c78-442a-8ea6-51fe73762e0e" name="Test Run" runUser="User">
              <Results>
                <UnitTestResult testId="12345678-1234-1234-1234-123456789abc" testName="Test1" outcome="Passed" executionId="87654321-4321-4321-4321-cba987654321" computerName="TestComputer" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" duration="00:00:01" startTime="2025-01-01T00:00:00.000Z" endTime="2025-01-01T00:00:01.000Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output />
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="12345678-1234-1234-1234-123456789abc">
                  <Execution id="87654321-4321-4321-4321-cba987654321" />
                  <TestMethod codeBase="TestAssembly" className="TestClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
              <TestEntries>
                <TestEntry testId="12345678-1234-1234-1234-123456789abc" executionId="87654321-4321-4321-4321-cba987654321" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestEntries>
              <TestLists>
                <TestList name="All Loaded Results" id="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestLists>
              <ResultSummary outcome="Completed">
                <Counters total="1" executed="1" passed="1" failed="0" />
              </ResultSummary>
            </TestRun>
            """;

        // Act: identify format
        var format = Serializer.Identify(trxContent);

        // Assert: verify it's identified as TRX
        Assert.Equal(TestResultFormat.Trx, format);
    }

    /// <summary>
    ///     Test that Identify correctly identifies JUnit format with testsuites root
    /// </summary>
    [Fact]
    public void Serializer_Identify_JUnitTestsuitesContent_ReturnsJUnit()
    {
        // Arrange: create a sample JUnit content with testsuites root
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuites name="Test Suite">
              <testsuite name="TestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="TestClass" time="1.500" />
              </testsuite>
            </testsuites>
            """;

        // Act: identify format
        var format = Serializer.Identify(junitContent);

        // Assert: verify it's identified as JUnit
        Assert.Equal(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that Identify correctly identifies JUnit format with testsuite root
    /// </summary>
    [Fact]
    public void Serializer_Identify_JUnitTestsuiteContent_ReturnsJUnit()
    {
        // Arrange: create a sample JUnit content with testsuite root
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuite name="TestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
              <testcase name="Test1" classname="TestClass" time="1.500" />
            </testsuite>
            """;

        // Act: identify format
        var format = Serializer.Identify(junitContent);

        // Assert: verify it's identified as JUnit
        Assert.Equal(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for empty content
    /// </summary>
    [Fact]
    public void Serializer_Identify_EmptyContent_ReturnsUnknown()
    {
        // Act: identify format of empty string
        var format = Serializer.Identify(string.Empty);

        // Assert: verify it's identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for null content
    /// </summary>
    [Fact]
    public void Serializer_Identify_NullContent_ReturnsUnknown()
    {
        // Act: identify format of null string
        var format = Serializer.Identify(null!);

        // Assert: verify it's identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for whitespace content
    /// </summary>
    [Fact]
    public void Serializer_Identify_WhitespaceContent_ReturnsUnknown()
    {
        // Act: identify format of whitespace string
        var format = Serializer.Identify("   \n\t  ");

        // Assert: verify it's identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for invalid XML
    /// </summary>
    [Fact]
    public void Serializer_Identify_InvalidXml_ReturnsUnknown()
    {
        // Arrange: create invalid XML content
        var invalidXml = "<invalid><unclosed>";

        // Act: identify format
        var format = Serializer.Identify(invalidXml);

        // Assert: verify it's identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for unrecognized XML format
    /// </summary>
    [Fact]
    public void Serializer_Identify_UnrecognizedXmlFormat_ReturnsUnknown()
    {
        // Arrange: create XML with unrecognized root element
        var unrecognizedXml = """
            <?xml version="1.0"?>
            <UnknownRoot>
              <SomeData>value</SomeData>
            </UnknownRoot>
            """;

        // Act: identify format
        var format = Serializer.Identify(unrecognizedXml);

        // Assert: verify it's identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Deserialize successfully deserializes TRX content
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_TrxContent_ReturnsTestResults()
    {
        // Arrange: create a sample TRX content
        var trxContent = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" id="da21858f-2c78-442a-8ea6-51fe73762e0e" name="Test Run" runUser="User">
              <Results>
                <UnitTestResult testId="12345678-1234-1234-1234-123456789abc" testName="Test1" outcome="Passed" executionId="87654321-4321-4321-4321-cba987654321" computerName="TestComputer" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" duration="00:00:01" startTime="2025-01-01T00:00:00.000Z" endTime="2025-01-01T00:00:01.000Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output />
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="12345678-1234-1234-1234-123456789abc">
                  <Execution id="87654321-4321-4321-4321-cba987654321" />
                  <TestMethod codeBase="TestAssembly" className="TestClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
              <TestEntries>
                <TestEntry testId="12345678-1234-1234-1234-123456789abc" executionId="87654321-4321-4321-4321-cba987654321" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestEntries>
              <TestLists>
                <TestList name="All Loaded Results" id="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestLists>
              <ResultSummary outcome="Completed">
                <Counters total="1" executed="1" passed="1" failed="0" />
              </ResultSummary>
            </TestRun>
            """;

        // Act: deserialize
        var results = Serializer.Deserialize(trxContent);

        // Assert: verify results
        Assert.NotNull(results);
        Assert.Equal("Test Run", results.Name);
        Assert.Equal("User", results.UserName);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that Deserialize successfully deserializes JUnit content
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_JUnitContent_ReturnsTestResults()
    {
        // Arrange: create a sample JUnit content
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuites name="Test Suite">
              <testsuite name="TestClass" tests="2" failures="1" errors="0" skipped="0" time="2.500">
                <testcase name="Test1" classname="TestClass" time="1.500" />
                <testcase name="Test2" classname="TestClass" time="1.000">
                  <failure message="Test failed">Stack trace here</failure>
                </testcase>
              </testsuite>
            </testsuites>
            """;

        // Act: deserialize
        var results = Serializer.Deserialize(junitContent);

        // Assert: verify results
        Assert.NotNull(results);
        Assert.Equal("Test Suite", results.Name);
        Assert.Equal(2, results.Results.Count);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.Equal("Test2", results.Results[1].Name);
        Assert.Equal(TestOutcome.Failed, results.Results[1].Outcome);
        Assert.Equal("Test failed", results.Results[1].ErrorMessage);
    }

    /// <summary>
    ///     Test that Deserialize can handle real TRX example file
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_RealTrxExample_ReturnsTestResults()
    {
        // Arrange: load example TRX file
        var trxContent = TestHelpers.GetEmbeddedResource(
            "DemaConsulting.TestResults.Tests.IO.Examples.example1.trx");

        // Act: identify format
        var format = Serializer.Identify(trxContent);

        // Assert: verify format
        Assert.Equal(TestResultFormat.Trx, format);

        // Act: deserialize
        var results = Serializer.Deserialize(trxContent);

        // Assert: verify results
        Assert.NotNull(results);
        Assert.Equal("Sample TRX Import", results.Name);
        Assert.Equal("Brian Mancini", results.UserName);
        Assert.NotEmpty(results.Results);
    }

    /// <summary>
    ///     Test that Deserialize handles multiple test outcomes from TRX
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_TrxWithMultipleOutcomes_ParsesCorrectly()
    {
        // Arrange: create TRX content with different outcomes
        var trxContent = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" id="da21858f-2c78-442a-8ea6-51fe73762e0e" name="Test Run" runUser="User">
              <Results>
                <UnitTestResult testId="11111111-1111-1111-1111-111111111111" testName="PassedTest" outcome="Passed" executionId="11111111-1111-1111-1111-111111111112" computerName="Computer" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" duration="00:00:01" startTime="2025-01-01T00:00:00.000Z" endTime="2025-01-01T00:00:01.000Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output />
                </UnitTestResult>
                <UnitTestResult testId="22222222-2222-2222-2222-222222222222" testName="FailedTest" outcome="Failed" executionId="22222222-2222-2222-2222-222222222223" computerName="Computer" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" duration="00:00:01" startTime="2025-01-01T00:00:01.000Z" endTime="2025-01-01T00:00:02.000Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output>
                    <ErrorInfo>
                      <Message>Test failed</Message>
                    </ErrorInfo>
                  </Output>
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="PassedTest" id="11111111-1111-1111-1111-111111111111">
                  <Execution id="11111111-1111-1111-1111-111111111112" />
                  <TestMethod codeBase="Assembly" className="Class" name="PassedTest" />
                </UnitTest>
                <UnitTest name="FailedTest" id="22222222-2222-2222-2222-222222222222">
                  <Execution id="22222222-2222-2222-2222-222222222223" />
                  <TestMethod codeBase="Assembly" className="Class" name="FailedTest" />
                </UnitTest>
              </TestDefinitions>
              <TestEntries>
                <TestEntry testId="11111111-1111-1111-1111-111111111111" executionId="11111111-1111-1111-1111-111111111112" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
                <TestEntry testId="22222222-2222-2222-2222-222222222222" executionId="22222222-2222-2222-2222-222222222223" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestEntries>
              <TestLists>
                <TestList name="All Loaded Results" id="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestLists>
              <ResultSummary outcome="Completed">
                <Counters total="2" executed="2" passed="1" failed="1" />
              </ResultSummary>
            </TestRun>
            """;

        // Act: deserialize
        var results = Serializer.Deserialize(trxContent);

        // Assert: verify results
        Assert.NotNull(results);
        Assert.Equal(2, results.Results.Count);

        var passedTest = results.Results[0];
        Assert.Equal("PassedTest", passedTest.Name);
        Assert.Equal(TestOutcome.Passed, passedTest.Outcome);

        var failedTest = results.Results[1];
        Assert.Equal("FailedTest", failedTest.Name);
        Assert.Equal(TestOutcome.Failed, failedTest.Outcome);
        Assert.Equal("Test failed", failedTest.ErrorMessage);
    }

    /// <summary>
    ///     Test that Deserialize handles JUnit with system output and error
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_JUnitWithSystemOutput_ParsesCorrectly()
    {
        // Arrange: create JUnit content with system output
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuites name="Test Suite">
              <testsuite name="TestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="TestClass" time="1.500">
                  <system-out>Standard output</system-out>
                  <system-err>Standard error</system-err>
                </testcase>
              </testsuite>
            </testsuites>
            """;

        // Act: deserialize
        var results = Serializer.Deserialize(junitContent);

        // Assert: verify results
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal("Standard output", results.Results[0].SystemOutput);
        Assert.Equal("Standard error", results.Results[0].SystemError);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentNullException for null input
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_NullContents_ThrowsArgumentNullException()
    {
        // Arrange: null contents
        string? nullContents = null;

        // Act: attempt to deserialize null contents (combined with Assert)
        var ex = Assert.Throws<ArgumentNullException>(() => Serializer.Deserialize(nullContents!));
        Assert.Equal("contents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange: empty string
        var emptyContents = string.Empty;

        // Act: attempt to deserialize empty string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => Serializer.Deserialize(emptyContents));
        Assert.Equal("contents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange: whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act: attempt to deserialize whitespace string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => Serializer.Deserialize(whitespaceContents));
        Assert.Equal("contents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException for unknown format
    /// </summary>
    [Fact]
    public void Serializer_Deserialize_UnknownFormat_ThrowsInvalidOperationException()
    {
        // Arrange: valid XML but unknown format
        var unknownFormatXml = """
            <?xml version="1.0"?>
            <UnknownRoot>
              <SomeData>value</SomeData>
            </UnknownRoot>
            """;

        // Act: attempt to deserialize valid XML with unknown format (combined with Assert)
        var ex = Assert.Throws<InvalidOperationException>(() => Serializer.Deserialize(unknownFormatXml));
        Assert.Contains("Unable to identify test result format", ex.Message);
    }

    /// <summary>
    ///     Tests that Identify returns Unknown for a TestRun element in the wrong XML namespace.
    /// </summary>
    /// <remarks>
    ///     Proves that the TRX discriminator requires both the correct root element name
    ///     (<c>TestRun</c>) and the correct namespace URI. A document with a <c>TestRun</c>
    ///     root in a different namespace is not a TRX file and should be reported as Unknown.
    /// </remarks>
    [Fact]
    public void Serializer_Identify_TestRunInWrongNamespace_ReturnsUnknown()
    {
        // Arrange: XML with TestRun root but no TRX namespace
        var xml = """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun id="1234" name="SomeName">
              <Results />
            </TestRun>
            """;

        // Act: attempt to identify format of the XML content
        var format = Serializer.Identify(xml);

        // Assert: without the correct namespace this is not a valid TRX file
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Tests that core test data is preserved when results are round-tripped from TRX to JUnit at the Serializer level.
    /// </summary>
    /// <remarks>
    ///     Proves that the Serializer unit correctly dispatches to TrxSerializer and JUnitSerializer
    ///     so that test count, names, class names, and outcomes survive a serialize-deserialize-
    ///     serialize-deserialize chain through both formats. This exercises
    ///     <c>TestResults-Serializer-TrxDeserialization</c>, <c>TestResults-Serializer-JUnitDeserialization</c>,
    ///     and <c>TestResults-IO-RoundTrip</c> at the unit level.
    /// </remarks>
    [Fact]
    public void Serializer_RoundTrip_TrxToJUnit_PreservesCoreTestData()
    {
        // Arrange: create test results with Passed, Failed, and NotExecuted outcomes
        var original = new TestResults
        {
            Name = "UnitRoundTripRun",
            Results =
            [
                new TestResult
                {
                    Name = "PassedTest",
                    ClassName = "Unit.PassedClass",
                    Outcome = TestOutcome.Passed,
                    Duration = TimeSpan.FromSeconds(1.0)
                },
                new TestResult
                {
                    Name = "FailedTest",
                    ClassName = "Unit.FailedClass",
                    Outcome = TestOutcome.Failed,
                    Duration = TimeSpan.FromSeconds(0.5),
                    ErrorMessage = "Assertion failed"
                },
                new TestResult
                {
                    Name = "SkippedTest",
                    ClassName = "Unit.SkippedClass",
                    Outcome = TestOutcome.NotExecuted,
                    Duration = TimeSpan.Zero
                }
            ]
        };

        // Act: serialize to TRX via Serializer, deserialize, then serialize to JUnit via Serializer, deserialize again
        var trxContent = TrxSerializer.Serialize(original);
        var fromTrx = Serializer.Deserialize(trxContent);
        var junitContent = JUnitSerializer.Serialize(fromTrx);
        var fromJUnit = Serializer.Deserialize(junitContent);

        // Assert: core structure is preserved through the unit-level conversion chain
        Assert.NotNull(fromJUnit);
        Assert.Equal(3, fromJUnit.Results.Count);

        // Assert: PassedTest core data is preserved
        var passed = fromJUnit.Results.First(r => r.Name == "PassedTest");
        Assert.Equal("Unit.PassedClass", passed.ClassName);
        Assert.Equal(TestOutcome.Passed, passed.Outcome);

        // Assert: FailedTest core data is preserved
        var failed = fromJUnit.Results.First(r => r.Name == "FailedTest");
        Assert.Equal("Unit.FailedClass", failed.ClassName);
        Assert.Equal(TestOutcome.Failed, failed.Outcome);

        // Assert: SkippedTest core data is preserved
        var skipped = fromJUnit.Results.First(r => r.Name == "SkippedTest");
        Assert.Equal("Unit.SkippedClass", skipped.ClassName);
        Assert.Equal(TestOutcome.NotExecuted, skipped.Outcome);
    }
}
