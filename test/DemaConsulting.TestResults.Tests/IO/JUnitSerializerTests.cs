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
using Xunit;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for JUnitSerializer class
/// </summary>
public sealed class JUnitSerializerTests
{
    /// <summary>
    ///     Test for basic serialization with passed test
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer correctly serializes a basic TestResults object with a single passed test.
    ///     Proves that the serializer produces valid JUnit XML with correct testsuites structure, 
    ///     testsuite counters (tests=1, failures=0, errors=0, skipped=0), and testcase attributes
    ///     including proper time formatting and that no failure/error/skipped elements are present for passed tests.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_PassedTest_ProducesValidJUnitXml()
    {
        // Arrange: Construct a basic test results object with one passed test
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

        // Act: Serialize the test results to JUnit XML
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.NotNull(root);
        Assert.Equal("testsuites", root.Name.LocalName);
        Assert.Equal("BasicTests", root.Attribute("name")?.Value);

        // Assert: Verify test suite attributes
        var testSuite = root.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("MyTestClass", testSuite.Attribute("name")?.Value);
        Assert.Equal("1", testSuite.Attribute("tests")?.Value);
        Assert.Equal("0", testSuite.Attribute("failures")?.Value);
        Assert.Equal("0", testSuite.Attribute("errors")?.Value);
        Assert.Equal("0", testSuite.Attribute("skipped")?.Value);
        Assert.Equal("1.500", testSuite.Attribute("time")?.Value);

        // Assert: Verify test case attributes
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        Assert.Equal("Test1", testCase.Attribute("name")?.Value);
        Assert.Equal("MyTestClass", testCase.Attribute("classname")?.Value);
        Assert.Equal("1.500", testCase.Attribute("time")?.Value);

        // Assert: Verify no failure/error/skipped elements for passed test
        Assert.Null(testCase.Element("failure"));
        Assert.Null(testCase.Element("error"));
        Assert.Null(testCase.Element("skipped"));
    }

    /// <summary>
    ///     Test for serialization with failed test
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer correctly serializes a failed test with error message and stack trace.
    ///     Proves that the serializer includes a failure element with the correct message attribute and 
    ///     stack trace content, and updates the failures counter correctly (failures=1).
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_FailedTest_IncludesFailureElement()
    {
        // Arrange: Construct test results with a failed test
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

        // Act: Serialize the test results to JUnit XML
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("1", testSuite.Attribute("tests")?.Value);
        Assert.Equal("1", testSuite.Attribute("failures")?.Value);
        Assert.Equal("0", testSuite.Attribute("errors")?.Value);

        // Assert: Verify test case with failure element
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        Assert.Equal("Test2", testCase.Attribute("name")?.Value);

        // Assert: Verify failure element contains error message and stack trace
        var failure = testCase.Element("failure");
        Assert.NotNull(failure);
        Assert.Equal("Expected value to be 42 but was 0", failure.Attribute("message")?.Value);
        Assert.Contains("at MyTestClass.Test2() in Test.cs:line 15", failure.Value);
    }

    /// <summary>
    ///     Test for serialization with error test
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps <see cref="TestOutcome.Error"/> to an <c>error</c>
    ///     element (not a <c>failure</c> element), with the correct <c>message</c> attribute and
    ///     stack trace content, and increments the suite <c>errors</c> counter correctly.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_ErrorTest_IncludesErrorElement()
    {
        // Arrange: Construct test results with an error test
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

        // Act: Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("1", testSuite.Attribute("tests")?.Value);
        Assert.Equal("0", testSuite.Attribute("failures")?.Value);
        Assert.Equal("1", testSuite.Attribute("errors")?.Value);

        // Assert: Verify test case with error
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        var error = testCase.Element("error");
        Assert.NotNull(error);
        Assert.Equal("Unexpected exception occurred", error.Attribute("message")?.Value);
        Assert.Contains("at MyTestClass.Test3() in Test.cs:line 20", error.Value);
    }

    /// <summary>
    ///     Test for serialization with skipped test
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps <see cref="TestOutcome.NotExecuted"/> to a
    ///     <c>skipped</c> element with the optional <c>message</c> attribute populated from
    ///     <see cref="TestResult.ErrorMessage"/>, and increments the suite <c>skipped</c>
    ///     counter correctly.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_SkippedTest_IncludesSkippedElement()
    {
        // Arrange: Construct test results with a skipped test
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

        // Act: Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("1", testSuite.Attribute("tests")?.Value);
        Assert.Equal("0", testSuite.Attribute("failures")?.Value);
        Assert.Equal("0", testSuite.Attribute("errors")?.Value);
        Assert.Equal("1", testSuite.Attribute("skipped")?.Value);

        // Assert: Verify test case with skipped element
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        var skipped = testCase.Element("skipped");
        Assert.NotNull(skipped);
        Assert.Equal("Test was skipped", skipped.Attribute("message")?.Value);
    }

    /// <summary>
    ///     Test for serialization with system output
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer emits <c>system-out</c> and <c>system-err</c> child
    ///     elements on the <c>testcase</c> when <see cref="TestResult.SystemOutput"/> and
    ///     <see cref="TestResult.SystemError"/> are non-empty.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_TestWithOutput_IncludesSystemOutAndErr()
    {
        // Arrange: Construct test results with system output
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

        // Act: Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testCase = doc.Root?.Element("testsuite")?.Element("testcase");
        Assert.NotNull(testCase);

        // Assert: Verify system-out element
        var systemOut = testCase.Element("system-out");
        Assert.NotNull(systemOut);
        Assert.Equal("Standard output message", systemOut.Value);

        // Assert: Verify system-err element
        var systemErr = testCase.Element("system-err");
        Assert.NotNull(systemErr);
        Assert.Equal("Standard error message", systemErr.Value);
    }

    /// <summary>
    ///     Test for serialization with multiple test results
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer groups tests by <see cref="TestResult.ClassName"/>,
    ///     producing one <c>testsuite</c> per distinct class name, each with correct
    ///     <c>tests</c>, <c>failures</c>, and <c>time</c> counter attributes.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_MultipleTestsInClasses_GroupsByClassName()
    {
        // Arrange: Construct test results with multiple tests
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

        // Act: Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.NotNull(root);

        // Assert: Verify two test suites (one for each class)
        var testSuites = root.Elements("testsuite").ToList();
        Assert.Equal(2, testSuites.Count);

        // Assert: Verify first test suite (Class1)
        var suite1 = testSuites[0];
        Assert.Equal("Class1", suite1.Attribute("name")?.Value);
        Assert.Equal("2", suite1.Attribute("tests")?.Value);
        Assert.Equal("1", suite1.Attribute("failures")?.Value);
        Assert.Equal("1.500", suite1.Attribute("time")?.Value);

        // Assert: Verify second test suite (Class2)
        var suite2 = testSuites[1];
        Assert.Equal("Class2", suite2.Attribute("name")?.Value);
        Assert.Equal("1", suite2.Attribute("tests")?.Value);
        Assert.Equal("0", suite2.Attribute("failures")?.Value);
        Assert.Equal("2.000", suite2.Attribute("time")?.Value);
    }

    /// <summary>
    ///     Test for serialization with empty class name
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps tests with an empty
    ///     <see cref="TestResult.ClassName"/> to the <c>DefaultSuite</c> sentinel name in both
    ///     the <c>testsuite/@name</c> and <c>testcase/@classname</c> attributes.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_EmptyClassName_UsesDefaultSuite()
    {
        // Arrange: Construct test results with empty class name
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

        // Act: Serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the XML structure
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("DefaultSuite", testSuite.Attribute("name")?.Value);

        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        Assert.Equal("DefaultSuite", testCase.Attribute("classname")?.Value);
    }

    /// <summary>
    ///     Test for serialization matching the usage example from the issue
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer correctly handles a mixed pass/fail result set,
    ///     producing a valid <c>testsuites</c> document with the correct counters and
    ///     a <c>failure</c> element on the failing test case.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_UsageExample_ProducesValidJUnitXml()
    {
        // Arrange: Create a TestResults instance matching the usage example
        var results = new TestResults { Name = "SomeTests" };

        // Arrange: Add some results
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

        // Act: Serialize the results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the structure matches expected JUnit format
        var doc = XDocument.Parse(xml);
        var root = doc.Root;
        Assert.NotNull(root);
        Assert.Equal("testsuites", root.Name.LocalName);
        Assert.Equal("SomeTests", root.Attribute("name")?.Value);

        // Assert: Verify test suite
        var testSuite = root.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("SomeTestClass", testSuite.Attribute("name")?.Value);
        Assert.Equal("2", testSuite.Attribute("tests")?.Value);
        Assert.Equal("1", testSuite.Attribute("failures")?.Value);

        // Assert: Verify both test cases are present
        var testCases = testSuite.Elements("testcase").ToList();
        Assert.Equal(2, testCases.Count);

        // Assert: Verify passed test
        var passedTest = testCases.FirstOrDefault(tc => tc.Attribute("name")?.Value == "Test1");
        Assert.NotNull(passedTest);
        Assert.Null(passedTest.Element("failure"));

        // Assert: Verify failed test
        var failedTest = testCases.FirstOrDefault(tc => tc.Attribute("name")?.Value == "Test2");
        Assert.NotNull(failedTest);
        var failure = failedTest.Element("failure");
        Assert.NotNull(failure);
        Assert.Equal("Expected value to be 42 but was 0", failure.Attribute("message")?.Value);
    }

    /// <summary>
    ///     Test for basic deserialization
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer correctly deserializes basic JUnit XML with a single passed test.
    ///     Proves that the deserializer extracts test suite name, test count, test name, class name, 
    ///     duration, and outcome correctly from valid JUnit XML format.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_BasicJUnitXml_ReturnsTestResults()
    {
        // Arrange: Create basic JUnit XML content
        var junitXml = """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="BasicTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
                <testcase name="Test1" classname="MyTestClass" time="1.500" />
              </testsuite>
            </testsuites>
            """;

        // Act: Deserialize the test results object
        var results = JUnitSerializer.Deserialize(junitXml);

        // Assert: Verify results information
        Assert.NotNull(results);
        Assert.Equal("BasicTests", results.Name);
        Assert.Single(results.Results);

        // Assert: Verify test result information
        var result = results.Results[0];
        Assert.Equal("Test1", result.Name);
        Assert.Equal("MyTestClass", result.ClassName);
        Assert.Equal(1.5, result.Duration.TotalSeconds);
        Assert.Equal(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test for deserialization with failure
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps a <c>failure</c> child element to
    ///     <see cref="TestOutcome.Failed"/> and populates <see cref="TestResult.ErrorMessage"/>
    ///     from the <c>message</c> attribute and <see cref="TestResult.ErrorStackTrace"/> from
    ///     the element text content.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_FailedTest_ReturnsFailureDetails()
    {
        // Arrange and Act: Deserialize the test results object with a failed test
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
        Assert.NotNull(results);

        // Assert: Verify results information
        Assert.Equal("FailureTests", results.Name);
        Assert.Single(results.Results);

        // Assert: Verify test result information
        var result = results.Results[0];
        Assert.Equal("Test2", result.Name);
        Assert.Equal("MyTestClass", result.ClassName);
        Assert.Equal(TestOutcome.Failed, result.Outcome);
        Assert.Equal("Expected value to be 42 but was 0", result.ErrorMessage);
        Assert.Contains("at MyTestClass.Test2() in Test.cs:line 15", result.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with error
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps an <c>error</c> child element to
    ///     <see cref="TestOutcome.Error"/> and populates <see cref="TestResult.ErrorMessage"/>
    ///     and <see cref="TestResult.ErrorStackTrace"/> from the element attributes and content.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_ErrorTest_ReturnsErrorDetails()
    {
        // Arrange and Act: Deserialize the test results object with an error test
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
        Assert.NotNull(results);

        // Assert: Verify test result information
        var result = results.Results[0];
        Assert.Equal("Test3", result.Name);
        Assert.Equal(TestOutcome.Error, result.Outcome);
        Assert.Equal("Unexpected exception occurred", result.ErrorMessage);
        Assert.Contains("at MyTestClass.Test3() in Test.cs:line 20", result.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with skipped test
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps a <c>skipped</c> child element to
    ///     <see cref="TestOutcome.NotExecuted"/> and populates
    ///     <see cref="TestResult.ErrorMessage"/> from the optional <c>message</c> attribute.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_SkippedTest_ReturnsSkippedStatus()
    {
        // Arrange and Act: Deserialize the test results object with a skipped test
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
        Assert.NotNull(results);

        // Assert: Verify test result information
        var result = results.Results[0];
        Assert.Equal("Test4", result.Name);
        Assert.Equal(TestOutcome.NotExecuted, result.Outcome);
        Assert.Equal("Test was skipped", result.ErrorMessage);
    }

    /// <summary>
    ///     Test for deserialization with system output
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer reads <c>system-out</c> and <c>system-err</c> child
    ///     elements and maps them to <see cref="TestResult.SystemOutput"/> and
    ///     <see cref="TestResult.SystemError"/> respectively.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_TestWithOutput_ReturnsSystemOutput()
    {
        // Arrange and Act: Deserialize the test results object with system output
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
        Assert.NotNull(results);

        // Assert: Verify test result information
        var result = results.Results[0];
        Assert.Equal("Test5", result.Name);
        Assert.Equal("Standard output message", result.SystemOutput);
        Assert.Equal("Standard error message", result.SystemError);
    }

    /// <summary>
    ///     Test for deserialization with multiple test suites
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer collects test cases from all <c>testsuite</c> children
    ///     of a <c>testsuites</c> root, returning a flat list of all test results in document
    ///     order.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_MultipleTestSuites_ReturnsAllTests()
    {
        // Arrange and Act: Deserialize the test results object with multiple test suites
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
        Assert.NotNull(results);

        // Assert: Verify results information
        Assert.Equal("MultipleTests", results.Name);
        Assert.Equal(3, results.Results.Count);

        // Assert: Verify first test
        var test1 = results.Results[0];
        Assert.Equal("Test1", test1.Name);
        Assert.Equal("Class1", test1.ClassName);
        Assert.Equal(TestOutcome.Passed, test1.Outcome);

        // Assert: Verify second test
        var test2 = results.Results[1];
        Assert.Equal("Test2", test2.Name);
        Assert.Equal(TestOutcome.Failed, test2.Outcome);

        // Assert: Verify third test
        var test3 = results.Results[2];
        Assert.Equal("Test3", test3.Name);
        Assert.Equal("Class2", test3.ClassName);
        Assert.Equal(TestOutcome.Passed, test3.Outcome);
    }

    /// <summary>
    ///     Test for deserialization with empty class name (DefaultSuite)
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer maps the <c>DefaultSuite</c> sentinel class name back to
    ///     an empty string on deserialization, completing the round-trip for tests that were
    ///     originally serialized with an empty <see cref="TestResult.ClassName"/>.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_DefaultSuite_ReturnsEmptyClassName()
    {
        // Arrange and Act: Deserialize the test results object with DefaultSuite
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="EmptyClassTests">
              <testsuite name="DefaultSuite" tests="1" failures="0" errors="0" skipped="0" time="1.000">
                <testcase name="Test1" classname="DefaultSuite" time="1.000" />
              </testsuite>
            </testsuites>
            """);
        Assert.NotNull(results);

        // Assert: Verify test result information - DefaultSuite should be converted to empty string
        var result = results.Results[0];
        Assert.Equal("Test1", result.Name);
        Assert.Equal(string.Empty, result.ClassName);
    }

    /// <summary>
    ///     Test for round-trip serialization and deserialization
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer correctly performs round-trip serialization and deserialization.
    ///     Proves that serializing TestResults to JUnit XML and deserializing back preserves all key data 
    ///     including test names, class names, durations, outcomes, error messages, stack traces, and system output.
    ///     This validates data integrity through the complete serialize/deserialize cycle.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_ThenDeserialize_PreservesTestData()
    {
        // Arrange: Create original test results with multiple test scenarios
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

        // Act: Serialize to JUnit XML and then deserialize back
        var xml = JUnitSerializer.Serialize(original);
        var deserialized = JUnitSerializer.Deserialize(xml);

        // Assert: Verify high-level results match
        Assert.Equal(original.Name, deserialized.Name);
        Assert.Equal(original.Results.Count, deserialized.Results.Count);

        // Assert: Verify first test (passed with system output) preserved correctly
        var origTest1 = original.Results[0];
        var deserializedTest1 = deserialized.Results[0];
        Assert.Equal(origTest1.Name, deserializedTest1.Name);
        Assert.Equal(origTest1.ClassName, deserializedTest1.ClassName);
        Assert.Equal(origTest1.Duration.TotalSeconds, deserializedTest1.Duration.TotalSeconds, 3);
        Assert.Equal(origTest1.Outcome, deserializedTest1.Outcome);
        Assert.Equal(origTest1.SystemOutput, deserializedTest1.SystemOutput);

        // Assert: Verify second test (failed with error details) preserved correctly
        var origTest2 = original.Results[1];
        var deserializedTest2 = deserialized.Results[1];
        Assert.Equal(origTest2.Name, deserializedTest2.Name);
        Assert.Equal(origTest2.ClassName, deserializedTest2.ClassName);
        Assert.Equal(origTest2.Outcome, deserializedTest2.Outcome);
        Assert.Equal(origTest2.ErrorMessage, deserializedTest2.ErrorMessage);
        Assert.Equal(origTest2.ErrorStackTrace, deserializedTest2.ErrorStackTrace);
    }

    /// <summary>
    ///     Test for deserialization with missing time attribute
    /// </summary>
    /// <remarks>
    ///     Proves that JUnitSerializer tolerates a missing <c>time</c> attribute on
    ///     <c>testcase</c> elements by defaulting <see cref="TestResult.Duration"/> to
    ///     <see cref="TimeSpan.Zero"/> rather than throwing an exception.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_MissingTimeAttribute_DefaultsToZero()
    {
        // Arrange and Act: Deserialize the test results object without time attribute
        var results = JUnitSerializer.Deserialize(
            """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="MissingTimeTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="0.000">
                <testcase name="TestWithoutTime" classname="MyTestClass" />
              </testsuite>
            </testsuites>
            """);
        Assert.NotNull(results);

        // Assert: test result information - duration should default to zero
        var result = results.Results[0];
        Assert.Equal("TestWithoutTime", result.Name);
        Assert.Equal("MyTestClass", result.ClassName);
        Assert.Equal(TimeSpan.Zero, result.Duration);
        Assert.Equal(TestOutcome.Passed, result.Outcome);
    }

    /// <summary>
    ///     Test that Serialize throws ArgumentNullException for null input
    /// </summary>
    /// <remarks>
    ///     Proves that null input is rejected at the entry point before any serialization
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_NullResults_ThrowsArgumentNullException()
    {
        // Arrange: null test results
        TestResults? nullResults = null;

        // Act: attempt to serialize null test results (combined with Assert)
        var ex = Assert.Throws<ArgumentNullException>(() => JUnitSerializer.Serialize(nullResults!));
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
    public void JUnitSerializer_Deserialize_NullContents_ThrowsArgumentNullException()
    {
        // Arrange: null contents
        string? nullContents = null;

        // Act: attempt to deserialize null contents (combined with Assert)
        var ex = Assert.Throws<ArgumentNullException>(() => JUnitSerializer.Deserialize(nullContents!));
        Assert.Equal("junitContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for empty string input
    /// </summary>
    /// <remarks>
    ///     Proves that empty string input is rejected at the entry point before any XML parsing
    ///     attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_EmptyContents_ThrowsArgumentException()
    {
        // Arrange: empty string
        var emptyContents = string.Empty;

        // Act: attempt to deserialize empty string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => JUnitSerializer.Deserialize(emptyContents));
        Assert.Equal("junitContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that Deserialize throws ArgumentException for whitespace input
    /// </summary>
    /// <remarks>
    ///     Proves that whitespace-only input is rejected at the entry point before any XML
    ///     parsing attempt, with the correct parameter name reported in the exception.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_WhitespaceContents_ThrowsArgumentException()
    {
        // Arrange: whitespace string
        var whitespaceContents = "   \n\t  ";

        // Act: attempt to deserialize whitespace string (combined with Assert)
        var ex = Assert.Throws<ArgumentException>(() => JUnitSerializer.Deserialize(whitespaceContents));
        Assert.Equal("junitContents", ex.ParamName);
    }

    /// <summary>
    ///     Test that serialization emits a UTC timestamp on testsuite with Z suffix
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer emits a UTC timestamp attribute on each testsuite element
    ///     using the earliest StartTime in that suite, formatted as ISO 8601 with a trailing Z suffix
    ///     to unambiguously identify the value as UTC.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_WithStartTime_EmitsUtcTimestampAttribute()
    {
        // Arrange: test results with an explicit UTC start time
        var startTime = new DateTime(2025, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        var results = new TestResults
        {
            Name = "TimestampTests",
            Results =
            [
                new TestResult
                {
                    Name = "Test1",
                    ClassName = "MyTestClass",
                    StartTime = startTime,
                    Duration = TimeSpan.FromSeconds(1.0),
                    Outcome = TestOutcome.Passed
                }
            ]
        };

        // Act: Serialize the test results to JUnit XML
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: Parse and verify the timestamp attribute is present in UTC format with Z suffix
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        var timestampAttr = testSuite.Attribute("timestamp")?.Value;
        Assert.NotNull(timestampAttr);
        Assert.Equal("2025-06-15T10:30:00Z", timestampAttr);
    }

    /// <summary>
    ///     Test that deserialization reads the testsuite timestamp and applies it to test cases
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer correctly reads the optional timestamp attribute from
    ///     each testsuite element and applies it as StartTime to all test cases in that suite.
    ///     Also verifies that an absent timestamp leaves StartTime at its default (DateTime.UtcNow).
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_WithTimestamp_SetsStartTimeOnTestCases()
    {
        // Arrange: JUnit XML with a timestamp on one suite and no timestamp on another
        var junitXml = """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="TimestampTests">
              <testsuite name="Class1" tests="1" failures="0" errors="0" skipped="0" time="1.000" timestamp="2025-06-15T10:30:00Z">
                <testcase name="Test1" classname="Class1" time="1.000" />
              </testsuite>
              <testsuite name="Class2" tests="1" failures="0" errors="0" skipped="0" time="2.000">
                <testcase name="Test2" classname="Class2" time="2.000" />
              </testsuite>
            </testsuites>
            """;

        // Act: Deserialize the test results
        var before = DateTime.UtcNow;
        var results = JUnitSerializer.Deserialize(junitXml);
        var after = DateTime.UtcNow;

        // Assert: Test1 should have the suite timestamp as its StartTime
        Assert.NotNull(results);
        Assert.Equal(2, results.Results.Count);
        var test1 = results.Results[0];
        Assert.Equal(new DateTime(2025, 6, 15, 10, 30, 0, DateTimeKind.Utc), test1.StartTime);

        // Assert: Test2 should have a default StartTime (suite had no timestamp), between before and after
        var test2 = results.Results[1];
        Assert.True(test2.StartTime >= before && test2.StartTime <= after);
    }

    /// <summary>
    ///     Test that deserialization of a malformed timestamp does not throw
    /// </summary>
    /// <remarks>
    ///     Tests that JUnitSerializer gracefully ignores a malformed timestamp attribute
    ///     on the testsuite element, leaving StartTime at its default value rather than
    ///     throwing an exception.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_InvalidTimestamp_DefaultsStartTime()
    {
        // Arrange: JUnit XML with a malformed timestamp
        var junitXml = """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuites name="InvalidTimestampTests">
              <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="1.000" timestamp="not-a-timestamp">
                <testcase name="Test1" classname="MyTestClass" time="1.000" />
              </testsuite>
            </testsuites>
            """;

        // Act: Deserialize the test results (should not throw)
        var before = DateTime.UtcNow;
        var results = JUnitSerializer.Deserialize(junitXml);
        var after = DateTime.UtcNow;

        // Assert: StartTime should be at its default (between before and after) because the timestamp could not be parsed
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.True(results.Results[0].StartTime >= before && results.Results[0].StartTime <= after);
    }

    /// <summary>
    ///     Test that a Timeout outcome is deserialized as Error after a JUnit round-trip.
    /// </summary>
    /// <remarks>
    ///     Proves that JUnit XML has no distinct timeout element; both Timeout and Error outcomes
    ///     serialize to an <c>error</c> element, which deserializes back as
    ///     <see cref="TestOutcome.Error"/>. Timeout is therefore not preserved through a JUnit
    ///     round-trip.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_ThenDeserialize_TimeoutOutcomeBecomesError()
    {
        // Arrange: a single test result with a Timeout outcome
        var original = new TestResults
        {
            Name = "TimeoutFidelityTest",
            Results =
            [
                new TestResult
                {
                    Name = "TimedOutTest",
                    ClassName = "Suite.TimingClass",
                    Outcome = TestOutcome.Timeout,
                    Duration = TimeSpan.FromSeconds(30.0)
                }
            ]
        };

        // Act: serialize to JUnit XML and then deserialize back
        var junitXml = JUnitSerializer.Serialize(original);
        var deserialized = JUnitSerializer.Deserialize(junitXml);

        // Assert: the Timeout outcome becomes Error because JUnit has no timeout element
        Assert.NotNull(deserialized);
        Assert.Single(deserialized.Results);
        Assert.Equal(TestOutcome.Error, deserialized.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that an Aborted outcome is deserialized as Error after a JUnit round-trip.
    /// </summary>
    /// <remarks>
    ///     Proves that JUnit XML has no distinct aborted element; both Aborted and Error outcomes
    ///     serialize to an <c>error</c> element, which deserializes back as
    ///     <see cref="TestOutcome.Error"/>. Aborted is therefore not preserved through a JUnit
    ///     round-trip.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_ThenDeserialize_AbortedOutcomeBecomesError()
    {
        // Arrange: a single test result with an Aborted outcome
        var original = new TestResults
        {
            Name = "AbortedFidelityTest",
            Results =
            [
                new TestResult
                {
                    Name = "AbortedTest",
                    ClassName = "Suite.AbortedClass",
                    Outcome = TestOutcome.Aborted,
                    Duration = TimeSpan.FromSeconds(5.0)
                }
            ]
        };

        // Act: serialize to JUnit XML and then deserialize back
        var junitXml = JUnitSerializer.Serialize(original);
        var deserialized = JUnitSerializer.Deserialize(junitXml);

        // Assert: the Aborted outcome becomes Error because JUnit has no aborted element
        Assert.NotNull(deserialized);
        Assert.Single(deserialized.Results);
        Assert.Equal(TestOutcome.Error, deserialized.Results[0].Outcome);
    }

    /// <summary>
    ///     Tests that a bare testsuite root element (without a testsuites wrapper) is deserialized correctly.
    /// </summary>
    /// <remarks>
    ///     Proves that the deserializer handles both the common two-level structure
    ///     (<c>testsuites</c> → <c>testsuite</c> → <c>testcase</c>) and the bare single-level
    ///     structure (<c>testsuite</c> → <c>testcase</c>) that some JUnit producers emit.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Deserialize_BareTestSuiteRoot_DeserializesCorrectly()
    {
        // Arrange: JUnit XML with a bare testsuite root (no testsuites wrapper)
        var junitXml = """
            <?xml version="1.0" encoding="utf-8"?>
            <testsuite name="MyTestClass" tests="1" failures="0" errors="0" skipped="0" time="1.500">
              <testcase name="Test1" classname="MyTestClass" time="1.500" />
            </testsuite>
            """;

        // Act: Deserialize the test results
        var results = JUnitSerializer.Deserialize(junitXml);

        // Assert: one test result should be deserialized correctly
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal("MyTestClass", results.Results[0].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.Equal(TimeSpan.FromSeconds(1.5), results.Results[0].Duration);
    }

    /// <summary>
    ///     Test that a NotRunnable outcome serializes as a skipped element
    /// </summary>
    /// <remarks>
    ///     Proves that <see cref="TestOutcome.NotRunnable"/> is treated as a not-executed
    ///     outcome by JUnitSerializer and serialized as a <c>skipped</c> element, with the
    ///     suite <c>skipped</c> counter incremented.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_NotRunnableOutcome_IncludesSkippedElement()
    {
        // Arrange: test result with NotRunnable outcome
        var results = new TestResults
        {
            Name = "NotRunnableTests",
            Results =
            [
                new TestResult
                {
                    Name = "NotRunnableTest",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.Zero,
                    Outcome = TestOutcome.NotRunnable,
                    ErrorMessage = "Test is not runnable"
                }
            ]
        };

        // Act: serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: skipped element is present with message and skipped counter is incremented
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("1", testSuite.Attribute("skipped")?.Value);
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        var skipped = testCase.Element("skipped");
        Assert.NotNull(skipped);
        Assert.Equal("Test is not runnable", skipped.Attribute("message")?.Value);
    }

    /// <summary>
    ///     Test that a Pending outcome serializes as a skipped element
    /// </summary>
    /// <remarks>
    ///     Proves that <see cref="TestOutcome.Pending"/> is treated as a not-executed outcome
    ///     by JUnitSerializer and serialized as a <c>skipped</c> element, with the suite
    ///     <c>skipped</c> counter incremented.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_PendingOutcome_IncludesSkippedElement()
    {
        // Arrange: test result with Pending outcome
        var results = new TestResults
        {
            Name = "PendingTests",
            Results =
            [
                new TestResult
                {
                    Name = "PendingTest",
                    ClassName = "MyTestClass",
                    Duration = TimeSpan.Zero,
                    Outcome = TestOutcome.Pending,
                    ErrorMessage = "Test is pending"
                }
            ]
        };

        // Act: serialize the test results
        var xml = JUnitSerializer.Serialize(results);
        Assert.NotNull(xml);

        // Assert: skipped element is present with message and skipped counter is incremented
        var doc = XDocument.Parse(xml);
        var testSuite = doc.Root?.Element("testsuite");
        Assert.NotNull(testSuite);
        Assert.Equal("1", testSuite.Attribute("skipped")?.Value);
        var testCase = testSuite.Element("testcase");
        Assert.NotNull(testCase);
        var skipped = testCase.Element("skipped");
        Assert.NotNull(skipped);
        Assert.Equal("Test is pending", skipped.Attribute("message")?.Value);
    }

    /// <summary>
    ///     Test that JUnitSerializer serializes to a string with an XML declaration declaring UTF-8 encoding
    /// </summary>
    /// <remarks>
    ///     Proves that the Utf8StringWriter helper used internally by JUnitSerializer produces output
    ///     with an explicit <c>encoding="utf-8"</c> XML declaration, ensuring consuming tools
    ///     interpret the content correctly.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_IncludesXmlDeclarationWithUtf8Encoding()
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
        var xml = JUnitSerializer.Serialize(results);

        // Assert: XML declaration with UTF-8 encoding is present
        Assert.True(
            xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"", StringComparison.OrdinalIgnoreCase),
            "Expected XML declaration with encoding=\"utf-8\" at start of output");
    }

    /// <summary>
    ///     Test that an Inconclusive outcome is deserialized as Passed after a JUnit round-trip.
    /// </summary>
    /// <remarks>
    ///     Proves that JUnit XML has no dedicated inconclusive element; an Inconclusive test serializes
    ///     as a plain <c>testcase</c> element with no outcome child, which deserializes back as
    ///     <see cref="TestOutcome.Passed"/>. This round-trip loss is a known limitation of the JUnit
    ///     format.
    /// </remarks>
    [Fact]
    public void JUnitSerializer_Serialize_ThenDeserialize_InconclusiveOutcomeBecomesPass()
    {
        // Arrange: a single test result with an Inconclusive outcome
        var original = new TestResults
        {
            Name = "InconclusiveFidelityTest",
            Results =
            [
                new TestResult
                {
                    Name = "InconclusiveTest",
                    ClassName = "Suite.InconclusiveClass",
                    Outcome = TestOutcome.Inconclusive,
                    Duration = TimeSpan.FromSeconds(1.0)
                }
            ]
        };

        // Act: serialize to JUnit XML and then deserialize back
        var junitXml = JUnitSerializer.Serialize(original);
        var deserialized = JUnitSerializer.Deserialize(junitXml);

        // Assert: the Inconclusive outcome becomes Passed because JUnit has no inconclusive element
        Assert.NotNull(deserialized);
        Assert.Single(deserialized.Results);
        Assert.Equal(TestOutcome.Passed, deserialized.Results[0].Outcome);
    }
}
