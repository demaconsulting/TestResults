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
using System.Xml.Linq;

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     Serializes and deserializes test results in the JUnit XML format.
/// </summary>
/// <remarks>
///     JUnit XML is the de-facto standard format accepted by Jenkins, GitHub Actions test
///     reporters, GitLab CI, and many other CI systems. Use this class directly when you know
///     the format is JUnit. For auto-detected deserialization of unknown inputs, prefer
///     <see cref="Serializer.Deserialize"/> instead.
///     <para>
///         <b>Known round-trip losses:</b> <see cref="TestOutcome.Timeout"/> and
///         <see cref="TestOutcome.Aborted"/> both serialize as <c>error</c> elements and
///         deserialize back as <see cref="TestOutcome.Error"/>.
///         <see cref="TestOutcome.Inconclusive"/> serializes as a plain passing <c>testcase</c>
///         and deserializes back as <see cref="TestOutcome.Passed"/>. A
///         <see cref="TestResult.ClassName"/> of <c>DefaultSuite</c> deserializes as an
///         empty string.
///     </para>
///     <para>Both methods are stateless and safe for concurrent calls.</para>
/// </remarks>
/// <example>
///     Write a JUnit XML file:
///     <code>
///     var results = new TestResults { Name = "My Run" };
///     results.Results.Add(new TestResult { Name = "Test1", Outcome = TestOutcome.Passed });
///     string xml = JUnitSerializer.Serialize(results);
///     File.WriteAllText("results.xml", xml);
///     </code>
///     Read a JUnit XML file:
///     <code>
///     string xml = File.ReadAllText("results.xml");
///     TestResults results = JUnitSerializer.Deserialize(xml);
///     </code>
/// </example>
public static class JUnitSerializer
{
    /// <summary>
    ///     Default suite name for tests without a class name.
    /// </summary>
    /// <remarks>
    ///     Tests with an empty <see cref="TestResult.ClassName"/> are grouped under this name during
    ///     serialization, and the name is mapped back to an empty string during deserialization.
    ///     As a consequence, a test with <see cref="TestResult.ClassName"/> equal to "DefaultSuite"
    ///     cannot be round-tripped faithfully — it will be deserialized with an empty class name.
    /// </remarks>
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
    /// <remarks>
    ///     Groups test results by <see cref="TestResult.ClassName"/>, creating one
    ///     <c>testsuite</c> element per distinct class name. Tests with an empty class name are
    ///     grouped under the <c>DefaultSuite</c> sentinel name. Within each suite, outcomes are
    ///     mapped to JUnit child elements as follows: <see cref="TestOutcome.Failed"/> produces a
    ///     <c>failure</c> element; <see cref="TestOutcome.Error"/>, <see cref="TestOutcome.Timeout"/>,
    ///     and <see cref="TestOutcome.Aborted"/> produce an <c>error</c> element; outcomes where
    ///     <see cref="TestOutcomeExtensions.IsExecuted"/> returns <see langword="false"/> produce a
    ///     <c>skipped</c> element; all other outcomes produce a plain <c>testcase</c> element with no
    ///     outcome child. The <c>timestamp</c> attribute on each <c>testsuite</c> is set to the
    ///     earliest <see cref="TestResult.StartTime"/> across all tests in that suite. Stateless;
    ///     safe for concurrent calls.
    /// </remarks>
    /// <param name="results">Test Results</param>
    /// <returns>JUnit XML file contents</returns>
    /// <exception cref="ArgumentNullException">Thrown when results is null</exception>
    public static string Serialize(TestResults results)
    {
        // Validate input
        ArgumentNullException.ThrowIfNull(results);

        // Group test results by class name for test suites
        var testSuites = results.Results
            .GroupBy(r => r.ClassName)
            .OrderBy(g => g.Key);

        // Construct the root element
        var root = new XElement("testsuites",
            new XAttribute("name", results.Name));

        // Add test suites for each class
        root.Add(testSuites.Select(CreateTestSuiteElement));

        // Write the XML text
        var doc = new XDocument(root);
        using var writer = new Utf8StringWriter();
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
        var timestamp = suiteTests.Min(t => t.StartTime);

        var testSuite = new XElement("testsuite",
            new XAttribute("name", className),
            new XAttribute("tests", suiteTests.Count),
            new XAttribute("failures", suiteTests.Count(t => t.Outcome == TestOutcome.Failed)),
            new XAttribute("errors", suiteTests.Count(t => IsErrorOutcome(t.Outcome))),
            new XAttribute("skipped", suiteTests.Count(t => !t.Outcome.IsExecuted())),
            new XAttribute("time", suiteTests.Sum(t => t.Duration.TotalSeconds).ToString(TimeFormatString, CultureInfo.InvariantCulture)),
            new XAttribute("timestamp", timestamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)));

        // Add test cases
        testSuite.Add(suiteTests.Select(CreateTestCaseElement));

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
    /// <remarks>
    ///     Accepts both the common two-level structure (<c>testsuites</c> → <c>testsuite</c> →
    ///     <c>testcase</c>) and the bare single-level structure (<c>testsuite</c> → <c>testcase</c>)
    ///     that some JUnit producers emit. The run name is read from the root element's
    ///     <c>name</c> attribute in both cases. Known round-trip losses: <see cref="TestOutcome.Timeout"/>
    ///     and <see cref="TestOutcome.Aborted"/> both deserialize as <see cref="TestOutcome.Error"/>
    ///     because JUnit has no distinct timeout or aborted element; <see cref="TestOutcome.Inconclusive"/>
    ///     deserializes as <see cref="TestOutcome.Passed"/> because JUnit has no inconclusive element;
    ///     and a class name of <c>DefaultSuite</c> deserializes as an empty string. Stateless; safe
    ///     for concurrent calls.
    /// </remarks>
    /// <param name="junitContents">JUnit XML File Contents</param>
    /// <returns>Test Results</returns>
    /// <exception cref="ArgumentNullException">Thrown when junitContents is null</exception>
    /// <exception cref="ArgumentException">Thrown when junitContents is whitespace</exception>
    /// <exception cref="InvalidOperationException">Thrown when the XML structure is invalid</exception>
    public static TestResults Deserialize(string junitContents)
    {
        // Validate input
        ArgumentException.ThrowIfNullOrWhiteSpace(junitContents);

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
        // Read the optional timestamp attribute from the testsuite element
        var startTime = TryParseTimestamp(testSuiteElement.Attribute("timestamp")?.Value);

        var testCaseElements = testSuiteElement.Elements("testcase");
        results.Results.AddRange(testCaseElements.Select(e => ParseTestCase(e, startTime)));
    }

    /// <summary>
    ///     Tries to parse an ISO 8601 timestamp string to a UTC DateTime
    /// </summary>
    /// <param name="timestampStr">The timestamp string to parse, or null/empty if not available</param>
    /// <returns>The parsed UTC DateTime, or null if the string is absent or cannot be parsed</returns>
    private static DateTime? TryParseTimestamp(string? timestampStr)
    {
        if (string.IsNullOrWhiteSpace(timestampStr))
        {
            return null;
        }

        return DateTimeOffset.TryParse(
            timestampStr,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var parsedTimestamp)
            ? parsedTimestamp.UtcDateTime
            : null;
    }

    /// <summary>
    ///     Parses a test case element
    /// </summary>
    /// <param name="testCaseElement">The testcase XML element to parse</param>
    /// <param name="startTime">The start time from the enclosing testsuite element, or null if not available</param>
    /// <returns>A TestResult object populated with data from the XML element</returns>
    private static TestResult ParseTestCase(XElement testCaseElement, DateTime? startTime)
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
        var result = new TestResult
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

        // Apply start time from testsuite if available
        if (startTime.HasValue)
        {
            result.StartTime = startTime.Value;
        }

        return result;
    }

    /// <summary>
    ///     Parses a duration string to a TimeSpan
    /// </summary>
    /// <param name="timeStr">The time string to parse, representing seconds as a decimal number</param>
    /// <returns>A TimeSpan representing the duration, or TimeSpan.Zero if parsing fails</returns>
    private static TimeSpan ParseDuration(string? timeStr)
    {
        if (string.IsNullOrEmpty(timeStr))
        {
            return TimeSpan.Zero;
        }

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

}
