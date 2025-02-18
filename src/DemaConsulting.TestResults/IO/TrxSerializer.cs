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

using System.Text;
using System.Xml.Linq;

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     TRX Serializer class
/// </summary>
public static class TrxSerializer
{
    /// <summary>
    ///     Namespace for TRX files
    /// </summary>
    private static readonly XNamespace TrxNamespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";

    /// <summary>
    ///     Serializes the TestSuites object to a TRX file
    /// </summary>
    /// <param name="suites">Test Suites</param>
    /// <returns>TRX file contents</returns>
    public static string Serialize(TestSuites suites)
    {
        // Construct the document
        var doc = new XDocument();

        // Construct the root element
        var root = new XElement(TrxNamespace + "TestRun",
            new XAttribute("id", suites.Id),
            new XAttribute("name", suites.Name),
            new XAttribute("runUser", suites.UserName));
        doc.Add(root);

        // Construct the results
        var results = new XElement(TrxNamespace + "Results");
        root.Add(results);
        foreach (var c in suites.Cases)
        {
            // Construct the result
            var result = new XElement(TrxNamespace + "UnitTestResult",
                new XAttribute("executionId", c.ExecutionId),
                new XAttribute("testId", c.TestId),
                new XAttribute("testName", c.Name),
                new XAttribute("computerName", c.ComputerName),
                new XAttribute("testType", "13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B"),
                new XAttribute("outcome", c.Outcome),
                new XAttribute("duration", c.Duration),
                new XAttribute("startTime", c.StartTime),
                new XAttribute("endTime", c.StartTime + TimeSpan.FromSeconds(c.Duration)),
                new XAttribute("testListId", "19431567-8539-422a-85D7-44EE4E166BDA"));
            results.Add(result);

            // Construct the output
            var output = new XElement(TrxNamespace + "Output");
            result.Add(output);

            // Construct the stdout output
            if (c.SystemOutput != string.Empty)
            {
                output.Add(
                    new XElement(TrxNamespace + "StdOut",
                        new XCData(c.SystemOutput)));
            }

            // Construct the stderr output
            if (c.SystemError != string.Empty)
            {
                output.Add(
                    new XElement(TrxNamespace + "StdOut",
                        new XCData(c.SystemOutput)));
            }

            // Construct the error 
            if (c.Error != null)
            {
                output.Add(
                    new XElement(TrxNamespace + "ErrorInfo",
                        new XElement(TrxNamespace + "Message",
                            new XCData(c.Error.Message)),
                        new XElement(TrxNamespace + "StackTrace",
                            new XCData(c.Error.StackTrace))));
            }
        }

        // Construct definitions
        var definitions = new XElement(TrxNamespace + "TestDefinitions");
        root.Add(definitions);
        foreach (var c in suites.Cases)
        {
            definitions.Add(
                new XElement(TrxNamespace + "UnitTest",
                    new XAttribute("name", c.Name),
                    new XAttribute("id", c.TestId),
                    new XElement(TrxNamespace + "Execution",
                        new XAttribute("id", c.ExecutionId)),
                    new XElement(TrxNamespace + "TestMethod",
                        new XAttribute("codeBase", c.CodeBase),
                        new XAttribute("className", c.ClassName),
                        new XAttribute("name", c.Name))));
        }

        // Construct the Test Entries
        var entries = new XElement(TrxNamespace + "TestEntries");
        root.Add(entries);
        foreach (var c in suites.Cases)
            entries.Add(
                new XElement(TrxNamespace + "TestEntry",
                    new XAttribute("testId", c.TestId),
                    new XAttribute("executionId", c.ExecutionId),
                    new XAttribute("testListId", "19431567-8539-422a-85D7-44EE4E166BDA")));

        // Construct the test lists
        root.Add(
            new XElement(TrxNamespace + "TestLists",
                new XElement(TrxNamespace + "TestList",
                    new XAttribute("name", "All Loaded Results"),
                    new XAttribute("id", "19431567-8539-422a-85D7-44EE4E166BDA"))));

        // Construct the summary
        root.Add(
            new XElement(
                TrxNamespace + "ResultSummary",
                new XAttribute("outcome", "Completed"),
                new XElement(
                    TrxNamespace + "Counters",
                    new XAttribute("total", suites.Cases.Count()),
                    new XAttribute("executed", suites.Cases.Count(c => c.Outcome != TestOutcome.Skipped)),
                    new XAttribute("passed", suites.Cases.Count(c => c.Outcome == TestOutcome.Passed)),
                    new XAttribute("failed", suites.Cases.Count(c => c.Outcome == TestOutcome.Failed)))));

        // Write the TRX text
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