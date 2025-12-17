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

using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     JUnit Serializer class
/// </summary>
public static class JUnitSerializer
{
    /// <summary>
    ///     Default suite name for tests without a class name
    /// </summary>
    private const string DefaultSuiteName = "DefaultSuite";

    /// <summary>
    ///     Format string for time values in seconds with 3 decimal places
    /// </summary>
    private const string TimeFormatString = "F3";

    /// <summary>
    ///     Error message for invalid JUnit XML file
    /// </summary>
    private const string InvalidJUnitFileMessage = "Invalid JUnit XML file";

    /// <summary>
    ///     Attribute name for message in XML elements
    /// </summary>
    private const string MessageAttributeName = "message";

    /// <summary>
    ///     Serializes the TestResults object to a JUnit XML file
    /// </summary>
    /// <param name="results">Test Results</param>
    /// <returns>JUnit XML file contents</returns>
    public static string Serialize(TestResults results)
    {
        // Group test results by class name for test suites
        var testSuites = results.Results
            .GroupBy(r => r.ClassName)
            .OrderBy(g => g.Key);

        // Construct the root element
        var root = new XElement("testsuites",
            new XAttribute("name", results.Name));

        // Add test suites for each class
        foreach (var suiteGroup in testSuites)
        {
            var testSuite = CreateTestSuiteElement(suiteGroup);
            root.Add(testSuite);
        }

        // Write the XML text
        var doc = new XDocument(root);
        var writer = new Utf8StringWriter();
        doc.Save(writer);
        return writer.ToString();
    }

    /// <summary>
    ///     Creates a test suite element for a group of tests
    /// </summary>
    /// <param name="suiteGroup">A grouping of test results by class name</param>
    /// <returns>An XElement representing a testsuite with all test cases and statistics</returns>
    private static XElement CreateTestSuiteElement(IGrouping<string, TestResult> suiteGroup)
    {
        var className = string.IsNullOrEmpty(suiteGroup.Key) ? DefaultSuiteName : suiteGroup.Key;
        var suiteTests = suiteGroup.ToList();

        var testSuite = new XElement("testsuite",
            new XAttribute("name", className),
            new XAttribute("tests", suiteTests.Count),
            new XAttribute("failures", suiteTests.Count(t => t.Outcome == TestOutcome.Failed)),
            new XAttribute("errors", suiteTests.Count(t => IsErrorOutcome(t.Outcome))),
            new XAttribute("skipped", suiteTests.Count(t => !t.Outcome.IsExecuted())),
            new XAttribute("time", suiteTests.Sum(t => t.Duration.TotalSeconds).ToString(TimeFormatString, CultureInfo.InvariantCulture)));

        // Add test cases
        foreach (var test in suiteTests)
        {
            var testCase = CreateTestCaseElement(test);
            testSuite.Add(testCase);
        }

        return testSuite;
    }

    /// <summary>
    ///     Determines if an outcome represents an error condition
    /// </summary>
    /// <param name="outcome">The test outcome to evaluate</param>
    /// <returns>True if the outcome is Error, Timeout, or Aborted; otherwise false</returns>
    private static bool IsErrorOutcome(TestOutcome outcome)
    {
        return outcome == TestOutcome.Error || outcome == TestOutcome.Timeout || outcome == TestOutcome.Aborted;
    }

    /// <summary>
    ///     Creates a test case element
    /// </summary>
    /// <param name="test">The test result to serialize</param>
    /// <returns>An XElement representing a testcase with outcome, system output, and system error</returns>
    private static XElement CreateTestCaseElement(TestResult test)
    {
        var testCase = new XElement("testcase",
            new XAttribute("name", test.Name),
            new XAttribute("classname", string.IsNullOrEmpty(test.ClassName) ? DefaultSuiteName : test.ClassName),
            new XAttribute("time", test.Duration.TotalSeconds.ToString(TimeFormatString, CultureInfo.InvariantCulture)));

        // Add failure or error information based on outcome
        AddOutcomeElement(testCase, test);

        // Add system output
        if (!string.IsNullOrEmpty(test.SystemOutput))
        {
            testCase.Add(new XElement("system-out", new XCData(test.SystemOutput)));
        }

        // Add system error
        if (!string.IsNullOrEmpty(test.SystemError))
        {
            testCase.Add(new XElement("system-err", new XCData(test.SystemError)));
        }

        return testCase;
    }

    /// <summary>
    ///     Adds the appropriate outcome element (failure, error, or skipped) to a test case
    /// </summary>
    /// <param name="testCase">The testcase element to add the outcome element to</param>
    /// <param name="test">The test result containing the outcome information</param>
    private static void AddOutcomeElement(XElement testCase, TestResult test)
    {
        if (test.Outcome == TestOutcome.Failed)
        {
            var failure = CreateFailureOrErrorElement("failure", test);
            testCase.Add(failure);
        }
        else if (IsErrorOutcome(test.Outcome))
        {
            var error = CreateFailureOrErrorElement("error", test);
            testCase.Add(error);
        }
        else if (!test.Outcome.IsExecuted())
        {
            var skipped = new XElement("skipped");
            if (!string.IsNullOrEmpty(test.ErrorMessage))
            {
                skipped.Add(new XAttribute(MessageAttributeName, test.ErrorMessage));
            }
            testCase.Add(skipped);
        }
    }

    /// <summary>
    ///     Creates a failure or error element with message and stack trace
    /// </summary>
    /// <param name="elementName">The name of the element to create ("failure" or "error")</param>
    /// <param name="test">The test result containing error message and stack trace</param>
    /// <returns>An XElement with the specified name containing message attribute and stack trace content</returns>
    private static XElement CreateFailureOrErrorElement(string elementName, TestResult test)
    {
        var element = new XElement(elementName);

        if (!string.IsNullOrEmpty(test.ErrorMessage))
        {
            element.Add(new XAttribute(MessageAttributeName, test.ErrorMessage));
        }

        if (!string.IsNullOrEmpty(test.ErrorStackTrace))
        {
            element.Add(new XCData(test.ErrorStackTrace));
        }

        return element;
    }

    /// <summary>
    ///     Deserializes a JUnit XML file to a TestResults object
    /// </summary>
    /// <param name="junitContents">JUnit XML File Contents</param>
    /// <returns>Test Results</returns>
    public static TestResults Deserialize(string junitContents)
    {
        // Parse the document
        var doc = XDocument.Parse(junitContents);

        // Construct the results
        var results = new TestResults();

        // Get the root element (testsuites)
        var rootElement = doc.Root ?? throw new InvalidOperationException(InvalidJUnitFileMessage);

        // Get the test suite name (from testsuites or first testsuite)
        results.Name = rootElement.Attribute("name")?.Value ?? string.Empty;

        // Handle both testsuites (with nested testsuite) and single testsuite root
        var testSuiteElements = rootElement.Name.LocalName == "testsuites"
            ? rootElement.Elements("testsuite")
            : new[] { rootElement };

        // Process each test suite
        foreach (var testSuiteElement in testSuiteElements)
        {
            ParseTestSuite(testSuiteElement, results);
        }

        return results;
    }

    /// <summary>
    ///     Parses a test suite element and adds test cases to results
    /// </summary>
    /// <param name="testSuiteElement">The testsuite XML element to parse</param>
    /// <param name="results">The TestResults object to populate with test case data</param>
    private static void ParseTestSuite(XElement testSuiteElement, TestResults results)
    {
        var testCaseElements = testSuiteElement.Elements("testcase");

        foreach (var testCaseElement in testCaseElements)
        {
            var testResult = ParseTestCase(testCaseElement);
            results.Results.Add(testResult);
        }
    }

    /// <summary>
    ///     Parses a test case element
    /// </summary>
    /// <param name="testCaseElement">The testcase XML element to parse</param>
    /// <returns>A TestResult object populated with data from the XML element</returns>
    private static TestResult ParseTestCase(XElement testCaseElement)
    {
        // Parse basic test case attributes
        var name = testCaseElement.Attribute("name")?.Value ?? string.Empty;
        var className = testCaseElement.Attribute("classname")?.Value ?? string.Empty;
        var duration = ParseDuration(testCaseElement.Attribute("time")?.Value);

        // Determine outcome and error information
        var (outcome, errorMessage, errorStackTrace) = ParseOutcome(testCaseElement);

        // Get system output and error
        var systemOutput = testCaseElement.Element("system-out")?.Value ?? string.Empty;
        var systemError = testCaseElement.Element("system-err")?.Value ?? string.Empty;

        // Create test result
        return new TestResult
        {
            Name = name,
            ClassName = className == DefaultSuiteName ? string.Empty : className,
            Duration = duration,
            Outcome = outcome,
            ErrorMessage = errorMessage,
            ErrorStackTrace = errorStackTrace,
            SystemOutput = systemOutput,
            SystemError = systemError
        };
    }

    /// <summary>
    ///     Parses a duration string to a TimeSpan
    /// </summary>
    /// <param name="timeStr">The time string to parse, representing seconds as a decimal number</param>
    /// <returns>A TimeSpan representing the duration, or TimeSpan.Zero if parsing fails</returns>
    private static TimeSpan ParseDuration(string? timeStr)
    {
        if (string.IsNullOrEmpty(timeStr))
            return TimeSpan.Zero;

        return double.TryParse(timeStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var timeValue)
            ? TimeSpan.FromSeconds(timeValue)
            : TimeSpan.Zero;
    }

    /// <summary>
    ///     Parses outcome information from test case child elements
    /// </summary>
    /// <param name="testCaseElement">The testcase XML element to parse for outcome information</param>
    /// <returns>A tuple containing the test outcome, error message, and error stack trace</returns>
    private static (TestOutcome outcome, string errorMessage, string errorStackTrace) ParseOutcome(XElement testCaseElement)
    {
        var failureElement = testCaseElement.Element("failure");
        var errorElement = testCaseElement.Element("error");
        var skippedElement = testCaseElement.Element("skipped");

        if (failureElement != null)
        {
            return (
                TestOutcome.Failed,
                failureElement.Attribute(MessageAttributeName)?.Value ?? string.Empty,
                failureElement.Value
            );
        }

        if (errorElement != null)
        {
            return (
                TestOutcome.Error,
                errorElement.Attribute(MessageAttributeName)?.Value ?? string.Empty,
                errorElement.Value
            );
        }

        if (skippedElement != null)
        {
            return (
                TestOutcome.NotExecuted,
                skippedElement.Attribute(MessageAttributeName)?.Value ?? string.Empty,
                string.Empty
            );
        }

        return (TestOutcome.Passed, string.Empty, string.Empty);
    }

    /// <summary>
    ///     String writer that uses UTF-8 encoding
    /// </summary>
    private sealed class Utf8StringWriter : StringWriter
    {
        /// <summary>
        ///     Gets the UTF-8 encoding
        /// </summary>
        public override Encoding Encoding => Encoding.UTF8;
    }
}
