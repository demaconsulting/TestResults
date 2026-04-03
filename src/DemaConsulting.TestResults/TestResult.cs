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
///     Represents the result of a single test case execution.
/// </summary>
public sealed class TestResult
{
    // Identity properties — each result needs unique IDs for cross-referencing
    // between the test definition and its execution record

    /// <summary>
    ///     Gets or sets the ID of the test case.
    ///     Defaults to a newly generated <see cref="Guid" /> so every test definition is uniquely identifiable.
    /// </summary>
    public Guid TestId { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Gets or sets the ID of the test execution.
    ///     Defaults to a newly generated <see cref="Guid" /> so every execution is uniquely identifiable.
    /// </summary>
    public Guid ExecutionId { get; set; } = Guid.NewGuid();

    // Descriptive metadata — human-readable strings that identify the test in reports

    /// <summary>
    ///     Gets or sets the name of the test case.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the test code assembly.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string CodeBase { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the class containing the test case.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the computer that executed the test case.
    ///     Defaults to <see cref="Environment.MachineName" /> so locally-run results are attributed correctly.
    /// </summary>
    public string ComputerName { get; set; } = Environment.MachineName;

    // Timing properties — record when and how long the test ran

    /// <summary>
    ///     Gets or sets the start time of the test execution.
    ///     Defaults to <see cref="DateTime.UtcNow" /> at construction time so ad-hoc results have a meaningful timestamp.
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     Gets or sets the duration of the test execution.
    ///     Defaults to <see cref="TimeSpan.Zero" /> so the property is always valid even when timing is unavailable.
    /// </summary>
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;

    // Output capture — text written to stdout/stderr during the test run

    /// <summary>
    ///     Gets or sets the stdout output when executing the test case.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string SystemOutput { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the stderr output when executing the test case.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string SystemError { get; set; } = string.Empty;

    // Result properties — the outcome and any failure details

    /// <summary>
    ///     Gets or sets the outcome of the test case.
    ///     Defaults to <see cref="TestOutcome.NotExecuted" /> so a result that was never run is not mistaken for a pass.
    /// </summary>
    public TestOutcome Outcome { get; set; } = TestOutcome.NotExecuted;

    /// <summary>
    ///     Gets or sets the test case error message.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the test case error stack trace.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string ErrorStackTrace { get; set; } = string.Empty;
}
