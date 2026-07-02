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

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     The <c>DemaConsulting.TestResults.IO</c> namespace provides serialization and
///     deserialization of test result files in TRX (Visual Studio Test Results) and JUnit XML
///     formats.
/// </summary>
/// <remarks>
///     <para>
///         This namespace contains four public types:
///         <list type="bullet">
///             <item>
///                 <see cref="TrxSerializer"/> — reads and writes the TRX XML format produced by
///                 MSTest, <c>dotnet test</c>, and Azure DevOps pipelines.
///             </item>
///             <item>
///                 <see cref="JUnitSerializer"/> — reads and writes the JUnit XML format accepted
///                 by Jenkins, GitHub Actions test reporters, GitLab CI, and most other CI
///                 systems.
///             </item>
///             <item>
///                 <see cref="Serializer"/> — a format-detecting facade that inspects the XML
///                 root element and namespace to pick the correct serializer automatically.
///                 Also exposes <see cref="Serializer.Identify"/> for callers that only need to
///                 know the format without deserializing.
///             </item>
///         </list>
///         The <see cref="TestResultFormat"/> enum labels the supported formats:
///         <see cref="TestResultFormat.Trx"/>, <see cref="TestResultFormat.JUnit"/>, and
///         <see cref="TestResultFormat.Unknown"/> for unrecognized input.
///     </para>
///     <para>
///         <b>Choosing a serializer:</b>
///         <list type="bullet">
///             <item>
///                 Use <see cref="Serializer.Deserialize"/> when the input format is not known
///                 in advance — it identifies and delegates automatically.
///             </item>
///             <item>
///                 Use <see cref="TrxSerializer"/> or <see cref="JUnitSerializer"/> directly
///                 when the format is already known, to avoid the detection overhead.
///             </item>
///         </list>
///     </para>
///     <para>
///         <b>Typical usage — auto-detected deserialization:</b>
///         <code>
///         using DemaConsulting.TestResults;
///         using DemaConsulting.TestResults.IO;
///
///         string content = File.ReadAllText("results.trx");
///         TestResults run = Serializer.Deserialize(content);
///
///         Console.WriteLine($"Run: {run.Name}, Tests: {run.Results.Count}");
///         </code>
///     </para>
///     <para>
///         <b>Typical usage — format identification only:</b>
///         <code>
///         using DemaConsulting.TestResults.IO;
///
///         string content = File.ReadAllText("results.xml");
///         TestResultFormat format = Serializer.Identify(content);
///         Console.WriteLine(format); // "JUnit" or "Trx" or "Unknown"
///         </code>
///     </para>
///     <para>
///         <b>Typical usage — explicit TRX serialization:</b>
///         <code>
///         using DemaConsulting.TestResults;
///         using DemaConsulting.TestResults.IO;
///
///         var run = new TestResults { Name = "My Run" };
///         run.Results.Add(new TestResult { Name = "Test1", Outcome = TestOutcome.Passed });
///
///         string trx = TrxSerializer.Serialize(run);
///         File.WriteAllText("results.trx", trx);
///         </code>
///     </para>
///     <para>
///         <b>Typical usage — explicit JUnit XML serialization:</b>
///         <code>
///         using DemaConsulting.TestResults;
///         using DemaConsulting.TestResults.IO;
///
///         var run = new TestResults { Name = "My Run" };
///         run.Results.Add(new TestResult { Name = "Test1", Outcome = TestOutcome.Passed });
///
///         string xml = JUnitSerializer.Serialize(run);
///         File.WriteAllText("results.xml", xml);
///         </code>
///     </para>
///     <para>
///         <b>Known JUnit round-trip losses:</b> <see cref="TestOutcome.Timeout"/> and
///         <see cref="TestOutcome.Aborted"/> both serialize as <c>error</c> elements and
///         deserialize back as <see cref="TestOutcome.Error"/>.
///         <see cref="TestOutcome.Inconclusive"/> serializes as a plain passing
///         <c>testcase</c> and deserializes back as <see cref="TestOutcome.Passed"/>.
///     </para>
///     <para>
///         <b>Thread safety:</b> All serializer methods in this namespace are stateless and
///         safe for concurrent calls.
///     </para>
/// </remarks>
internal static class NamespaceDoc
{
}
