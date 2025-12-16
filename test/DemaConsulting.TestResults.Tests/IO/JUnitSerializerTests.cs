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

using System.Xml.Linq;
using DemaConsulting.TestResults.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for JUnitSerializer class
/// </summary>
[TestClass]
public sealed class JUnitSerializerTests
{
    /// <summary>
    ///     Test for basic serialization with passed test
    /// </summary>
    [TestMethod]
    public void TestSerializeBasic()
    {
        // Construct a basic test results object
        var results = new TestResults
        {
            Name = "BasicTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test1",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.FromSeconds(1.5),
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.IsNotNull(root);
        Assert.AreEqual("testsuites", root.Name.LocalName);
        Assert.AreEqual("BasicTests", root.Attribute("name")?.Value);

        // Verify test suite
        var testSuite = root.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("MyTestClass", testSuite.Attribute("name")?.Value);
        Assert.AreEqual("1", testSuite.Attribute("tests")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("failures")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("errors")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("skipped")?.Value);
        Assert.AreEqual("1.500", testSuite.Attribute("time")?.Value);

        // Verify test case
        var testCase = testSuite.Element("testcase");
        Assert.IsNotNull(testCase);
        Assert.AreEqual("Test1", testCase.Attribute("name")?.Value);
        Assert.AreEqual("MyTestClass", testCase.Attribute("classname")?.Value);
        Assert.AreEqual("1.500", testCase.Attribute("time")?.Value);

        // Verify no failure/error/skipped elements for passed test
        Assert.IsNull(testCase.Element("failure"));
        Assert.IsNull(testCase.Element("error"));
        Assert.IsNull(testCase.Element("skipped"));
    }

    /// <summary>
    ///     Test for serialization with failed test
    /// </summary>
    [TestMethod]
    public void TestSerializeWithFailure()
    {
        // Construct test results with a failed test
        var results = new TestResults
        {
            Name = "FailureTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test2",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.FromSeconds(0.5),
                    Outcome = TestOutcome.Failed,
                    ErrorMessage = "Expected value to be 42 but was 0",
                    ErrorStackTrace = "at MyTestClass.Test2() in Test.cs:line 15"
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("1", testSuite.Attribute("tests")?.Value);
        Assert.AreEqual("1", testSuite.Attribute("failures")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("errors")?.Value);

        // Verify test case with failure
        var testCase = testSuite.Element("testcase");
        Assert.IsNotNull(testCase);
        Assert.AreEqual("Test2", testCase.Attribute("name")?.Value);

        // Verify failure element
        var failure = testCase.Element("failure");
        Assert.IsNotNull(failure);
        Assert.AreEqual("Expected value to be 42 but was 0", failure.Attribute("message")?.Value);
        Assert.Contains("at MyTestClass.Test2() in Test.cs:line 15", failure.Value);
    }

    /// <summary>
    ///     Test for serialization with error test
    /// </summary>
    [TestMethod]
    public void TestSerializeWithError()
    {
        // Construct test results with an error test
        var results = new TestResults
        {
            Name = "ErrorTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test3",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.FromSeconds(0.1),
                    Outcome = TestOutcome.Error,
                    ErrorMessage = "Unexpected exception occurred",
                    ErrorStackTrace = "at MyTestClass.Test3() in Test.cs:line 20"
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("1", testSuite.Attribute("tests")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("failures")?.Value);
        Assert.AreEqual("1", testSuite.Attribute("errors")?.Value);

        // Verify test case with error
        var testCase = testSuite.Element("testcase");
        Assert.IsNotNull(testCase);
        var error = testCase.Element("error");
        Assert.IsNotNull(error);
        Assert.AreEqual("Unexpected exception occurred", error.Attribute("message")?.Value);
        Assert.Contains("at MyTestClass.Test3() in Test.cs:line 20", error.Value);
    }

    /// <summary>
    ///     Test for serialization with skipped test
    /// </summary>
    [TestMethod]
    public void TestSerializeWithSkipped()
    {
        // Construct test results with a skipped test
        var results = new TestResults
        {
            Name = "SkippedTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test4",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.Zero,
                    Outcome = TestOutcome.NotExecuted,
                    ErrorMessage = "Test was skipped"
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("1", testSuite.Attribute("tests")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("failures")?.Value);
        Assert.AreEqual("0", testSuite.Attribute("errors")?.Value);
        Assert.AreEqual("1", testSuite.Attribute("skipped")?.Value);

        // Verify test case with skipped element
        var testCase = testSuite.Element("testcase");
        Assert.IsNotNull(testCase);
        var skipped = testCase.Element("skipped");
        Assert.IsNotNull(skipped);
        Assert.AreEqual("Test was skipped", skipped.Attribute("message")?.Value);
    }

    /// <summary>
    ///     Test for serialization with system output
    /// </summary>
    [TestMethod]
    public void TestSerializeWithSystemOutput()
    {
        // Construct test results with system output
        var results = new TestResults
        {
            Name = "OutputTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test5",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Passed,
                    SystemOutput = "Standard output message",
                    SystemError = "Standard error message"
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testCase = doc.Root?.Element("testsuite")?.Element("testcase");
        Assert.IsNotNull(testCase);

        // Verify system-out element
        var systemOut = testCase.Element("system-out");
        Assert.IsNotNull(systemOut);
        Assert.AreEqual("Standard output message", systemOut.Value);

        // Verify system-err element
        var systemErr = testCase.Element("system-err");
        Assert.IsNotNull(systemErr);
        Assert.AreEqual("Standard error message", systemErr.Value);
    }

    /// <summary>
    ///     Test for serialization with multiple test results
    /// </summary>
    [TestMethod]
    public void TestSerializeMultipleTests()
    {
        // Construct test results with multiple tests
        var results = new TestResults
        {
            Name = "MultipleTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test1",
                    ClassName = "Class1",
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Passed
                },
                new TestResult
                {
                    Name = "Test2",
                    ClassName = "Class1",
                    Duration = TimeSpan.FromSeconds(0.5),
                    Outcome = TestOutcome.Failed,
                    ErrorMessage = "Test failed"
                },
                new TestResult
                {
                    Name = "Test3",
                    ClassName = "Class2",
                    Duration = TimeSpan.FromSeconds(2.0),
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.IsNotNull(root);

        // Verify two test suites (one for each class)
        var testSuites = root.Elements("testsuite").ToList();
        Assert.HasCount(2, testSuites);

        // Verify first test suite (Class1)
        var suite1 = testSuites[0];
        Assert.AreEqual("Class1", suite1.Attribute("name")?.Value);
        Assert.AreEqual("2", suite1.Attribute("tests")?.Value);
        Assert.AreEqual("1", suite1.Attribute("failures")?.Value);
        Assert.AreEqual("1.500", suite1.Attribute("time")?.Value);

        // Verify second test suite (Class2)
        var suite2 = testSuites[1];
        Assert.AreEqual("Class2", suite2.Attribute("name")?.Value);
        Assert.AreEqual("1", suite2.Attribute("tests")?.Value);
        Assert.AreEqual("0", suite2.Attribute("failures")?.Value);
        Assert.AreEqual("2.000", suite2.Attribute("time")?.Value);
    }

    /// <summary>
    ///     Test for serialization with empty class name
    /// </summary>
    [TestMethod]
    public void TestSerializeEmptyClassName()
    {
        // Construct test results with empty class name
        var results = new TestResults
        {
            Name = "EmptyClassTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test1",
                    ClassName = string.Empty,
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("DefaultSuite", testSuite.Attribute("name")?.Value);

        var testCase = testSuite.Element("testcase");
        Assert.IsNotNull(testCase);
        Assert.AreEqual("DefaultSuite", testCase.Attribute("classname")?.Value);
    }

    /// <summary>
    ///     Test for serialization matching the usage example from the issue
    /// </summary>
    [TestMethod]
    public void TestSerializeUsageExample()
    {
        // Create a TestResults instance matching the usage example
        var results = new TestResults { Name = "SomeTests" };

        // Add some results
        results.Results.Add(
            new TestResult
            {
                Name = "Test1",
                ClassName = "SomeTestClass",
                CodeBase = "MyTestAssembly",
                Outcome = TestOutcome.Passed,
                Duration = TimeSpan.FromSeconds(1.5),
                StartTime = DateTime.UtcNow,
            });

        results.Results.Add(
            new TestResult
            {
                Name = "Test2",
                ClassName = "SomeTestClass",
                CodeBase = "MyTestAssembly",
                Outcome = TestOutcome.Failed,
                ErrorMessage = "Expected value to be 42 but was 0",
                ErrorStackTrace = "at SomeTestClass.Test2() in Test.cs:line 15"
            });

        // Serialize the results
        var xml = JUnitSerializer.Serialize(results);
        Assert.IsNotNull(xml);

        // Parse and verify the structure matches expected JUnit format
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.IsNotNull(root);
        Assert.AreEqual("testsuites", root.Name.LocalName);
        Assert.AreEqual("SomeTests", root.Attribute("name")?.Value);

        // Verify test suite
        var testSuite = root.Element("testsuite");
        Assert.IsNotNull(testSuite);
        Assert.AreEqual("SomeTestClass", testSuite.Attribute("name")?.Value);
        Assert.AreEqual("2", testSuite.Attribute("tests")?.Value);
        Assert.AreEqual("1", testSuite.Attribute("failures")?.Value);

        // Verify both test cases are present
        var testCases = testSuite.Elements("testcase").ToList();
        Assert.HasCount(2, testCases);

        // Verify passed test
        var passedTest = testCases.FirstOrDefault(tc => tc.Attribute("name")?.Value == "Test1");
        Assert.IsNotNull(passedTest);
        Assert.IsNull(passedTest.Element("failure"));

        // Verify failed test
        var failedTest = testCases.FirstOrDefault(tc => tc.Attribute("name")?.Value == "Test2");
        Assert.IsNotNull(failedTest);
        var failure = failedTest.Element("failure");
        Assert.IsNotNull(failure);
        Assert.AreEqual("Expected value to be 42 but was 0", failure.Attribute("message")?.Value);
    }
}
