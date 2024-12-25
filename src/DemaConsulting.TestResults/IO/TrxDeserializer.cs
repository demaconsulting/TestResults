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

public static class TrxDeserializer
{
    /// <summary>
    ///     Deserializes the TRX test results.
    /// </summary>
    /// <param name="xml">TRX text</param>
    /// <returns>Deserialized TestRun</returns>
    public static TestRun Deserialize(string xml)
    {
        // Parse the document
        var doc = XDocument.Parse(xml);

        // Get the root element
        var root = doc.Root ?? throw new InvalidOperationException("Invalid TRX XML");

        // Deserialize the TestRun from the root element
        return DeserializeRun(root);
    }

    /// <summary>
    ///     Deserializes a TRX test run.
    /// </summary>
    /// <param name="element">TRX TestRun Element</param>
    /// <returns>Deserialized TestRun</returns>
    public static TestRun DeserializeRun(XElement element)
    {
        // Handle times if provided
        DateTime? timestamp = null;
        double? duration = null;
        var times = element.Element("Times");
        if (times != null)
        {
            timestamp = ParseDateTime(times.Attribute("start")?.Value);
            var finishTime = ParseDateTime(times.Attribute("finish")?.Value);
            duration = (finishTime - timestamp)?.TotalSeconds;
        }

        // Deserialize the test-suites into a TestRun
        return new TestRun
        {
            Name = element.Attribute("name")?.Value,
            TimeStamp = timestamp,
            Duration = duration
        };
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