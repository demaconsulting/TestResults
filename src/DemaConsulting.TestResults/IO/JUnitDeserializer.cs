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
using System.Xml.Linq;

namespace DemaConsulting.TestResults.IO;

/// <summary>
///    Deserializer for JUnit XML test results.
/// </summary>
public static class JUnitDeserializer
{
    /// <summary>
    ///     Deserializes the JUnit XML test results.
    /// </summary>
    /// <param name="xml">JUnit XML text</param>
    /// <returns>Deserialized TestRun</returns>
    public static TestRun Deserialize(string xml)
    {
        // Parse the document
        var doc = XDocument.Parse(xml);

        // Get the root element
        var root = doc.Root ?? throw new InvalidOperationException("Invalid JUnit XML");

        // Deserialize the TestRun from the root element
        return DeserializeRun(root);
    }

    /// <summary>
    ///     Deserializes a JUnit XML.
    /// </summary>
    /// <param name="element">JUnit XML Element (test-suites or single test-suite)</param>
    /// <returns>Deserialized TestRun</returns>
    public static TestRun DeserializeRun(XElement element)
    {
        // Support JUnit XML consisting of a single test-suite node
        if (element.Name.LocalName == "testsuite")
        {
            var suite = DeserializeTestSuite(element);
            return new TestRun { Suites = [suite] };
        }

        // Deserialize the test-suites into a TestRun
        return new TestRun
        {
            Name = element.Attribute("name")?.Value,
            TimeStamp = ParseDateTime(element.Attribute("timestamp")?.Value),
            Duration = ParseDouble(element.Attribute("time")?.Value),
            Suites = DeserializeTestSuiteList(element)
        };
    }

    /// <summary>
    ///     Deserializes a list of TestSuite.
    /// </summary>
    /// <param name="parent">Parent element</param>
    /// <returns>List of TestSuite</returns>
    public static List<TestSuite> DeserializeTestSuiteList(XElement parent)
    {
        return parent.Elements("testsuite").Select(DeserializeTestSuite).ToList();
    }

    /// <summary>
    ///     Deserializes a TestSuite.
    /// </summary>
    /// <param name="element">JUnit XML test-suite element</param>
    /// <returns>Deserialized TestSuite</returns>
    public static TestSuite DeserializeTestSuite(XElement element)
    {
        return new TestSuite
        {
            Name = element.Attribute("name")?.Value,
            SourceFile = element.Attribute("file")?.Value,
            TimeStamp = ParseDateTime(element.Attribute("timestamp")?.Value),
            Duration = ParseDouble(element.Attribute("time")?.Value),
            HostName = element.Attribute("hostname")?.Value,
            Id = element.Attribute("id")?.Value,
            StdOut = DeserializeTextArray(element.Elements("system-out")),
            StdErr = DeserializeTextArray(element.Elements("system-err")),
            Cases = DeserializeTestCaseList(element)
        };
    }

    /// <summary>
    ///     Deserializes a list of TestCase.
    /// </summary>
    /// <param name="parent">JUnit XML test-suite element</param>
    /// <returns>List of TestCase</returns>
    public static List<TestCase> DeserializeTestCaseList(XElement parent)
    {
        return parent.Elements("testcase").Select(DeserializeTestCase).ToList();
    }

    /// <summary>
    ///     Deserializes a TestCase.
    /// </summary>
    /// <param name="element">JUnit XML test-case element</param>
    /// <returns>Deserialized TestCase</returns>
    public static TestCase DeserializeTestCase(XElement element)
    {
        return new TestCase
        {
            Name = element.Attribute("name")?.Value,
            ClassName = element.Attribute("classname")?.Value,
            SourceFile = element.Attribute("file")?.Value,
            LineNumber = ParseInt(element.Attribute("line")?.Value),
            Assertions = ParseInt(element.Attribute("assertions")?.Value),
            TimeStamp = ParseDateTime(element.Attribute("timestamp")?.Value),
            Duration = ParseDouble(element.Attribute("time")?.Value),
            Skipped = DeserializeSkipped(element),
            Failures = DeserializeFailureArray(element),
            Errors = DeserializeErrorArray(element),
            StdOut = DeserializeTextArray(element.Elements("system-out")),
            StdErr = DeserializeTextArray(element.Elements("system-err"))
        };
    }

    /// <summary>
    ///     Deserializes the skipped reason from a test-case element.
    /// </summary>
    /// <param name="element">JUnit XML test-case element</param>
    /// <returns>Skipped reason or null</returns>
    public static string? DeserializeSkipped(XElement element)
    {
        var skipped = element.Element("skipped");
        return skipped?.Value;
    }

    /// <summary>
    ///     Deserializes a list of TestFailure.
    /// </summary>
    /// <param name="parent">JUnit XML test-case element</param>
    /// <returns>List of TestFailure</returns>
    public static TestFailure[] DeserializeFailureArray(XElement parent)
    {
        return parent.Elements("failure").Select(DeserializeFailure).ToArray();
    }

    /// <summary>
    ///     Deserializes a TestFailure.
    /// </summary>
    /// <param name="element">JUnit XML test-failure element</param>
    /// <returns>Deserialized TestFailure</returns>
    public static TestFailure DeserializeFailure(XElement element)
    {
        return new TestFailure
        {
            Message = element.Attribute("message")?.Value,
            Type = element.Attribute("type")?.Value,
            Description = element.Value
        };
    }

    /// <summary>
    ///     Deserializes a list of TestError.
    /// </summary>
    /// <param name="parent">JUnit XML test-case element</param>
    /// <returns>List of TestError</returns>
    public static TestError[] DeserializeErrorArray(XElement parent)
    {
        return parent.Elements("error").Select(DeserializeError).ToArray();
    }

    /// <summary>
    ///     Deserializes a TestError.
    /// </summary>
    /// <param name="element">JUnit XML test-error element</param>
    /// <returns>Deserialized TestError</returns>
    public static TestError DeserializeError(XElement element)
    {
        return new TestError
        {
            Message = element.Attribute("message")?.Value,
            Type = element.Attribute("type")?.Value,
            Description = element.Value
        };
    }

    /// <summary>
    ///     Deserializes a list of text elements.
    /// </summary>
    /// <param name="elements">Text elements</param>
    /// <returns>Array of strings</returns>
    public static string[] DeserializeTextArray(IEnumerable<XElement> elements)
    {
        return elements.Select(e => e.Value).ToArray();
    }

    /// <summary>
    ///     Parses an optional integer from an optional string.
    /// </summary>
    /// <param name="value">Optional string</param>
    /// <returns>Optional integer</returns>
    private static int? ParseInt(string? value)
    {
        return value == null ? null : int.Parse(value);
    }

    /// <summary>
    ///     Parses an optional double from an optional string.
    /// </summary>
    /// <param name="value">Optional string</param>
    /// <returns>Optional double</returns>
    private static double? ParseDouble(string? value)
    {
        return value == null ? null : double.Parse(value);
    }

    /// <summary>
    ///     Parses an optional DateTime from an optional string.
    /// </summary>
    /// <param name="value">Optional string</param>
    /// <returns>Optional DateTime</returns>
    private static DateTime? ParseDateTime(string? value)
    {
        return value == null ? null : DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }
}