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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for Serializer class
/// </summary>
[TestClass]
public sealed class SerializerTests
{
    /// <summary>
    ///     Test that Identify correctly identifies TRX format
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_TrxContent_ReturnsTrx()
    {
        // Create a sample TRX content
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

        // Identify format
        var format = Serializer.Identify(trxContent);

        // Verify it's identified as TRX
        Assert.AreEqual(TestResultFormat.Trx, format);
    }

    /// <summary>
    ///     Test that Identify correctly identifies JUnit format with testsuites root
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_JUnitTestsuitesContent_ReturnsJUnit()
    {
        // Create a sample JUnit content with testsuites root
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuites name="Test Suite">
              <testsuite name="TestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="TestClass" time="1.500" />
              </testsuite>
            </testsuites>
            """;

        // Identify format
        var format = Serializer.Identify(junitContent);

        // Verify it's identified as JUnit
        Assert.AreEqual(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that Identify correctly identifies JUnit format with testsuite root
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_JUnitTestsuiteContent_ReturnsJUnit()
    {
        // Create a sample JUnit content with testsuite root
        var junitContent = """
            <?xml version="1.0" encoding="UTF-8"?>
            <testsuite name="TestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
              <testcase name="Test1" classname="TestClass" time="1.500" />
            </testsuite>
            """;

        // Identify format
        var format = Serializer.Identify(junitContent);

        // Verify it's identified as JUnit
        Assert.AreEqual(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for empty content
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_EmptyContent_ReturnsUnknown()
    {
        // Identify format of empty string
        var format = Serializer.Identify(string.Empty);

        // Verify it's identified as Unknown
        Assert.AreEqual(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for null content
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_NullContent_ReturnsUnknown()
    {
        // Identify format of null string
        var format = Serializer.Identify(null!);

        // Verify it's identified as Unknown
        Assert.AreEqual(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for whitespace content
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_WhitespaceContent_ReturnsUnknown()
    {
        // Identify format of whitespace string
        var format = Serializer.Identify("   \n\t  ");

        // Verify it's identified as Unknown
        Assert.AreEqual(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for invalid XML
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_InvalidXml_ReturnsUnknown()
    {
        // Create invalid XML content
        var invalidXml = "<invalid><unclosed>";

        // Identify format
        var format = Serializer.Identify(invalidXml);

        // Verify it's identified as Unknown
        Assert.AreEqual(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Identify returns Unknown for unrecognized XML format
    /// </summary>
    [TestMethod]
    public void Serializer_Identify_UnrecognizedXmlFormat_ReturnsUnknown()
    {
        // Create XML with unrecognized root element
        var unrecognizedXml = """
            <?xml version="1.0"?>
            <UnknownRoot>
              <SomeData>value</SomeData>
            </UnknownRoot>
            """;

        // Identify format
        var format = Serializer.Identify(unrecognizedXml);

        // Verify it's identified as Unknown
        Assert.AreEqual(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Deserialize successfully deserializes TRX content
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_TrxContent_ReturnsTestResults()
    {
        // Create a sample TRX content
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

        // Deserialize
        var results = Serializer.Deserialize(trxContent);

        // Verify results
        Assert.IsNotNull(results);
        Assert.AreEqual("Test Run", results.Name);
        Assert.AreEqual("User", results.UserName);
        Assert.HasCount(1, results.Results);
        Assert.AreEqual("Test1", results.Results[0].Name);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that Deserialize successfully deserializes JUnit content
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_JUnitContent_ReturnsTestResults()
    {
        // Create a sample JUnit content
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

        // Deserialize
        var results = Serializer.Deserialize(junitContent);

        // Verify results
        Assert.IsNotNull(results);
        Assert.AreEqual("Test Suite", results.Name);
        Assert.HasCount(2, results.Results);
        Assert.AreEqual("Test1", results.Results[0].Name);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.AreEqual("Test2", results.Results[1].Name);
        Assert.AreEqual(TestOutcome.Failed, results.Results[1].Outcome);
        Assert.AreEqual("Test failed", results.Results[1].ErrorMessage);
    }

    /// <summary>
    ///     Test that Deserialize can handle real TRX example file
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_RealTrxExample_ReturnsTestResults()
    {
        // Load example TRX file
        var trxContent = TestHelpers.GetEmbeddedResource(
            "DemaConsulting.TestResults.Tests.IO.Examples.example1.trx");

        // Identify and verify format
        var format = Serializer.Identify(trxContent);
        Assert.AreEqual(TestResultFormat.Trx, format);

        // Deserialize
        var results = Serializer.Deserialize(trxContent);

        // Verify results
        Assert.IsNotNull(results);
        Assert.AreEqual("Sample TRX Import", results.Name);
        Assert.AreEqual("Brian Mancini", results.UserName);
        Assert.IsNotEmpty(results.Results);
    }

    /// <summary>
    ///     Test that Deserialize handles multiple test outcomes from TRX
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_TrxWithMultipleOutcomes_ParsesCorrectly()
    {
        // Create TRX content with different outcomes
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

        // Deserialize
        var results = Serializer.Deserialize(trxContent);

        // Verify results
        Assert.IsNotNull(results);
        Assert.HasCount(2, results.Results);

        var passedTest = results.Results[0];
        Assert.AreEqual("PassedTest", passedTest.Name);
        Assert.AreEqual(TestOutcome.Passed, passedTest.Outcome);

        var failedTest = results.Results[1];
        Assert.AreEqual("FailedTest", failedTest.Name);
        Assert.AreEqual(TestOutcome.Failed, failedTest.Outcome);
        Assert.AreEqual("Test failed", failedTest.ErrorMessage);
    }

    /// <summary>
    ///     Test that Deserialize handles JUnit with system output and error
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_JUnitWithSystemOutput_ParsesCorrectly()
    {
        // Create JUnit content with system output
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

        // Deserialize
        var results = Serializer.Deserialize(junitContent);

        // Verify results
        Assert.IsNotNull(results);
        Assert.HasCount(1, results.Results);
        Assert.AreEqual("Test1", results.Results[0].Name);
        Assert.AreEqual("Standard output", results.Results[0].SystemOutput);
        Assert.AreEqual("Standard error", results.Results[0].SystemError);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for null input
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_NullContents_ThrowsArgumentException()
    {
        // Arrange - null contents
        string? nullContents = null;

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Serializer.Deserialize(nullContents!));
        Assert.AreEqual("contents", ex.ParamName);
        Assert.Contains("cannot be null or whitespace", ex.Message);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange - empty string
        var emptyContents = string.Empty;

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Serializer.Deserialize(emptyContents));
        Assert.AreEqual("contents", ex.ParamName);
        Assert.Contains("cannot be null or whitespace", ex.Message);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange - whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => Serializer.Deserialize(whitespaceContents));
        Assert.AreEqual("contents", ex.ParamName);
        Assert.Contains("cannot be null or whitespace", ex.Message);
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException for unknown format
    /// </summary>
    [TestMethod]
    public void Serializer_Deserialize_UnknownFormat_ThrowsInvalidOperationException()
    {
        // Arrange - valid XML but unknown format
        var unknownFormatXml = """
            <?xml version="1.0"?>
            <UnknownRoot>
              <SomeData>value</SomeData>
            </UnknownRoot>
            """;

        // Act & Assert
        var ex = Assert.ThrowsExactly<InvalidOperationException>(() => Serializer.Deserialize(unknownFormatXml));
        Assert.Contains("Unable to identify test result format", ex.Message);
    }
}
