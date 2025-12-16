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
            var className = string.IsNullOrEmpty(suiteGroup.Key) ? "DefaultSuite" : suiteGroup.Key;
            var suiteTests = suiteGroup.ToList();

            var testSuite = new XElement("testsuite",
                new XAttribute("name", className),
                new XAttribute("tests", suiteTests.Count),
                new XAttribute("failures", suiteTests.Count(t => t.Outcome == TestOutcome.Failed)),
                new XAttribute("errors", suiteTests.Count(t => t.Outcome == TestOutcome.Error || t.Outcome == TestOutcome.Timeout || t.Outcome == TestOutcome.Aborted)),
                new XAttribute("skipped", suiteTests.Count(t => !t.Outcome.IsExecuted())),
                new XAttribute("time", suiteTests.Sum(t => t.Duration.TotalSeconds).ToString("F3", CultureInfo.InvariantCulture)));

            // Add test cases
            foreach (var test in suiteTests)
            {
                var testCase = new XElement("testcase",
                    new XAttribute("name", test.Name),
                    new XAttribute("classname", string.IsNullOrEmpty(test.ClassName) ? "DefaultSuite" : test.ClassName),
                    new XAttribute("time", test.Duration.TotalSeconds.ToString("F3", CultureInfo.InvariantCulture)));

                // Add failure or error information
                if (test.Outcome == TestOutcome.Failed)
                {
                    var failure = new XElement("failure");
                    if (!string.IsNullOrEmpty(test.ErrorMessage))
                    {
                        failure.Add(new XAttribute("message", test.ErrorMessage));
                    }
                    if (!string.IsNullOrEmpty(test.ErrorStackTrace))
                    {
                        failure.Add(new XCData(test.ErrorStackTrace));
                    }
                    testCase.Add(failure);
                }
                else if (test.Outcome == TestOutcome.Error || test.Outcome == TestOutcome.Timeout || test.Outcome == TestOutcome.Aborted)
                {
                    var error = new XElement("error");
                    if (!string.IsNullOrEmpty(test.ErrorMessage))
                    {
                        error.Add(new XAttribute("message", test.ErrorMessage));
                    }
                    if (!string.IsNullOrEmpty(test.ErrorStackTrace))
                    {
                        error.Add(new XCData(test.ErrorStackTrace));
                    }
                    testCase.Add(error);
                }
                else if (!test.Outcome.IsExecuted())
                {
                    var skipped = new XElement("skipped");
                    if (!string.IsNullOrEmpty(test.ErrorMessage))
                    {
                        skipped.Add(new XAttribute("message", test.ErrorMessage));
                    }
                    testCase.Add(skipped);
                }

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

                testSuite.Add(testCase);
            }

            root.Add(testSuite);
        }

        // Write the XML text
        var doc = new XDocument(root);
        var writer = new Utf8StringWriter();
        doc.Save(writer);
        return writer.ToString();
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
