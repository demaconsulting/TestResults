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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for TrxSerializer class
/// </summary>
[TestClass]
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
    [TestMethod]
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
        Assert.IsNotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the UnitTestResult element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test']", nsMgr));

        // Assert: verify the UnitTest element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test']", nsMgr));

        // Assert: verify the Counters element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='0']", nsMgr));
    }

    /// <summary>
    ///     Test for complex serialization
    /// </summary>
    [TestMethod]
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
        Assert.IsNotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the UnitTestResult elements are present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test2']", nsMgr));

        // Assert: verify the UnitTest elements are present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test2']", nsMgr));

        // Assert: verify the Counters element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='2']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='2']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='1']", nsMgr));
    }

    /// <summary>
    ///     Test for basic deserialization
    /// </summary>
    [TestMethod]
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
        Assert.IsNotNull(results);

        // Assert: results information
        Assert.AreEqual(Guid.Parse("0ef15ada-c28f-4755-8d4c-5b68d1f9dda6"), results.Id);
        Assert.AreEqual("Basic", results.Name);
        Assert.AreEqual("user", results.UserName);
        Assert.HasCount(1, results.Results);

        // Assert: test result information
        var result = results.Results[0];
        Assert.AreEqual(Guid.Parse("ec83398d-3b21-4dc4-b55c-c0ee2e81c074"), result.TestId);
        Assert.AreEqual(Guid.Parse("735286a7-f9ed-404f-8871-300f9266eac9"), result.ExecutionId);
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual("Code", result.CodeBase);
        Assert.AreEqual("Class", result.ClassName);
        Assert.AreEqual("Machine", result.ComputerName);
        Assert.AreEqual(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result.StartTime);
        Assert.AreEqual(1.0, result.Duration.TotalSeconds);
        Assert.AreEqual("Output", result.SystemOutput);
        Assert.AreEqual(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test for complex deserialization
    /// </summary>
    [TestMethod]
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
        Assert.IsNotNull(results);

        // Assert: results information
        Assert.AreEqual(Guid.Parse("0704cd18-88b1-43f7-868e-ad02bfda887d"), results.Id);
        Assert.AreEqual("Basic", results.Name);
        Assert.AreEqual("user", results.UserName);
        Assert.HasCount(2, results.Results);

        // Assert: test1 result information
        var result1 = results.Results[0];
        Assert.AreEqual(Guid.Parse("57debb4d-6784-482d-93d0-75dca3d3f556"), result1.TestId);
        Assert.AreEqual(Guid.Parse("cebd9f31-adec-4a7d-862b-598c52f1b9cf"), result1.ExecutionId);
        Assert.AreEqual("Test1", result1.Name);
        Assert.AreEqual("Code", result1.CodeBase);
        Assert.AreEqual("Class", result1.ClassName);
        Assert.AreEqual("Machine", result1.ComputerName);
        Assert.AreEqual(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result1.StartTime);
        Assert.AreEqual(1.0, result1.Duration.TotalSeconds);
        Assert.AreEqual("Output", result1.SystemOutput);
        Assert.AreEqual(TestOutcome.Passed, result1.Outcome);

        // Assert: test2 result information
        var result2 = results.Results[1];
        Assert.AreEqual(Guid.Parse("eb73087a-1def-4776-ba71-c56cd5a1bb1c"), result2.TestId);
        Assert.AreEqual(Guid.Parse("ceb08c73-796f-4924-ad35-098a3fbb802b"), result2.ExecutionId);
        Assert.AreEqual("Test2", result2.Name);
        Assert.AreEqual("Code", result2.CodeBase);
        Assert.AreEqual("Class", result2.ClassName);
        Assert.AreEqual("Machine", result2.ComputerName);
        Assert.AreEqual(new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc), result2.StartTime);
        Assert.AreEqual(2.0, result2.Duration.TotalSeconds);
        Assert.AreEqual(TestOutcome.Failed, result2.Outcome);
        Assert.AreEqual("Error", result2.ErrorMessage);
    }

    /// <summary>
    ///     Test for serialization with stack trace but no error message
    /// </summary>
    [TestMethod]
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
        Assert.IsNotNull(result);

        // Assert: parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Assert: verify the StackTrace element is present
        var stackTraceElement = doc.XPathSelectElement(
            "/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='TestWithStackTrace']/trx:Output/trx:ErrorInfo/trx:StackTrace",
            nsMgr);
        Assert.IsNotNull(stackTraceElement);
        Assert.Contains("at TestClass.Method() in Test.cs:line 42", stackTraceElement.Value);
    }

    /// <summary>
    ///     Test that Serialize throws ArgumentNullException for null input
    /// </summary>
    [TestMethod]
    public void TrxSerializer_Serialize_NullResults_ThrowsArgumentNullException()
    {
        // Arrange: null test results
        TestResults? nullResults = null;

        // Act: attempt to serialize null test results (combined with Assert)
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TrxSerializer.Serialize(nullResults!));
        Assert.AreEqual("results", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentNullException for null input
    /// </summary>
    [TestMethod]
    public void TrxSerializer_Deserialize_NullContents_ThrowsArgumentNullException()
    {
        // Arrange: null contents
        string? nullContents = null;

        // Act: attempt to deserialize null contents (combined with Assert)
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => TrxSerializer.Deserialize(nullContents!));
        Assert.AreEqual("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    [TestMethod]
    public void TrxSerializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange: empty string
        var emptyContents = string.Empty;

        // Act: attempt to deserialize empty string (combined with Assert)
        var ex = Assert.ThrowsExactly<ArgumentException>(() => TrxSerializer.Deserialize(emptyContents));
        Assert.AreEqual("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    [TestMethod]
    public void TrxSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange: whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act: attempt to deserialize whitespace string (combined with Assert)
        var ex = Assert.ThrowsExactly<ArgumentException>(() => TrxSerializer.Deserialize(whitespaceContents));
        Assert.AreEqual("trxContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that serialization emits the storage attribute on UnitTest matching CodeBase
    /// </summary>
    /// <remarks>
    ///     Tests that TrxSerializer emits a storage attribute on the UnitTest element and that
    ///     its value matches the TestResult.CodeBase property, consistent with the de-facto TRX
    ///     schema where both UnitTest/@storage and TestMethod/@codeBase reference the test assembly.
    /// </remarks>
    [TestMethod]
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
        Assert.IsNotNull(result);

        // Assert: Parse and verify the storage attribute on the UnitTest element
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        var unitTest = doc.XPathSelectElement(
            "/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='TestWithCodeBase']", nsMgr);
        Assert.IsNotNull(unitTest);
        Assert.AreEqual("path/to/MyAssembly.dll", unitTest.Attribute("storage")?.Value);

        // Also verify TestMethod/@codeBase matches
        var testMethod = unitTest.XPathSelectElement("trx:TestMethod", nsMgr);
        Assert.IsNotNull(testMethod);
        Assert.AreEqual("path/to/MyAssembly.dll", testMethod.Attribute("codeBase")?.Value);
    }

    /// <summary>
    ///     Tests that serializing TestResults to TRX and deserializing back preserves all key data.
    /// </summary>
    /// <remarks>
    ///     Proves that serializing TestResults to TRX and deserializing back preserves all key data
    ///     including run metadata, test properties, outcomes, timing, and output/error information.
    /// </remarks>
    [TestMethod]
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
        Assert.IsNotNull(deserialized);
        Assert.AreEqual(original.Name, deserialized.Name);
        Assert.AreEqual(original.UserName, deserialized.UserName);
        Assert.HasCount(4, deserialized.Results);

        // Assert: PassedTest properties are preserved
        var passed = deserialized.Results[0];
        Assert.AreEqual("PassedTest", passed.Name);
        Assert.AreEqual("Suite.PassedClass", passed.ClassName);
        Assert.AreEqual("path/to/TestAssembly.dll", passed.CodeBase);
        Assert.AreEqual("BuildAgent01", passed.ComputerName);
        Assert.AreEqual(TestOutcome.Passed, passed.Outcome);
        Assert.AreEqual(startTime, passed.StartTime);
        Assert.IsLessThan(0.001, Math.Abs((passed.Duration - TimeSpan.FromSeconds(1.5)).TotalSeconds));
        Assert.AreEqual("All good", passed.SystemOutput);
        Assert.AreEqual(string.Empty, passed.SystemError);
        Assert.AreEqual(string.Empty, passed.ErrorMessage);
        Assert.AreEqual(string.Empty, passed.ErrorStackTrace);

        // Assert: FailedTest properties are preserved
        var failed = deserialized.Results[1];
        Assert.AreEqual("FailedTest", failed.Name);
        Assert.AreEqual("Suite.FailedClass", failed.ClassName);
        Assert.AreEqual("path/to/TestAssembly.dll", failed.CodeBase);
        Assert.AreEqual("BuildAgent01", failed.ComputerName);
        Assert.AreEqual(TestOutcome.Failed, failed.Outcome);
        Assert.AreEqual(startTime.AddSeconds(2), failed.StartTime);
        Assert.IsLessThan(0.001, Math.Abs((failed.Duration - TimeSpan.FromSeconds(0.75)).TotalSeconds));
        Assert.AreEqual(string.Empty, failed.SystemOutput);
        Assert.AreEqual("err output", failed.SystemError);
        Assert.AreEqual("Expected 1 but was 2", failed.ErrorMessage);
        Assert.AreEqual("at Suite.FailedClass.FailedTest() line 42", failed.ErrorStackTrace);

        // Assert: ErrorTest properties are preserved
        var error = deserialized.Results[2];
        Assert.AreEqual("ErrorTest", error.Name);
        Assert.AreEqual("Suite.ErrorClass", error.ClassName);
        Assert.AreEqual(TestOutcome.Error, error.Outcome);
        Assert.AreEqual(startTime.AddSeconds(4), error.StartTime);
        Assert.IsLessThan(0.001, Math.Abs((error.Duration - TimeSpan.FromSeconds(0.25)).TotalSeconds));
        Assert.AreEqual("NullReferenceException", error.ErrorMessage);
        Assert.AreEqual("at Suite.ErrorClass.ErrorTest() line 17", error.ErrorStackTrace);

        // Assert: SkippedTest properties are preserved
        var skipped = deserialized.Results[3];
        Assert.AreEqual("SkippedTest", skipped.Name);
        Assert.AreEqual("Suite.SkippedClass", skipped.ClassName);
        Assert.AreEqual(TestOutcome.NotExecuted, skipped.Outcome);
        Assert.AreEqual(startTime.AddSeconds(6), skipped.StartTime);
        Assert.IsLessThan(0.001, Math.Abs(skipped.Duration.TotalSeconds));
    }

    /// <summary>
    ///     Test that TrxSerializer serializes to a string with an XML declaration declaring UTF-8 encoding
    /// </summary>
    /// <remarks>
    ///     Proves that the Utf8StringWriter helper used internally by TrxSerializer produces output
    ///     with an explicit <c>encoding="utf-8"</c> XML declaration, ensuring consuming tools
    ///     interpret the content correctly.
    /// </remarks>
    [TestMethod]
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
        Assert.IsTrue(
            xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"", StringComparison.OrdinalIgnoreCase),
            "Expected XML declaration with encoding=\"utf-8\" at start of output");
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException for TRX with duplicate UnitTest IDs
    /// </summary>
    [TestMethod]
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
        Assert.ThrowsExactly<InvalidOperationException>(() => TrxSerializer.Deserialize(trxWithDuplicateIds));
    }

    /// <summary>
    ///     Test that Deserialize throws InvalidOperationException when a UnitTestResult references a non-existent testId
    /// </summary>
    [TestMethod]
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
        Assert.ThrowsExactly<InvalidOperationException>(() => TrxSerializer.Deserialize(trxWithMissingTestId));
    }
}
