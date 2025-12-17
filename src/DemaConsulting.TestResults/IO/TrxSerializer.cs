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
    ///     Standard test type GUID for unit tests
    /// </summary>
    private const string TestTypeGuid = "13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B";

    /// <summary>
    ///     Standard test list ID for all loaded results
    /// </summary>
    private const string TestListId = "19431567-8539-422a-85D7-44EE4E166BDA";

    /// <summary>
    ///     Name for the standard test list
    /// </summary>
    private const string TestListName = "All Loaded Results";

    /// <summary>
    ///     Error message for invalid TRX file
    /// </summary>
    private const string InvalidTrxFileMessage = "Invalid TRX file";

    /// <summary>
    ///     Date/time format string for invariant culture
    /// </summary>
    private const string DateTimeFormatString = "o";

    /// <summary>
    ///     Duration format string for TimeSpan
    /// </summary>
    private const string DurationFormatString = "c";

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
        var root = CreateRootElement(results);
        doc.Add(root);

        // Add results section
        var resultsElement = CreateResultsElement(results.Results);
        root.Add(resultsElement);

        // Add definitions section
        var definitionsElement = CreateDefinitionsElement(results.Results);
        root.Add(definitionsElement);

        // Add test entries section
        var entriesElement = CreateTestEntriesElement(results.Results);
        root.Add(entriesElement);

        // Add test lists section
        var testListsElement = CreateTestListsElement();
        root.Add(testListsElement);

        // Add summary section
        var summaryElement = CreateSummaryElement(results.Results);
        root.Add(summaryElement);

        // Write the TRX text
        var writer = new Utf8StringWriter();
        doc.Save(writer);
        return writer.ToString();
    }

    /// <summary>
    ///     Creates the root TestRun element
    /// </summary>
    private static XElement CreateRootElement(TestResults results)
    {
        return new XElement(TrxNamespace + "TestRun",
            new XAttribute("id", results.Id),
            new XAttribute("name", results.Name),
            new XAttribute("runUser", results.UserName));
    }

    /// <summary>
    ///     Creates the Results section with all test results
    /// </summary>
    private static XElement CreateResultsElement(IList<TestResult> testResults)
    {
        var resultsElement = new XElement(TrxNamespace + "Results");

        foreach (var test in testResults)
        {
            var resultElement = CreateUnitTestResultElement(test);
            resultsElement.Add(resultElement);
        }

        return resultsElement;
    }

    /// <summary>
    ///     Creates a single UnitTestResult element
    /// </summary>
    private static XElement CreateUnitTestResultElement(TestResult test)
    {
        var resultElement = new XElement(TrxNamespace + "UnitTestResult",
            new XAttribute("executionId", test.ExecutionId),
            new XAttribute("testId", test.TestId),
            new XAttribute("testName", test.Name),
            new XAttribute("computerName", test.ComputerName),
            new XAttribute("testType", TestTypeGuid),
            new XAttribute("outcome", test.Outcome),
            new XAttribute("duration", test.Duration.ToString(DurationFormatString)),
            new XAttribute("startTime", test.StartTime.ToString(DateTimeFormatString, CultureInfo.InvariantCulture)),
            new XAttribute("endTime", (test.StartTime + test.Duration).ToString(DateTimeFormatString, CultureInfo.InvariantCulture)),
            new XAttribute("testListId", TestListId));

        var outputElement = CreateOutputElement(test);
        resultElement.Add(outputElement);

        return resultElement;
    }

    /// <summary>
    ///     Creates the Output element with stdout, stderr, and error information
    /// </summary>
    private static XElement CreateOutputElement(TestResult test)
    {
        var outputElement = new XElement(TrxNamespace + "Output");

        // Add stdout if present
        if (!string.IsNullOrEmpty(test.SystemOutput))
        {
            outputElement.Add(
                new XElement(TrxNamespace + "StdOut",
                    new XCData(test.SystemOutput)));
        }

        // Add stderr if present
        if (!string.IsNullOrEmpty(test.SystemError))
        {
            outputElement.Add(
                new XElement(TrxNamespace + "StdErr",
                    new XCData(test.SystemError)));
        }

        // Add error info if present
        if (!string.IsNullOrEmpty(test.ErrorMessage) || !string.IsNullOrEmpty(test.ErrorStackTrace))
        {
            var errorInfoElement = CreateErrorInfoElement(test);
            outputElement.Add(errorInfoElement);
        }

        return outputElement;
    }

    /// <summary>
    ///     Creates the ErrorInfo element with message and stack trace
    /// </summary>
    private static XElement CreateErrorInfoElement(TestResult test)
    {
        var errorInfoElement = new XElement(TrxNamespace + "ErrorInfo");

        if (!string.IsNullOrEmpty(test.ErrorMessage))
        {
            errorInfoElement.Add(
                new XElement(TrxNamespace + "Message",
                    new XCData(test.ErrorMessage)));
        }

        if (!string.IsNullOrEmpty(test.ErrorStackTrace))
        {
            errorInfoElement.Add(
                new XElement(TrxNamespace + "StackTrace",
                    new XCData(test.ErrorStackTrace)));
        }

        return errorInfoElement;
    }

    /// <summary>
    ///     Creates the TestDefinitions section with all unit test definitions
    /// </summary>
    private static XElement CreateDefinitionsElement(IList<TestResult> testResults)
    {
        var definitionsElement = new XElement(TrxNamespace + "TestDefinitions");

        foreach (var test in testResults)
        {
            definitionsElement.Add(
                new XElement(TrxNamespace + "UnitTest",
                    new XAttribute("name", test.Name),
                    new XAttribute("id", test.TestId),
                    new XElement(TrxNamespace + "Execution",
                        new XAttribute("id", test.ExecutionId)),
                    new XElement(TrxNamespace + "TestMethod",
                        new XAttribute("codeBase", test.CodeBase),
                        new XAttribute("className", test.ClassName),
                        new XAttribute("name", test.Name))));
        }

        return definitionsElement;
    }

    /// <summary>
    ///     Creates the TestEntries section with all test entry mappings
    /// </summary>
    private static XElement CreateTestEntriesElement(IList<TestResult> testResults)
    {
        var entriesElement = new XElement(TrxNamespace + "TestEntries");

        foreach (var test in testResults)
        {
            entriesElement.Add(
                new XElement(TrxNamespace + "TestEntry",
                    new XAttribute("testId", test.TestId),
                    new XAttribute("executionId", test.ExecutionId),
                    new XAttribute("testListId", TestListId)));
        }

        return entriesElement;
    }

    /// <summary>
    ///     Creates the TestLists section
    /// </summary>
    private static XElement CreateTestListsElement()
    {
        return new XElement(TrxNamespace + "TestLists",
            new XElement(TrxNamespace + "TestList",
                new XAttribute("name", TestListName),
                new XAttribute("id", TestListId)));
    }

    /// <summary>
    ///     Creates the ResultSummary section with test statistics
    /// </summary>
    private static XElement CreateSummaryElement(IList<TestResult> testResults)
    {
        return new XElement(
            TrxNamespace + "ResultSummary",
            new XAttribute("outcome", "Completed"),
            new XElement(
                TrxNamespace + "Counters",
                new XAttribute("total", testResults.Count),
                new XAttribute("executed", testResults.Count(t => t.Outcome.IsExecuted())),
                new XAttribute("passed", testResults.Count(t => t.Outcome.IsPassed())),
                new XAttribute("failed", testResults.Count(t => t.Outcome.IsFailed()))));
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

        // Parse the run element
        ParseRunElement(doc, nsMgr, results);

        // Parse all test results
        ParseTestResults(doc, nsMgr, results);

        // Return the results
        return results;
    }

    /// <summary>
    ///     Parses the TestRun element and populates basic result properties
    /// </summary>
    private static void ParseRunElement(XDocument doc, XmlNamespaceManager nsMgr, TestResults results)
    {
        var runElement = doc.XPathSelectElement("/trx:TestRun", nsMgr) ??
                         throw new InvalidOperationException(InvalidTrxFileMessage);
        results.Id = Guid.Parse(runElement.Attribute("id")?.Value ?? Guid.NewGuid().ToString());
        results.Name = runElement.Attribute("name")?.Value ?? string.Empty;
        results.UserName = runElement.Attribute("runUser")?.Value ?? string.Empty;
    }

    /// <summary>
    ///     Parses all test result elements and adds them to the results collection
    /// </summary>
    private static void ParseTestResults(XDocument doc, XmlNamespaceManager nsMgr, TestResults results)
    {
        var resultElements = doc.XPathSelectElements(
            "/trx:TestRun/trx:Results/trx:UnitTestResult",
            nsMgr);

        foreach (var resultElement in resultElements)
        {
            var testResult = ParseTestResult(doc, nsMgr, resultElement);
            results.Results.Add(testResult);
        }
    }

    /// <summary>
    ///     Parses a single UnitTestResult element
    /// </summary>
    private static TestResult ParseTestResult(XDocument doc, XmlNamespaceManager nsMgr, XElement resultElement)
    {
        var testId = resultElement.Attribute("testId") ??
                     throw new InvalidOperationException(InvalidTrxFileMessage);

        var methodElement = doc.XPathSelectElement(
            $"/trx:TestRun/trx:TestDefinitions/trx:UnitTest[@id='{testId.Value}']/trx:TestMethod",
            nsMgr) ?? throw new InvalidOperationException(InvalidTrxFileMessage);

        var outputElement = resultElement.Element(TrxNamespace + "Output");
        var errorInfoElement = outputElement?.Element(TrxNamespace + "ErrorInfo");

        return new TestResult
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
        };
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
