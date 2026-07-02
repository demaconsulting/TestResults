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

namespace DemaConsulting.TestResults;

/// <summary>
///     The <c>DemaConsulting.TestResults</c> namespace provides an in-memory model and
///     serialization support for reading and writing test result files in TRX and JUnit XML
///     formats.
/// </summary>
/// <remarks>
///     <para>
///         The library is organized into two layers:
///         <list type="bullet">
///             <item>
///                 <b>Model layer</b> — three types that represent a complete test run in memory:
///                 <see cref="TestResults"/>, <see cref="TestResult"/>, and
///                 <see cref="TestOutcome"/>.
///             </item>
///             <item>
///                 <b>IO layer</b> — serializers in the <c>DemaConsulting.TestResults.IO</c>
///                 child namespace that translate between the model and XML on disk.
///             </item>
///         </list>
///     </para>
///     <para>
///         <b>Core model types:</b>
///         <list type="bullet">
///             <item>
///                 <see cref="TestResults"/> — represents an entire test run. It holds a
///                 <see cref="TestResults.Name"/>, a <see cref="TestResults.UserName"/>, and an
///                 ordered <see cref="TestResults.Results"/> collection of
///                 <see cref="TestResult"/> objects.
///             </item>
///             <item>
///                 <see cref="TestResult"/> — represents a single test case execution. It carries
///                 identity (<see cref="TestResult.TestId"/>, <see cref="TestResult.ExecutionId"/>),
///                 metadata (<see cref="TestResult.Name"/>, <see cref="TestResult.ClassName"/>,
///                 <see cref="TestResult.CodeBase"/>, <see cref="TestResult.ComputerName"/>),
///                 timing (<see cref="TestResult.StartTime"/>, <see cref="TestResult.Duration"/>),
///                 captured output (<see cref="TestResult.SystemOutput"/>,
///                 <see cref="TestResult.SystemError"/>), and failure details
///                 (<see cref="TestResult.Outcome"/>, <see cref="TestResult.ErrorMessage"/>,
///                 <see cref="TestResult.ErrorStackTrace"/>).
///             </item>
///             <item>
///                 <see cref="TestOutcome"/> — an enum of all possible test case outcomes.
///                 <see cref="TestOutcomeExtensions"/> adds three classification helpers:
///                 <see cref="TestOutcomeExtensions.IsPassed"/>,
///                 <see cref="TestOutcomeExtensions.IsFailed"/>, and
///                 <see cref="TestOutcomeExtensions.IsExecuted"/>.
///             </item>
///         </list>
///     </para>
///     <para>
///         <b>Typical usage — building and serializing a test run:</b>
///         <code>
///         using DemaConsulting.TestResults;
///         using DemaConsulting.TestResults.IO;
///
///         var run = new TestResults { Name = "Smoke Tests", UserName = Environment.UserName };
///
///         run.Results.Add(new TestResult
///         {
///             Name        = "LoginSucceeds",
///             ClassName   = "AuthTests",
///             Outcome     = TestOutcome.Passed,
///             Duration    = TimeSpan.FromMilliseconds(42)
///         });
///
///         run.Results.Add(new TestResult
///         {
///             Name           = "LoginFailsBadPassword",
///             ClassName      = "AuthTests",
///             Outcome        = TestOutcome.Failed,
///             ErrorMessage   = "Expected 401, got 200",
///             ErrorStackTrace = "at AuthTests.LoginFailsBadPassword() ..."
///         });
///
///         // Write TRX (Visual Studio / MSTest format)
///         string trx = TrxSerializer.Serialize(run);
///         File.WriteAllText("results.trx", trx);
///
///         // Write JUnit XML (Jenkins / GitHub Actions / GitLab CI format)
///         string xml = JUnitSerializer.Serialize(run);
///         File.WriteAllText("results.xml", xml);
///         </code>
///     </para>
///     <para>
///         <b>Typical usage — reading an existing result file:</b>
///         <code>
///         using DemaConsulting.TestResults;
///         using DemaConsulting.TestResults.IO;
///
///         // Auto-detect format (TRX or JUnit)
///         string content = File.ReadAllText("results.trx");
///         TestResults run = Serializer.Deserialize(content);
///
///         foreach (TestResult result in run.Results)
///         {
///             Console.WriteLine($"{result.Name}: {result.Outcome}");
///             if (result.Outcome.IsFailed())
///                 Console.WriteLine($"  {result.ErrorMessage}");
///         }
///         </code>
///     </para>
///     <para>
///         <b>Default values:</b> String properties on <see cref="TestResult"/> and
///         <see cref="TestResults"/> default to non-null values so consumers can read any
///         property without null-checking. Specifically, most string properties default to
///         <see cref="string.Empty"/>, while <see cref="TestResult.ComputerName"/> defaults to
///         <see cref="System.Environment.MachineName"/>. <see cref="TestResult.Outcome"/> defaults
///         to <see cref="TestOutcome.NotExecuted"/> so a result that was never populated is not
///         mistaken for a pass. <see cref="TestResult.TestId"/>,
///         <see cref="TestResult.ExecutionId"/>, and <see cref="TestResults.Id"/> each
///         auto-generate a fresh <see cref="System.Guid"/> at construction time.
///     </para>
///     <para>
///         <b>Thread safety:</b> <see cref="TestResults"/> and <see cref="TestResult"/> are not
///         thread-safe; callers that share instances across threads must provide their own
///         synchronization. The serializer classes in the IO namespace are stateless and safe for
///         concurrent calls.
///     </para>
/// </remarks>
internal static class NamespaceDoc
{
}
