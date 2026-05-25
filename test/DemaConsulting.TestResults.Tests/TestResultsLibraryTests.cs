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

namespace DemaConsulting.TestResults.Tests;

/// <summary>
///     System-level tests for the TestResults Library.
/// </summary>
/// <remarks>
///     These tests verify the end-to-end system behavior of the TestResults Library
///     from the perspective of a library consumer. They prove the three system-level
///     architectural requirements:
///     <list type="bullet">
///         <item>The library provides a format-agnostic in-memory model.</item>
///         <item>The library deserializes test result files into a navigable in-memory collection.</item>
///         <item>The library serializes an in-memory collection into test result files.</item>
///     </list>
/// </remarks>
public sealed class TestResultsLibraryTests
{
    /// <summary>
    ///     Test that the TestResults Library provides a format-agnostic in-memory model.
    /// </summary>
    /// <remarks>
    ///     Tests that the in-memory model (TestResults, TestResult, TestOutcome) can represent
    ///     test results independently of any file format. Proves that the model is format-agnostic
    ///     and can be constructed, populated, and navigated without any serialization involved.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_FormatAgnosticModel_CanRepresentTestResults()
    {
        // Arrange: create an in-memory model without involving any file format
        var results = new TestResults
        {
            Name = "MySuite",
            Results =
            [
                new TestResult { Name = "Test1", Outcome = TestOutcome.Passed },
                new TestResult { Name = "Test2", Outcome = TestOutcome.Failed }
            ]
        };

        // Act: navigate the model (combined with Assert)
        Assert.Equal("MySuite", results.Name);
        Assert.Equal(2, results.Results.Count);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.Equal("Test2", results.Results[1].Name);
        Assert.Equal(TestOutcome.Failed, results.Results[1].Outcome);
    }

    /// <summary>
    ///     Test that the TestResults Library deserializes TRX content into a navigable in-memory model.
    /// </summary>
    /// <remarks>
    ///     Tests the end-to-end flow from TRX file content to the in-memory model. Proves that a
    ///     library consumer can read a TRX file and navigate the resulting model.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Deserialize_TrxContent_ReturnsNavigableModel()
    {
        // Arrange: TRX content representing one passing test
        const string content = """
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
            </TestRun>
            """;

        // Act: deserialize the TRX content into the in-memory model
        var results = Serializer.Deserialize(content);

        // Assert: the model is navigable and contains the expected data
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that the TestResults Library deserializes JUnit content into a navigable in-memory model.
    /// </summary>
    /// <remarks>
    ///     Tests the end-to-end flow from JUnit XML file content to the in-memory model. Proves that a
    ///     library consumer can read a JUnit file and navigate the resulting model.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Deserialize_JUnitContent_ReturnsNavigableModel()
    {
        // Arrange: JUnit content representing one passing test
        const string content = """
            <?xml version="1.0"?>
            <testsuites name="MySuite">
              <testsuite name="MyClass" tests="1" failures="0" errors="0">
                <testcase name="Test1" classname="MyClass" time="1.0" />
              </testsuite>
            </testsuites>
            """;

        // Act: deserialize the JUnit content into the in-memory model
        var results = Serializer.Deserialize(content);

        // Assert: the model is navigable and contains the expected data
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that the TestResults Library serializes an in-memory model to TRX content.
    /// </summary>
    /// <remarks>
    ///     Tests the end-to-end flow from an in-memory model to TRX file content. Proves that a
    ///     library consumer can write an in-memory model to a TRX file.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Serialize_InMemoryModel_ProducesTrxContent()
    {
        // Arrange: create an in-memory model with one passing test
        var results = new TestResults
        {
            Name = "MySuite",
            Results =
            [
                new TestResult { Name = "Test1", Outcome = TestOutcome.Passed }
            ]
        };

        // Act: serialize the in-memory model to TRX content
        var content = TrxSerializer.Serialize(results);

        // Assert: the content is valid TRX XML containing the expected data
        Assert.NotNull(content);
        Assert.Contains("TestRun", content);
        Assert.Contains("Test1", content);
    }

    /// <summary>
    ///     Test that the TestResults Library serializes an in-memory model to JUnit content.
    /// </summary>
    /// <remarks>
    ///     Tests the end-to-end flow from an in-memory model to JUnit XML file content. Proves that a
    ///     library consumer can write an in-memory model to a JUnit XML file.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Serialize_InMemoryModel_ProducesJUnitContent()
    {
        // Arrange: create an in-memory model with one passing test
        var results = new TestResults
        {
            Name = "MySuite",
            Results =
            [
                new TestResult { Name = "Test1", Outcome = TestOutcome.Passed }
            ]
        };

        // Act: serialize the in-memory model to JUnit content
        var content = JUnitSerializer.Serialize(results);

        // Assert: the content is valid JUnit XML containing the expected data
        Assert.NotNull(content);
        Assert.Contains("testsuites", content);
        Assert.Contains("Test1", content);
    }

    /// <summary>
    ///     Test that the TestResults Library identifies TRX content as TRX format.
    /// </summary>
    /// <remarks>
    ///     Verifies that a library consumer can determine the format of TRX file content
    ///     using Serializer.Identify() without attempting to deserialize it.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Identify_TrxContent_ReturnsTrx()
    {
        // Arrange: TRX content with the expected namespace and root element
        const string content = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" id="da21858f-2c78-442a-8ea6-51fe73762e0e" name="Test Run" runUser="User">
            </TestRun>
            """;

        // Act: identify the format
        var format = Serializer.Identify(content);

        // Assert: the format is identified as TRX
        Assert.Equal(TestResultFormat.Trx, format);
    }

    /// <summary>
    ///     Test that the TestResults Library identifies JUnit content as JUnit format.
    /// </summary>
    /// <remarks>
    ///     Verifies that a library consumer can determine the format of JUnit XML file content
    ///     using Serializer.Identify() without attempting to deserialize it.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Identify_JUnitContent_ReturnsJUnit()
    {
        // Arrange: JUnit content with testsuites root element
        const string content = """
            <?xml version="1.0"?>
            <testsuites name="MySuite">
            </testsuites>
            """;

        // Act: identify the format
        var format = Serializer.Identify(content);

        // Assert: the format is identified as JUnit
        Assert.Equal(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that the TestResults Library identifies unrecognized content as Unknown format.
    /// </summary>
    /// <remarks>
    ///     Verifies that content with an unrecognized XML root element is identified as Unknown
    ///     without throwing an exception, allowing format-aware workflows to handle unknown formats.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Identify_UnknownContent_ReturnsUnknown()
    {
        // Arrange: XML content with an unrecognized root element
        const string content = """
            <?xml version="1.0"?>
            <unknown-format>
            </unknown-format>
            """;

        // Act: identify the format
        var format = Serializer.Identify(content);

        // Assert: the format is identified as Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that Serializer.Deserialize throws ArgumentNullException when content is null.
    /// </summary>
    /// <remarks>
    ///     Verifies the null-rejection contract documented in the External Interfaces section of
    ///     the system design. Null inputs cannot represent any test result format.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Deserialize_NullContent_ThrowsArgumentNullException()
    {
        // Arrange: null content
        string? content = null;

        // Act / Assert: null input throws ArgumentNullException
        Assert.Throws<ArgumentNullException>(() => Serializer.Deserialize(content!));
    }

    /// <summary>
    ///     Test that Serializer.Deserialize throws ArgumentException when content is whitespace.
    /// </summary>
    /// <remarks>
    ///     Verifies the whitespace-rejection contract documented in the External Interfaces section
    ///     of the system design. Whitespace-only content cannot represent a valid test result file.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Deserialize_WhitespaceContent_ThrowsArgumentException()
    {
        // Arrange: whitespace-only content
        const string content = "   ";

        // Act / Assert: whitespace input throws ArgumentException
        Assert.Throws<ArgumentException>(() => Serializer.Deserialize(content));
    }

    /// <summary>
    ///     Test that Serializer.Deserialize throws InvalidOperationException for unrecognized XML.
    /// </summary>
    /// <remarks>
    ///     Verifies the unknown-format-rejection contract documented in the External Interfaces section
    ///     of the system design. XML with an unrecognized root element cannot be deserialized and must
    ///     not silently return an empty result.
    /// </remarks>
    [Fact]
    public void TestResultsLibrary_Deserialize_UnrecognizedXmlContent_ThrowsInvalidOperationException()
    {
        // Arrange: XML with an unrecognized root element
        const string content = """
            <?xml version="1.0"?>
            <unknown-format>
            </unknown-format>
            """;

        // Act / Assert: unrecognized format throws InvalidOperationException
        Assert.Throws<InvalidOperationException>(() => Serializer.Deserialize(content));
    }
}
