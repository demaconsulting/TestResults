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
    public void JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml()
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
    public void JUnitSerializer_Serialize_FailedTest_IncludesFailureElement()
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
    public void JUnitSerializer_Serialize_ErrorTest_IncludesErrorElement()
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
    public void JUnitSerializer_Serialize_SkippedTest_IncludesSkippedElement()
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
    public void JUnitSerializer_Serialize_TestWithOutput_IncludesSystemOutAndErr()
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
    public void JUnitSerializer_Serialize_MultipleTestsInClasses_GroupsByClassName()
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
    public void JUnitSerializer_Serialize_EmptyClassName_UsesDefaultSuite()
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
    public void JUnitSerializer_Serialize_UsageExample_ProducesValidJUnitXml()
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

    /// <summary>
    ///     Test for basic deserialization
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults()
    {
        // Deserialize the test results object
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="BasicTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="MyTestClass" time="1.500" />
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert results information
        Assert.AreEqual("BasicTests", results.Name);
        Assert.HasCount(1, results.Results);

        // Assert test result information
        var result = results.Results[0];
        Assert.AreEqual("Test1", result.Name);
        Assert.AreEqual("MyTestClass", result.ClassName);
        Assert.AreEqual(1.5, result.Duration.TotalSeconds);
        Assert.AreEqual(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test for deserialization with failure
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_FailedTest_ReturnsFailureDetails()
    {
        // Deserialize the test results object with a failed test
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="FailureTests">
              <testsuite name="MyTestClass" tests="1" failures="1" errors="0" skipped="0" time="0.500">
                <testcase name="Test2" classname="MyTestClass" time="0.500">
                  <failure message="Expected value to be 42 but was 0"><![CDATA[at MyTestClass.Test2() in Test.cs:line 15]]></failure>
                </testcase>
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert results information
        Assert.AreEqual("FailureTests", results.Name);
        Assert.HasCount(1, results.Results);

        // Assert test result information
        var result = results.Results[0];
        Assert.AreEqual("Test2", result.Name);
        Assert.AreEqual("MyTestClass", result.ClassName);
        Assert.AreEqual(TestOutcome.Failed, result.Outcome);
        Assert.AreEqual("Expected value to be 42 but was 0", result.ErrorMessage);
        Assert.Contains("at MyTestClass.Test2() in Test.cs:line 15", result.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with error
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_ErrorTest_ReturnsErrorDetails()
    {
        // Deserialize the test results object with an error test
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="ErrorTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="1" skipped="0" time="0.100">
                <testcase name="Test3" classname="MyTestClass" time="0.100">
                  <error message="Unexpected exception occurred"><![CDATA[at MyTestClass.Test3() in Test.cs:line 20]]></error>
                </testcase>
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert test result information
        var result = results.Results[0];
        Assert.AreEqual("Test3", result.Name);
        Assert.AreEqual(TestOutcome.Error, result.Outcome);
        Assert.AreEqual("Unexpected exception occurred", result.ErrorMessage);
        Assert.Contains("at MyTestClass.Test3() in Test.cs:line 20", result.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with skipped test
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_SkippedTest_ReturnsSkippedStatus()
    {
        // Deserialize the test results object with a skipped test
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="SkippedTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="1" time="0.000">
                <testcase name="Test4" classname="MyTestClass" time="0.000">
                  <skipped message="Test was skipped" />
                </testcase>
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert test result information
        var result = results.Results[0];
        Assert.AreEqual("Test4", result.Name);
        Assert.AreEqual(TestOutcome.NotExecuted, result.Outcome);
        Assert.AreEqual("Test was skipped", result.ErrorMessage);
    }

    /// <summary>
    ///     Test for deserialization with system output
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_TestWithOutput_ReturnsSystemOutput()
    {
        // Deserialize the test results object with system output
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="OutputTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="1.000">
                <testcase name="Test5" classname="MyTestClass" time="1.000">
                  <system-out><![CDATA[Standard output message]]></system-out>
                  <system-err><![CDATA[Standard error message]]></system-err>
                </testcase>
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert test result information
        var result = results.Results[0];
        Assert.AreEqual("Test5", result.Name);
        Assert.AreEqual("Standard output message", result.SystemOutput);
        Assert.AreEqual("Standard error message", result.SystemError);
    }

    /// <summary>
    ///     Test for deserialization with multiple test suites
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_MultipleTestSuites_ReturnsAllTests()
    {
        // Deserialize the test results object with multiple test suites
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="MultipleTests">
              <testsuite name="Class1" tests="2" failures="1" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="Class1" time="1.000" />
                <testcase name="Test2" classname="Class1" time="0.500">
                  <failure message="Test failed" />
                </testcase>
              </testsuite>
              <testsuite name="Class2" tests="1" failures="0" errors="0" skipped="0" time="2.000">
                <testcase name="Test3" classname="Class2" time="2.000" />
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert results information
        Assert.AreEqual("MultipleTests", results.Name);
        Assert.HasCount(3, results.Results);

        // Verify first test
        var test1 = results.Results[0];
        Assert.AreEqual("Test1", test1.Name);
        Assert.AreEqual("Class1", test1.ClassName);
        Assert.AreEqual(TestOutcome.Passed, test1.Outcome);

        // Verify second test
        var test2 = results.Results[1];
        Assert.AreEqual("Test2", test2.Name);
        Assert.AreEqual(TestOutcome.Failed, test2.Outcome);

        // Verify third test
        var test3 = results.Results[2];
        Assert.AreEqual("Test3", test3.Name);
        Assert.AreEqual("Class2", test3.ClassName);
        Assert.AreEqual(TestOutcome.Passed, test3.Outcome);
    }

    /// <summary>
    ///     Test for deserialization with empty class name (DefaultSuite)
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_DefaultSuite_ReturnsEmptyClassName()
    {
        // Deserialize the test results object with DefaultSuite
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="EmptyClassTests">
              <testsuite name="DefaultSuite" tests="1" failures="0" errors="0" skipped="0" time="1.000">
                <testcase name="Test1" classname="DefaultSuite" time="1.000" />
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert test result information - DefaultSuite should be converted to empty string
        var result = results.Results[0];
        Assert.AreEqual("Test1", result.Name);
        Assert.AreEqual(string.Empty, result.ClassName);
    }

    /// <summary>
    ///     Test for round-trip serialization and deserialization
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Serialize_ThenDeserialize_PreservesTestData()
    {
        // Create original test results
        var original = new TestResults { Name = "RoundTripTests" };
        original.Results.Add(
            new TestResult
            {
                Name = "Test1",
                ClassName = "TestClass",
                Duration = TimeSpan.FromSeconds(1.5),
                Outcome = TestOutcome.Passed,
                SystemOutput = "Output message"
            });
        original.Results.Add(
            new TestResult
            {
                Name = "Test2",
                ClassName = "TestClass",
                Duration = TimeSpan.FromSeconds(0.5),
                Outcome = TestOutcome.Failed,
                ErrorMessage = "Test failed",
                ErrorStackTrace = "Stack trace here"
            });

        // Serialize to JUnit XML
        var xml = JUnitSerializer.Serialize(original);

        // Deserialize back
        var deserialized = JUnitSerializer.Deserialize(xml);

        // Verify results match
        Assert.AreEqual(original.Name, deserialized.Name);
        Assert.HasCount(original.Results.Count, deserialized.Results);

        // Verify first test
        var origTest1 = original.Results[0];
        var deserializedTest1 = deserialized.Results[0];
        Assert.AreEqual(origTest1.Name, deserializedTest1.Name);
        Assert.AreEqual(origTest1.ClassName, deserializedTest1.ClassName);
        Assert.AreEqual(origTest1.Duration.TotalSeconds, deserializedTest1.Duration.TotalSeconds, 0.001);
        Assert.AreEqual(origTest1.Outcome, deserializedTest1.Outcome);
        Assert.AreEqual(origTest1.SystemOutput, deserializedTest1.SystemOutput);

        // Verify second test
        var origTest2 = original.Results[1];
        var deserializedTest2 = deserialized.Results[1];
        Assert.AreEqual(origTest2.Name, deserializedTest2.Name);
        Assert.AreEqual(origTest2.ClassName, deserializedTest2.ClassName);
        Assert.AreEqual(origTest2.Outcome, deserializedTest2.Outcome);
        Assert.AreEqual(origTest2.ErrorMessage, deserializedTest2.ErrorMessage);
        Assert.AreEqual(origTest2.ErrorStackTrace, deserializedTest2.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with missing time attribute
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_MissingTimeAttribute_DefaultsToZero()
    {
        // Deserialize the test results object without time attribute
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="MissingTimeTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="0.000">
                <testcase name="TestWithoutTime" classname="MyTestClass" />
              </testsuite>
            </testsuites>
            """);
        Assert.IsNotNull(results);

        // Assert test result information - duration should default to zero
        var result = results.Results[0];
        Assert.AreEqual("TestWithoutTime", result.Name);
        Assert.AreEqual("MyTestClass", result.ClassName);
        Assert.AreEqual(TimeSpan.Zero, result.Duration);
        Assert.AreEqual(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test that Serialize throws ArgumentNullException for null input
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Serialize_NullResults_ThrowsArgumentNullException()
    {
        // Arrange - null test results
        TestResults? nullResults = null;

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentNullException>(() => JUnitSerializer.Serialize(nullResults!));
        Assert.AreEqual("results", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for null input
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_NullContents_ThrowsArgumentException()
    {
        // Arrange - null contents
        string? nullContents = null;

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => JUnitSerializer.Deserialize(nullContents!));
        Assert.AreEqual("junitContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange - empty string
        var emptyContents = string.Empty;

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => JUnitSerializer.Deserialize(emptyContents));
        Assert.AreEqual("junitContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    [TestMethod]
    public void JUnitSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange - whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act & Assert
        var ex = Assert.ThrowsExactly<ArgumentException>(() => JUnitSerializer.Deserialize(whitespaceContents));
        Assert.AreEqual("junitContents", ex.ParamName);
    }
}
