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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

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
    ///     Serializes the TestResults object to a TRX file
    /// </summary>
    /// <param name="results">Test Results</param>
    /// <returns>TRX file contents</returns>
    public static string Serialize(TestResults results)
    {
        // Construct the document
        var doc = new XDocument();

        // Construct the root element
        var root = new XElement(TrxNamespace + "TestRun",
            new XAttribute("id", results.Id),
            new XAttribute("name", results.Name),
            new XAttribute("runUser", results.UserName));
        doc.Add(root);

        // Construct the results
        var resultsElement = new XElement(TrxNamespace + "Results");
        root.Add(resultsElement);
        foreach (var c in results.Results)
        {
            // Construct the result
            var resultElement = new XElement(TrxNamespace + "UnitTestResult",
                new XAttribute("executionId", c.ExecutionId),
                new XAttribute("testId", c.TestId),
                new XAttribute("testName", c.Name),
                new XAttribute("computerName", c.ComputerName),
                new XAttribute("testType", "13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B"),
                new XAttribute("outcome", c.Outcome),
                new XAttribute("duration", c.Duration.ToString("c")),
                new XAttribute("startTime", c.StartTime.ToString("o", CultureInfo.InvariantCulture)),
                new XAttribute("endTime", (c.StartTime + c.Duration).ToString("o", CultureInfo.InvariantCulture)),
                new XAttribute("testListId", "19431567-8539-422a-85D7-44EE4E166BDA"));
            resultsElement.Add(resultElement);

            // Construct the output element
            var outputElement = new XElement(TrxNamespace + "Output");
            resultElement.Add(outputElement);

            // Construct the stdout output
            if (!string.IsNullOrEmpty(c.SystemOutput))
            {
                outputElement.Add(
                    new XElement(TrxNamespace + "StdOut",
                        new XCData(c.SystemOutput)));
            }

            // Construct the stderr output
            if (!string.IsNullOrEmpty(c.SystemError))
            {
                outputElement.Add(
                    new XElement(TrxNamespace + "StdErr",
                        new XCData(c.SystemError)));
            }

            // Skip writing the error info element if there is no error information
            if (string.IsNullOrEmpty(c.ErrorMessage) &&
                string.IsNullOrEmpty(c.ErrorStackTrace))
            {
                continue;
            }

            // Construct the error info element
            var errorInfoElement = new XElement(TrxNamespace + "ErrorInfo");
            outputElement.Add(errorInfoElement);

            // Construct the error message
            if (!string.IsNullOrEmpty(c.ErrorMessage))
            {
                errorInfoElement.Add(
                    new XElement(TrxNamespace + "Message",
                        new XCData(c.ErrorMessage)));
            }

            // Construct the stack trace
            if (!string.IsNullOrEmpty(c.ErrorStackTrace))
            {
                errorInfoElement.Add(
                    new XElement(TrxNamespace + "StackTrace",
                        new XCData(c.ErrorStackTrace)));
            }
        }

        // Construct definitions
        var definitionsElement = new XElement(TrxNamespace + "TestDefinitions");
        root.Add(definitionsElement);
        foreach (var c in results.Results)
        {
            definitionsElement.Add(
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
        var entriesElement = new XElement(TrxNamespace + "TestEntries");
        root.Add(entriesElement);
        foreach (var c in results.Results)
            entriesElement.Add(
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
                    new XAttribute("total", results.Results.Count),
                    new XAttribute("executed", results.Results.Count(c => c.Outcome.IsExecuted())),
                    new XAttribute("passed", results.Results.Count(c => c.Outcome.IsPassed())),
                    new XAttribute("failed", results.Results.Count(c => c.Outcome.IsFailed())))));

        // Write the TRX text
        var writer = new Utf8StringWriter();
        doc.Save(writer);
        return writer.ToString();
    }

    /// <summary>
    ///     Deserializes a TRX file to a TestResults object
    /// </summary>
    /// <param name="trxContents">TRX File Contents</param>
    /// <returns>Test Results</returns>
    public static TestResults Deserialize(string trxContents)
    {
        // Parse the document
        var doc = XDocument.Parse(trxContents);
        var nsMgr = new XmlNamespaceManager(new NameTable());
        nsMgr.AddNamespace("trx", TrxNamespace.NamespaceName);

        // Construct the results
        var results = new TestResults();

        // Get the run element
        var runElement = doc.XPathSelectElement("/trx:TestRun", nsMgr) ??
                         throw new InvalidOperationException("Invalid TRX file");
        results.Id = Guid.Parse(runElement.Attribute("id")?.Value ?? Guid.NewGuid().ToString());
        results.Name = runElement.Attribute("name")?.Value ?? string.Empty;
        results.UserName = runElement.Attribute("runUser")?.Value ?? string.Empty;

        // Get the results
        var resultElements = doc.XPathSelectElements(
            "/trx:TestRun/trx:Results/trx:UnitTestResult",
            nsMgr);
        foreach (var resultElement in resultElements)
        {
            // Get the test ID
            var testId = resultElement.Attribute("testId") ??
                         throw new InvalidOperationException("Invalid TRX file");

            // Get the test method element
            var methodElement =
                doc.XPathSelectElement(
                    $"/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@id='{testId.Value}']/trx:TestMethod",
                    nsMgr) ??
                throw new InvalidOperationException("Invalid TRX File");

            // Get the output element
            var outputElement = resultElement.Element(TrxNamespace + "Output");

            // Get the errorInfo element
            var errorInfoElement = outputElement?.Element(TrxNamespace + "ErrorInfo");

            // Add the test result
            results.Results.Add(
                new TestResult
                {
                    TestId = Guid.Parse(testId.Value),
                    ExecutionId = Guid.Parse(
                        resultElement.Attribute("executionId")?.Value ?? Guid.NewGuid().ToString()),
                    Name = methodElement.Attribute("name")?.Value ?? string.Empty,
                    CodeBase = methodElement.Attribute("codeBase")?.Value ?? string.Empty,
                    ClassName = methodElement.Attribute("className")?.Value ?? string.Empty,
                    ComputerName = resultElement.Attribute("computerName")?.Value ?? string.Empty,
                    Outcome = Enum.Parse<TestOutcome>(resultElement.Attribute("outcome")?.Value ?? "Failed"),
                    StartTime = DateTime.Parse(
                        resultElement.Attribute("startTime")?.Value ??
                        DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal),
                    Duration = TimeSpan.Parse(
                        resultElement.Attribute("duration")?.Value ?? "0",
                        CultureInfo.InvariantCulture),
                    SystemOutput = outputElement
                        ?.Element(TrxNamespace + "StdOut")
                        ?.Value ?? string.Empty,
                    SystemError = outputElement
                        ?.Element(TrxNamespace + "StdErr")
                        ?.Value ?? string.Empty,
                    ErrorMessage = errorInfoElement
                        ?.Element(TrxNamespace + "Message")
                        ?.Value ?? string.Empty,
                    ErrorStackTrace = errorInfoElement
                        ?.Element(TrxNamespace + "StackTrace")
                        ?.Value ?? string.Empty
                });
        }

        // Return the results
        return results;
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
