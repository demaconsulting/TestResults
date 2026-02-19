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

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     Test result format types
/// </summary>
public enum TestResultFormat
{
    /// <summary>
    ///     Unknown format
    /// </summary>
    Unknown,

    /// <summary>
    ///     TRX (Visual Studio Test Results) format
    /// </summary>
    Trx,

    /// <summary>
    ///     JUnit XML format
    /// </summary>
    JUnit
}

/// <summary>
///     General purpose serializer for test results
/// </summary>
public static class Serializer
{
#pragma warning disable S1075 // URIs should not be hardcoded - This is an XML namespace URI, not a file path
    /// <summary>
    ///     TRX namespace for identifying TRX files
    /// </summary>
    private const string TrxNamespaceUri = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
#pragma warning restore S1075

    /// <summary>
    ///     Identifies the test result format based on the contents
    /// </summary>
    /// <param name="contents">The test result file contents</param>
    /// <returns>The identified test result format</returns>
    public static TestResultFormat Identify(string contents)
    {
        // Validate input is not null or whitespace
        if (string.IsNullOrWhiteSpace(contents))
        {
            return TestResultFormat.Unknown;
        }

        try
        {
            // Parse the XML document
            var doc = XDocument.Parse(contents);
            var root = doc.Root;

            // Check if root element exists
            if (root == null)
            {
                return TestResultFormat.Unknown;
            }

            // Check for TRX format by namespace and root element
            if (root.Name.Namespace == TrxNamespaceUri && root.Name.LocalName == "TestRun")
            {
                return TestResultFormat.Trx;
            }

            // Check for JUnit format by root element names
            if (root.Name.LocalName == "testsuites" || root.Name.LocalName == "testsuite")
            {
                return TestResultFormat.JUnit;
            }

            return TestResultFormat.Unknown;
        }
        catch (System.Xml.XmlException)
        {
            // If XML parsing fails, format is unknown
            return TestResultFormat.Unknown;
        }
    }

    /// <summary>
    ///     Deserializes test result contents to TestResults using the appropriate deserializer
    /// </summary>
    /// <param name="contents">The test result file contents</param>
    /// <returns>Deserialized test results</returns>
    /// <exception cref="ArgumentException">Thrown when contents is null or whitespace</exception>
    /// <exception cref="InvalidOperationException">Thrown when format cannot be identified or deserialization fails</exception>
    public static TestResults Deserialize(string contents)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(contents))
        {
            throw new ArgumentException("Test result contents cannot be null or whitespace", nameof(contents));
        }

        // Identify the format
        var format = Identify(contents);

        // Deserialize based on format
        return format switch
        {
            TestResultFormat.Trx => TrxSerializer.Deserialize(contents),
            TestResultFormat.JUnit => JUnitSerializer.Deserialize(contents),
            _ => throw new InvalidOperationException("Unable to identify test result format")
        };
    }
}
