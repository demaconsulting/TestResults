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
    public void Serialize_BasicTestResults_ProducesValidTrxXml()
    {
        // Construct a basic test results object
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

        // Serialize the test suites object
        var result = TrxSerializer.Serialize(suites);
        Assert.IsNotNull(result);

        // Parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Verify the UnitTestResult element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test']", nsMgr));

        // Verify the UnitTest element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test']", nsMgr));

        // Verify the Counters element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='0']", nsMgr));
    }

    /// <summary>
    ///     Test for complex serialization
    /// </summary>
    [TestMethod]
    public void Serialize_MultipleTestResults_ProducesValidTrxXml()
    {
        // Construct a complex test results object
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

        // Serialize the test suites object
        var result = TrxSerializer.Serialize(suites);
        Assert.IsNotNull(result);

        // Parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Verify the UnitTestResult elements are present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='Test2']", nsMgr));

        // Verify the UnitTest elements are present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@name='Test2']", nsMgr));

        // Verify the Counters element is present
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@total='2']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@executed='2']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@passed='1']", nsMgr));
        Assert.IsNotNull(doc.XPathSelectElement("/trx:TestRun/trx:ResultSummary/trx:Counters[@failed='1']", nsMgr));
    }

    /// <summary>
    ///     Test for basic deserialization
    /// </summary>
    [TestMethod]
    public void Deserialize_BasicTrxXml_ReturnsTestResults()
    {
        // Deserialize the test results object
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

        // Assert results information
        Assert.AreEqual(Guid.Parse("0ef15ada-c28f-4755-8d4c-5b68d1f9dda6"), results.Id);
        Assert.AreEqual("Basic", results.Name);
        Assert.AreEqual("user", results.UserName);
        Assert.HasCount(1, results.Results);

        // Assert test result information
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
    public void Deserialize_ComplexTrxXml_ReturnsTestResults()
    {
        // Deserialize the test results object
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

        // Assert results information
        Assert.AreEqual(Guid.Parse("0704cd18-88b1-43f7-868e-ad02bfda887d"), results.Id);
        Assert.AreEqual("Basic", results.Name);
        Assert.AreEqual("user", results.UserName);
        Assert.HasCount(2, results.Results);

        // Assert test1 result information
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

        // Assert test2 result information
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
    public void Serialize_StackTraceWithoutMessage_IncludesStackTraceElement()
    {
        // Construct a test results object with stack trace but no message
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

        // Serialize the test suites object
        var result = TrxSerializer.Serialize(suites);
        Assert.IsNotNull(result);

        // Parse the document
        var doc = XDocument.Parse(result);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace);

        // Verify the StackTrace element is present
        var stackTraceElement = doc.XPathSelectElement(
            "/trx:TestRun/trx:Results/trx:UnitTestResult[@testName='TestWithStackTrace']/trx:Output/trx:ErrorInfo/trx:StackTrace",
            nsMgr);
        Assert.IsNotNull(stackTraceElement);
        Assert.Contains("at TestClass.Method() in Test.cs:line 42", stackTraceElement.Value);
    }
}
