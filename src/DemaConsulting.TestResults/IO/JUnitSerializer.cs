// Copyright(c) 2024 DEMA Consulting
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
using System.Xml;
using System.Xml.Linq;

namespace DemaConsulting.TestResults.IO;

/// <summary>
///    Serializer for JUnit XML test results.
/// </summary>
public static class JUnitSerializer
{
    /// <summary>
    ///     Serializes the test run to JUnit XML.
    /// </summary>
    /// <param name="testRun">Test run</param>
    /// <returns>JUnit XML text</returns>
    public static string Serialize(TestRun testRun)
    {
        // Serialize the run
        var xml = SerializeRun(testRun);

        // Serialize the XML to a string
        using var ms = new MemoryStream();
        using (var xw = XmlWriter.Create(ms, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
        {
            xml.Save(xw);
        }

        // Return the string
        return Encoding.UTF8.GetString(ms.ToArray());
    }

    /// <summary>
    ///     Serializes the test run to JUnit XML.
    /// </summary>
    /// <param name="testRun">Test run</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeRun(TestRun testRun)
    {
        // Construct the test-suites element
        var element = new XElement("testsuites");

        // Add the optional name attribute
        if (testRun.Name != null)
            element.Add(new XAttribute("name", testRun.Name));

        // Add the statistics
        element.Add(new XAttribute("tests", testRun.GetTests()));
        element.Add(new XAttribute("failures", testRun.GetFailures()));
        element.Add(new XAttribute("errors", testRun.GetErrors()));
        element.Add(new XAttribute("skipped", testRun.GetSkipped()));

        // Add the optional time-stamp attribute
        if (testRun.TimeStamp != null)
            element.Add(new XAttribute("timestamp", ToIso8601(testRun.TimeStamp.Value)));

        // Add the optional duration attribute
        if (testRun.Duration != null)
            element.Add(new XAttribute("time", testRun.Duration.Value.ToString(CultureInfo.InvariantCulture)));

        // Add the test suites
        foreach (var suite in testRun.Suites)
            element.Add(SerializeTestSuite(suite));

        // Return the element
        return element;
    }

    /// <summary>
    ///     Serializes the test suite to JUnit XML.
    /// </summary>
    /// <param name="testSuite">Test suite</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeTestSuite(TestSuite testSuite)
    {
        // Construct the test-suite element
        var element = new XElement("testsuite");

        // Add the name attribute
        if (testSuite.Name != null)
            element.Add(new XAttribute("name", testSuite.Name));

        // Add the statistics
        element.Add(new XAttribute("tests", testSuite.GetTests()));
        element.Add(new XAttribute("failures", testSuite.GetFailures()));
        element.Add(new XAttribute("errors", testSuite.GetErrors()));
        element.Add(new XAttribute("skipped", testSuite.GetSkipped()));

        // Add the optional time-stamp attribute
        if (testSuite.TimeStamp != null)
            element.Add(new XAttribute("timestamp", ToIso8601(testSuite.TimeStamp.Value)));

        // Add the optional duration attribute
        if (testSuite.Duration != null)
            element.Add(new XAttribute("time", testSuite.Duration.Value.ToString(CultureInfo.InvariantCulture)));

        // Add the optional host-name attribute
        if (testSuite.HostName != null)
            element.Add(new XAttribute("hostname", testSuite.HostName));

        // Add the optional id attribute
        if (testSuite.Id != null)
            element.Add(new XAttribute("id", testSuite.Id));

        // Add the optional system-out elements
        foreach (var line in testSuite.StdOut)
            element.Add(SerializeText("system-out", line));

        // Add the optional system-err elements
        foreach (var line in testSuite.StdErr)
            element.Add(SerializeText("system-err", line));

        // Add the test cases
        foreach (var testCase in testSuite.Cases)
            element.Add(SerializeTestCase(testCase));

        // Return the test suite
        return element;
    }

    /// <summary>
    ///     Serializes the test case to JUnit XML.
    /// </summary>
    /// <param name="testCase">Test case</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeTestCase(TestCase testCase)
    {
        // Construct the test-case element
        var element = new XElement("testcase");

        // Add the name attribute
        if (testCase.Name != null)
            element.Add(new XAttribute("name", testCase.Name));

        // Add the optional assertions attribute
        if (testCase.Assertions != null)
            element.Add(new XAttribute("assertions", testCase.Assertions.Value));

        // Add the optional time-stamp attribute
        if (testCase.TimeStamp != null)
            element.Add(new XAttribute("timestamp", ToIso8601(testCase.TimeStamp.Value)));

        // Add the optional duration attribute
        if (testCase.Duration != null)
            element.Add(new XAttribute("time", testCase.Duration.Value.ToString(CultureInfo.InvariantCulture)));

        // Add the optional skipped flag
        if (testCase.Skipped != null)
            element.Add(new XElement("skipped", testCase.Skipped));

        // Add the failures
        foreach (var failure in testCase.Failures)
            element.Add(SerializeFailure(failure));

        // Add the errors
        foreach (var error in testCase.Errors)
            element.Add(SerializeError(error));

        // Ensure the outcome is encoded
        var outcome = testCase.GetOutcome();
        switch (outcome)
        {
            case TestOutcome.Ignored:
                // The test was skipped, but without reason. Add a skipped element
                if (testCase.Skipped == null)
                    element.Add(new XElement("skipped"));
                break;

            case TestOutcome.Failed:
                // The test failed, but without a record. Add a failed element
                if (testCase.Failures.Length == 0)
                    element.Add(new XElement("failure"));
                break;

            case TestOutcome.Error:
                // The test had an error, but without a record. Add an error element
                if (testCase.Errors.Length == 0)
                    element.Add(new XElement("error"));
                break;

            case TestOutcome.Passed:
                // The test passed. No action required.
                break;

            default:
                // The test had an unexpected outcome not covered by the standard. Convert to an error.
                element.Add(new XElement("error", $"Unsupported outcome {outcome}"));
                break;
        }

        // Add the optional system-out elements
        foreach (var line in testCase.StdOut)
            element.Add(SerializeText("system-out", line));

        // Add the optional system-err elements
        foreach (var line in testCase.StdErr)
            element.Add(SerializeText("system-err", line));

        // Return the element
        return element;
    }

    /// <summary>
    ///     Serializes the test failure to JUnit XML.
    /// </summary>
    /// <param name="failure">Test failure</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeFailure(TestFailure failure)
    {
        // Construct the failure element
        var element = new XElement("failure");

        // Add the optional message attribute
        if (failure.Message != null)
            element.Add(new XAttribute("message", failure.Message));

        // Add the optional type attribute
        if (failure.Type != null)
            element.Add(new XAttribute("type", failure.Type));

        // Add the description
        if (failure.Description != null)
            element.Value = failure.Description;

        // Return the element
        return element;
    }

    /// <summary>
    ///     Serializes the test error to JUnit XML.
    /// </summary>
    /// <param name="error">Test error</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeError(TestError error)
    {
        // Construct the failure element
        var element = new XElement("error");

        // Add the optional message attribute
        if (error.Message != null)
            element.Add(new XAttribute("message", error.Message));

        // Add the optional type attribute
        if (error.Type != null)
            element.Add(new XAttribute("type", error.Type));

        // Add the description
        if (error.Description != null)
            element.Value = error.Description;

        // Return the element
        return element;
    }

    /// <summary>
    ///    Serializes a text element.
    /// </summary>
    /// <param name="name">Element name</param>
    /// <param name="value">Text value</param>
    /// <returns>JUnit XML element</returns>
    public static XElement SerializeText(string name, string value)
    {
        var element = new XElement(name);
        if (value.Contains('\n'))
        {
            element.Add(new XCData(value));
        }
        else
        {
            element.Value = value;
        }
        return element;
    }

    /// <summary>
    ///    Emits the provided DateTime as an ISO-8601 formatted string
    /// </summary>
    /// <param name="time">DateTime</param>
    /// <returns>ISO-8601 formatted string</returns>
    private static string ToIso8601(DateTime time)
    {
        return time.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}