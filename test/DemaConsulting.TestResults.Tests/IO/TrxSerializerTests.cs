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

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using DemaConsulting.TestResults.IO;
using Xunit;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for TrxSerializer class
/// </summary>
public sealed class TrxSerializerTests
{
#pragma warning disable S1075 // URIs should not be hardcoded - This is an XML namespace URI, not a file path
    /// <summary>
    ///     TRX namespace URI
    /// </summary>
    private const string TrxNamespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
#pragma warning restore S1075

    /// <summary>
    ///     Test for basic serialization
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer produces a valid TRX document from a single-test
    ///     <see cref="TestResults"/> object, including correct <c>UnitTestResult</c>,
    ///     <c>UnitTest</c>, and <c>Counters</c> elements.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_BasicTestResults_ProducesValidTrxXml()
    {
        // Arrange: construct a basic test results object
        var suites = new TestResults
        {
            Name = "Basic",
            UserName = "user",
            Results =
            [
                new TestResult
                {
                    Name = "Test",
                    ClassName = "Class",
                    CodeBase = "Code",
                    StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0),
                    SystemOutput = "Output",
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Act: serialize the test suites object
        var result = TrxSerializer.Serialize(suites);

        // Assert: verify the serialized result is present
        Assert.NotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the UnitTestResult element is present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test']", nsMgr));

        // Assert: verify the UnitTest element is present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test']", nsMgr));

        // Assert: verify the Counters element is present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='0']", nsMgr));
    }

    /// <summary>
    ///     Test for complex serialization
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer correctly serializes multiple test results, producing one
    ///     <c>UnitTestResult</c> and one <c>UnitTest</c> element per result and setting summary
    ///     counters to reflect the mixed pass/fail outcomes.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_MultipleTestResults_ProducesValidTrxXml()
    {
        // Arrange: construct a complex test results object
        var suites = new TestResults
        {
            Name = "Basic",
            UserName = "user",
            Results =
            [
                new TestResult
                {
                    Name = "Test1",
                    ClassName = "Class",
                    CodeBase = "Code",
                    StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0),
                    SystemOutput = "Output",
                    Outcome = TestOutcome.Passed
                },
                new TestResult
                {
                    Name = "Test2",
                    ClassName = "Class",
                    CodeBase = "Code",
                    StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0),
                    SystemError = "Output",
                    Outcome = TestOutcome.Failed,
                    ErrorMessage = "Error"
                }
            ]
        };

        // Act: serialize the test suites object
        var result = TrxSerializer.Serialize(suites);

        // Assert: verify the serialized result is present
        Assert.NotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the UnitTestResult elements are present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test2']", nsMgr));

        // Assert: verify the UnitTest elements are present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test2']", nsMgr));

        // Assert: verify the Counters element is present
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='2']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='2']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.NotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='1']", nsMgr));
    }

    /// <summary>
    ///     Test for basic deserialization
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer correctly parses a minimal well-formed TRX document and
    ///     populates all key <see cref="TestResult"/> fields including identifiers, timing, output,
    ///     and outcome.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_BasicTrxXml_ReturnsTestResults()
    {
        // Act: deserialize the test results object
        var results = TrxSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun id="0ef15ada-c28f-4755-8d4c-5b68d1f9dda6" name="Basic" runUser="user" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult executionId="735286a7-f9ed-404f-8871-300f9266eac9" testId="ec83398d-3b21-4dc4-b55c-c0ee2e81c074" testName="Test" computerName="Machine" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" outcome="Passed" duration="00:00:01" startTime="2025-02-18T03:00:00Z" endTime="2025-02-18T03:00:01Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output>
                    <StdOut><![CDATA[Output]]></StdOut>
                  </Output>
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="Test" id="ec83398d-3b21-4dc4-b55c-c0ee2e81c074">
                  <Execution id="735286a7-f9ed-404f-8871-300f9266eac9" />
                  <TestMethod codeBase="Code" className="Class" name="Test" />
                </UnitTest>
              </TestDefinitions>
              <TestEntries>
                <TestEntry testId="ec83398d-3b21-4dc4-b55c-c0ee2e81c074" executionId="735286a7-f9ed-404f-8871-300f9266eac9" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestEntries>
              <TestLists>
                <TestList name="All Loaded Results" id="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestLists>
              <ResultSummary outcome="Completed">
                <Counters total="1" executed="1" passed="1" failed="0" />
              </ResultSummary>
            </TestRun>
            """);
        Assert.NotNull(results);

        // Assert: results information
        Assert.Equal(Guid.Parse("0ef15ada-c28f-4755-8d4c-5b68d1f9dda6"), results.Id);
        Assert.Equal("Basic", results.Name);
        Assert.Equal("user", results.UserName);
        Assert.Single(results.Results);

        // Assert: test result information
        var result = results.Results[0];
        Assert.Equal(Guid.Parse("ec83398d-3b21-4dc4-b55c-c0ee2e81c074"), result.TestId);
        Assert.Equal(Guid.Parse("735286a7-f9ed-404f-8871-300f9266eac9"), result.ExecutionId);
        Assert.Equal("Test", result.Name);
        Assert.Equal("Code", result.CodeBase);
        Assert.Equal("Class", result.ClassName);
        Assert.Equal("Machine", result.ComputerName);
        Assert.Equal(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result.StartTime);
        Assert.Equal(1.0, result.Duration.TotalSeconds);
        Assert.Equal("Output", result.SystemOutput);
        Assert.Equal(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test for complex deserialization
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer correctly parses a TRX document with multiple test results,
    ///     resolves each result against its <c>UnitTest</c> definition, and populates all key
    ///     fields including identifiers, timing, and outcome for both passed and failed tests.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_ComplexTrxXml_ReturnsTestResults()
    {
        // Act: deserialize the test results object
        var results = TrxSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun id="0704cd18-88b1-43f7-868e-ad02bfda887d" name="Basic" runUser="user" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult executionId="cebd9f31-adec-4a7d-862b-598c52f1b9cf" testId="57debb4d-6784-482d-93d0-75dca3d3f556" testName="Test1" computerName="Machine" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" outcome="Passed" duration="00:00:01" startTime="2025-02-18T03:00:00Z" endTime="2025-02-18T03:00:01Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output>
                    <StdOut><![CDATA[Output]]></StdOut>
                  </Output>
                </UnitTestResult>
                <UnitTestResult executionId="ceb08c73-796f-4924-ad35-098a3fbb802b" testId="eb73087a-1def-4776-ba71-c56cd5a1bb1c" testName="Test2" computerName="Machine" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" outcome="Failed" duration="00:00:02" startTime="2025-02-18T03:00:00Z" endTime="2025-02-18T03:00:02Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output>
                    <StdOut><![CDATA[]]></StdOut>
                    <ErrorInfo>
                      <Message><![CDATA[Error]]></Message>
                      <StackTrace><![CDATA[]]></StackTrace>
                    </ErrorInfo>
                  </Output>
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="57debb4d-6784-482d-93d0-75dca3d3f556">
                  <Execution id="cebd9f31-adec-4a7d-862b-598c52f1b9cf" />
                  <TestMethod codeBase="Code" className="Class" name="Test1" />
                </UnitTest>
                <UnitTest name="Test2" id="eb73087a-1def-4776-ba71-c56cd5a1bb1c">
                  <Execution id="ceb08c73-796f-4924-ad35-098a3fbb802b" />
                  <TestMethod codeBase="Code" className="Class" name="Test2" />
                </UnitTest>
              </TestDefinitions>
              <TestEntries>
                <TestEntry testId="57debb4d-6784-482d-93d0-75dca3d3f556" executionId="cebd9f31-adec-4a7d-862b-598c52f1b9cf" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
                <TestEntry testId="eb73087a-1def-4776-ba71-c56cd5a1bb1c" executionId="ceb08c73-796f-4924-ad35-098a3fbb802b" testListId="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestEntries>
              <TestLists>
                <TestList name="All Loaded Results" id="19431567-8539-422a-85D7-44EE4E166BDA" />
              </TestLists>
              <ResultSummary outcome="Completed">
                <Counters total="2" executed="2" passed="1" failed="1" />
              </ResultSummary>
            </TestRun>
            """);
        Assert.NotNull(results);

        // Assert: results information
        Assert.Equal(Guid.Parse("0704cd18-88b1-43f7-868e-ad02bfda887d"), results.Id);
        Assert.Equal("Basic", results.Name);
        Assert.Equal("user", results.UserName);
        Assert.Equal(2, results.Results.Count);

        // Assert: test1 result information
        var result1 = results.Results[0];
        Assert.Equal(Guid.Parse("57debb4d-6784-482d-93d0-75dca3d3f556"), result1.TestId);
        Assert.Equal(Guid.Parse("cebd9f31-adec-4a7d-862b-598c52f1b9cf"), result1.ExecutionId);
        Assert.Equal("Test1", result1.Name);
        Assert.Equal("Code", result1.CodeBase);
        Assert.Equal("Class", result1.ClassName);
        Assert.Equal("Machine", result1.ComputerName);
        Assert.Equal(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result1.StartTime);
        Assert.Equal(1.0, result1.Duration.TotalSeconds);
        Assert.Equal("Output", result1.SystemOutput);
        Assert.Equal(TestOutcome.Passed, result1.Outcome);

        // Assert: test2 result information
        var result2 = results.Results[1];
        Assert.Equal(Guid.Parse("eb73087a-1def-4776-ba71-c56cd5a1bb1c"), result2.TestId);
        Assert.Equal(Guid.Parse("ceb08c73-796f-4924-ad35-098a3fbb802b"), result2.ExecutionId);
        Assert.Equal("Test2", result2.Name);
        Assert.Equal("Code", result2.CodeBase);
        Assert.Equal("Class", result2.ClassName);
        Assert.Equal("Machine", result2.ComputerName);
        Assert.Equal(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result2.StartTime);
        Assert.Equal(2.0, result2.Duration.TotalSeconds);
        Assert.Equal(TestOutcome.Failed, result2.Outcome);
        Assert.Equal("Error", result2.ErrorMessage);
    }

    /// <summary>
    ///     Test for serialization with stack trace but no error message
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer emits a <c>StackTrace</c> element inside <c>ErrorInfo</c>
    ///     even when no error message is present, ensuring that stack traces are preserved
    ///     regardless of whether an associated message is set.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_StackTraceWithoutMessage_IncludesStackTraceElement()
    {
        // Arrange: construct a test results object with stack trace but no message
        var suites = new TestResults
        {
            Name = "StackTraceOnly",
            UserName = "user",
            Results =
            [
                new TestResult
                {
                    Name = "TestWithStackTrace",
                    ClassName = "Class",
                    CodeBase = "Code",
                    StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Failed,
                    ErrorStackTrace = "at TestClass.Method() in Test.cs:line 42"
                }
            ]
        };

        // Act: serialize the test suites object
        var result = TrxSerializer.Serialize(suites);

        // Assert: verify the serialized result is present
        Assert.NotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the StackTrace element is present
        var stackTraceElement = doc.XPathSelectElement(
            "/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='TestWithStackTrace']/trx:Output/trx:ErrorInfo/trx:StackTrace",
            nsMgr);
        Assert.NotNull(stackTraceElement);
        Assert.Contains("at TestClass.Method() in Test.cs:line 42", stackTraceElement.Value);
    }

    /// <summary>
    ///     Test that Serialize throws ArgumentNullException for null input
    /// </summary>
    /// <remarks>
    ///     Proves that null input is rejected at the entry point before any serialization
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_NullResults_ThrowsArgumentNullException()
    {
        // Arrange: null test results
        TestResults? nullResults = null;

        // Act: attempt to serialize null test results (combined with Assert)
        var ex = Assert.Throws<ArgumentNullException>(() => TrxSerializer.Serialize(nullResults!));
        Assert.Equal("results", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentNullException for null input
    /// </summary>
    /// <remarks>
    ///     Proves that null input is rejected at the entry point before any XML parsing
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_NullContents_ThrowsArgumentNullException()
    {
        // Arrange: null contents
        string? nullContents = null;

        // Act: attempt to deserialize null contents (combined with Assert)
        var ex = Assert.Throws<ArgumentNullException>(() => TrxSerializer.Deserialize(nullContents!));
        Assert.Equal("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    /// <remarks>
    ///     Proves that empty string input is rejected at the entry point before any XML parsing
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange: empty string
        var emptyContents = string.Empty;

        // Act: attempt to deserialize empty string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => TrxSerializer.Deserialize(emptyContents));
        Assert.Equal("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    /// <remarks>
    ///     Proves that whitespace-only input is rejected at the entry point before any XML parsing
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange: whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act: attempt to deserialize whitespace string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => TrxSerializer.Deserialize(whitespaceContents));
        Assert.Equal("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that serialization emits the storage attribute on UnitTest matching CodeBase
    /// </summary>
    /// <remarks>
    ///     Tests that TrxSerializer emits a storage attribute on the UnitTest element and that
    ///     its value matches the TestResult.CodeBase property, consistent with the de-facto TRX
    ///     schema where both UnitTest/@storage and TestMethod/@codeBase reference the test assembly.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_WithCodeBase_EmitsStorageAttributeOnUnitTest()
    {
        // Arrange: test results with a specific CodeBase
        var suites = new TestResults
        {
            Name = "StorageTest",
            UserName = "user",
            Results =
            [
                new TestResult
                {
                    Name = "TestWithCodeBase",
                    ClassName = "MyClass",
                    CodeBase = "path/to/MyAssembly.dll",
                    StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Act: Serialize the test results
        var result = TrxSerializer.Serialize(suites);
        Assert.NotNull(result);

        // Assert: Parse and verify the storage attribute on the UnitTest element
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        var unitTest = doc.XPathSelectElement(
            "/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='TestWithCodeBase']", nsMgr);
        Assert.NotNull(unitTest);
        Assert.Equal("path/to/MyAssembly.dll", unitTest.Attribute("storage")?.Value);

        // Also verify TestMethod/@codeBase matches
        var testMethod = unitTest.XPathSelectElement("trx:TestMethod", nsMgr);
        Assert.NotNull(testMethod);
        Assert.Equal("path/to/MyAssembly.dll", testMethod.Attribute("codeBase")?.Value);
    }

    /// <summary>
    ///     Tests that serializing TestResults to TRX and deserializing back preserves all key data.
    /// </summary>
    /// <remarks>
    ///     Proves that serializing TestResults to TRX and deserializing back preserves all key data
    ///     including run metadata, test properties, outcomes, timing, and output/error information.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_ThenDeserialize_PreservesTestData()
    {
        // Arrange: create test results with multiple outcomes and rich data
        var startTime = new DateTime(2025, 3, 10, 8, 0, 0, DateTimeKind.Utc);
        var original = new TestResults
        {
            Name = "RoundTripRun",
            UserName = "round.trip.user",
            Results =
            [
                new TestResult
                {
                    Name = "PassedTest",
                    ClassName = "Suite.PassedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.Passed,
                    StartTime = startTime,
                    Duration = TimeSpan.FromSeconds(1.5),
                    SystemOutput = "All good",
                    SystemError = string.Empty,
                    ErrorMessage = string.Empty,
                    ErrorStackTrace = string.Empty
                },
                new TestResult
                {
                    Name = "FailedTest",
                    ClassName = "Suite.FailedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.Failed,
                    StartTime = startTime.AddSeconds(2),
                    Duration = TimeSpan.FromSeconds(0.75),
                    SystemOutput = string.Empty,
                    SystemError = "err output",
                    ErrorMessage = "Expected 1 but was 2",
                    ErrorStackTrace = "at Suite.FailedClass.FailedTest() line 42"
                },
                new TestResult
                {
                    Name = "ErrorTest",
                    ClassName = "Suite.ErrorClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.Error,
                    StartTime = startTime.AddSeconds(4),
                    Duration = TimeSpan.FromSeconds(0.25),
                    SystemOutput = string.Empty,
                    SystemError = string.Empty,
                    ErrorMessage = "NullReferenceException",
                    ErrorStackTrace = "at Suite.ErrorClass.ErrorTest() line 17"
                },
                new TestResult
                {
                    Name = "SkippedTest",
                    ClassName = "Suite.SkippedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.NotExecuted,
                    StartTime = startTime.AddSeconds(6),
                    Duration = TimeSpan.Zero,
                    SystemOutput = string.Empty,
                    SystemError = string.Empty,
                    ErrorMessage = string.Empty,
                    ErrorStackTrace = string.Empty
                }
            ]
        };

        // Act: serialize to TRX and then deserialize back
        var trxContent = TrxSerializer.Serialize(original);
        var deserialized = TrxSerializer.Deserialize(trxContent);

        // Assert: run metadata is preserved
        Assert.NotNull(deserialized);
        Assert.Equal(original.Name, deserialized.Name);
        Assert.Equal(original.UserName, deserialized.UserName);
        Assert.Equal(4, deserialized.Results.Count);

        // Assert: PassedTest properties are preserved
        var passed = deserialized.Results[0];
        Assert.Equal("PassedTest", passed.Name);
        Assert.Equal("Suite.PassedClass", passed.ClassName);
        Assert.Equal("path/to/TestAssembly.dll", passed.CodeBase);
        Assert.Equal("BuildAgent01", passed.ComputerName);
        Assert.Equal(TestOutcome.Passed, passed.Outcome);
        Assert.Equal(startTime, passed.StartTime);
        Assert.True(Math.Abs((passed.Duration - TimeSpan.FromSeconds(1.5)).TotalSeconds) < 0.001);
        Assert.Equal("All good", passed.SystemOutput);
        Assert.Equal(string.Empty, passed.SystemError);
        Assert.Equal(string.Empty, passed.ErrorMessage);
        Assert.Equal(string.Empty, passed.ErrorStackTrace);

        // Assert: FailedTest properties are preserved
        var failed = deserialized.Results[1];
        Assert.Equal("FailedTest", failed.Name);
        Assert.Equal("Suite.FailedClass", failed.ClassName);
        Assert.Equal("path/to/TestAssembly.dll", failed.CodeBase);
        Assert.Equal("BuildAgent01", failed.ComputerName);
        Assert.Equal(TestOutcome.Failed, failed.Outcome);
        Assert.Equal(startTime.AddSeconds(2), failed.StartTime);
        Assert.True(Math.Abs((failed.Duration - TimeSpan.FromSeconds(0.75)).TotalSeconds) < 0.001);
        Assert.Equal(string.Empty, failed.SystemOutput);
        Assert.Equal("err output", failed.SystemError);
        Assert.Equal("Expected 1 but was 2", failed.ErrorMessage);
        Assert.Equal("at Suite.FailedClass.FailedTest() line 42", failed.ErrorStackTrace);

        // Assert: ErrorTest properties are preserved
        var error = deserialized.Results[2];
        Assert.Equal("ErrorTest", error.Name);
        Assert.Equal("Suite.ErrorClass", error.ClassName);
        Assert.Equal(TestOutcome.Error, error.Outcome);
        Assert.Equal(startTime.AddSeconds(4), error.StartTime);
        Assert.True(Math.Abs((error.Duration - TimeSpan.FromSeconds(0.25)).TotalSeconds) < 0.001);
        Assert.Equal("NullReferenceException", error.ErrorMessage);
        Assert.Equal("at Suite.ErrorClass.ErrorTest() line 17", error.ErrorStackTrace);

        // Assert: SkippedTest properties are preserved
        var skipped = deserialized.Results[3];
        Assert.Equal("SkippedTest", skipped.Name);
        Assert.Equal("Suite.SkippedClass", skipped.ClassName);
        Assert.Equal(TestOutcome.NotExecuted, skipped.Outcome);
        Assert.Equal(startTime.AddSeconds(6), skipped.StartTime);
        Assert.True(Math.Abs(skipped.Duration.TotalSeconds) < 0.001);
    }

    /// <summary>
    ///     Test that TrxSerializer serializes to a string with an XML declaration declaring UTF-8 encoding
    /// </summary>
    /// <remarks>
    ///     Proves that the Utf8StringWriter helper used internally by TrxSerializer produces output
    ///     with an explicit <c>encoding="utf-8"</c> XML declaration, ensuring consuming tools
    ///     interpret the content correctly.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding()
    {
        // Arrange: minimal test results
        var results = new TestResults
        {
            Name = "EncodingTest",
            Results =
            [
                new TestResult
                {
                    Name = "Test",
                    ClassName = "Class",
                    Duration = TimeSpan.Zero,
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Act: serialize to string
        var xml = TrxSerializer.Serialize(results);

        // Assert: XML declaration with UTF-8 encoding is present
        Assert.True(
            xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"", StringComparison.OrdinalIgnoreCase),
            "Expected XML declaration with encoding=\"utf-8\" at start of output");
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException for TRX with duplicate UnitTest IDs
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer detects and rejects TRX documents that contain two
    ///     <c>UnitTest</c> elements sharing the same <c>id</c> attribute, which would make
    ///     test-method resolution ambiguous.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_DuplicateUnitTestId_ThrowsInvalidOperationException()
    {
        // Arrange: TRX with two UnitTest elements sharing the same id
        var trxWithDuplicateIds =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="aaaaaaaa-0000-0000-0000-000000000001" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="Passed" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
                <UnitTest name="Test1Dup" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1Dup" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: attempt to deserialize the TRX with duplicate IDs (combined with Assert)
        Assert.Throws<InvalidOperationException>(() => TrxSerializer.Deserialize(trxWithDuplicateIds));
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException when a UnitTestResult references a non-existent testId
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer detects and rejects TRX documents where a
    ///     <c>UnitTestResult/@testId</c> references a <c>UnitTest/@id</c> that does not exist
    ///     in the <c>TestDefinitions</c> section.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_NonExistentTestId_ThrowsInvalidOperationException()
    {
        // Arrange: TRX where the UnitTestResult/@testId has no matching UnitTest/@id
        var trxWithMissingTestId =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="aaaaaaaa-0000-0000-0000-000000000099" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="Passed" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: attempt to deserialize the TRX with non-existent testId (combined with Assert)
        Assert.Throws<InvalidOperationException>(() => TrxSerializer.Deserialize(trxWithMissingTestId));
    }

    /// <summary>
    ///     Test that a malformed GUID in testId falls back to a newly generated GUID
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer tolerates non-GUID values in the <c>testId</c> attribute
    ///     by substituting a newly generated GUID, allowing deserialization to succeed even when
    ///     the TRX contains non-standard identifier values.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_MalformedGuid_FallsBackToNewGuid()
    {
        // Arrange: TRX XML with a malformed (non-GUID) testId value; the UnitTest/@id uses the same
        // non-GUID string so the test-method lookup resolves successfully and only the GUID parse fails
        var trx =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="NOT-A-GUID" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="Passed" duration="00:00:01" startTime="2025-01-01T00:00:00Z" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="NOT-A-GUID">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: deserialize the TRX with a malformed testId
        var results = TrxSerializer.Deserialize(trx);

        // Assert: the resulting TestId is a valid non-empty GUID generated as a fallback
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.NotEqual(Guid.Empty, results.Results[0].TestId);
    }

    /// <summary>
    ///     Test that a malformed duration string falls back to TimeSpan.Zero
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer tolerates malformed duration values by substituting
    ///     <see cref="TimeSpan.Zero"/>, allowing deserialization to succeed even when a
    ///     <c>duration</c> attribute does not conform to the expected format.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_MalformedDuration_FallsBackToZero()
    {
        // Arrange: TRX XML with a malformed duration value
        var trx =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="aaaaaaaa-0000-0000-0000-000000000001" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="Passed" duration="INVALID-DURATION" startTime="2025-01-01T00:00:00Z" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: deserialize the TRX with a malformed duration
        var results = TrxSerializer.Deserialize(trx);

        // Assert: the resulting Duration falls back to TimeSpan.Zero
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal(TimeSpan.Zero, results.Results[0].Duration);
    }

    /// <summary>
    ///     Test that a malformed timestamp string falls back to a DateTime close to UtcNow
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer tolerates malformed <c>startTime</c> attribute values by
    ///     substituting <see cref="DateTime.UtcNow"/> at parse time, allowing deserialization to
    ///     succeed even when a timestamp does not conform to the expected ISO 8601 format.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_MalformedTimestamp_FallsBackToUtcNow()
    {
        // Arrange: TRX XML with a malformed startTime value
        var trx =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="aaaaaaaa-0000-0000-0000-000000000001" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="Passed" duration="00:00:01" startTime="NOT-A-DATE" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: deserialize the TRX with a malformed timestamp; capture surrounding wall-clock time
        var before = DateTime.UtcNow;
        var results = TrxSerializer.Deserialize(trx);
        var after = DateTime.UtcNow;

        // Assert: the resulting StartTime falls back to DateTime.UtcNow at parse time (within tolerance)
        Assert.NotNull(results);
        Assert.Single(results.Results);
        var startTime = results.Results[0].StartTime;
        Assert.True(startTime >= before && startTime <= after, $"Expected StartTime between {before} and {after}, was {startTime}");
    }

    /// <summary>
    ///     Test that an unrecognized outcome string falls back to TestOutcome.Failed
    /// </summary>
    /// <remarks>
    ///     Proves that TrxSerializer tolerates unrecognized outcome string values by falling back
    ///     to <see cref="TestOutcome.Failed"/>, ensuring that all <c>UnitTestResult</c> elements
    ///     are deserialized rather than causing an exception.
    /// </remarks>
    [Fact]
    public void TrxSerializer_Deserialize_UnrecognizedOutcome_FallsBackToFailed()
    {
        // Arrange: TRX XML with an outcome value that is not a valid TestOutcome member
        var trx =
            """
            <?xml version="1.0" encoding="utf-8"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
              <Results>
                <UnitTestResult testId="aaaaaaaa-0000-0000-0000-000000000001" executionId="aaaaaaaa-0000-0000-0000-000000000010" testName="Test1" outcome="UnknownOutcome" duration="00:00:01" startTime="2025-01-01T00:00:00Z" />
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="aaaaaaaa-0000-0000-0000-000000000001">
                  <TestMethod className="MyClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: deserialize the TRX with an unrecognized outcome string
        var results = TrxSerializer.Deserialize(trx);

        // Assert: the resulting Outcome falls back to TestOutcome.Failed
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal(TestOutcome.Failed, results.Results[0].Outcome);
    }
}
