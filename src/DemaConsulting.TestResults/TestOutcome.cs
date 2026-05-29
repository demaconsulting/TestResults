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
///     Defines the possible outcomes for a test case execution.
/// </summary>
/// <remarks>
///     Outcomes are partitioned into three groups used by serializers and extension methods:
///     <list type="bullet">
///         <item><b>Passed</b> (<see cref="TestOutcomeExtensions.IsPassed"/>):
///             <see cref="Passed"/>, <see cref="PassedButRunAborted"/>, <see cref="Warning"/></item>
///         <item><b>Failed</b> (<see cref="TestOutcomeExtensions.IsFailed"/>):
///             <see cref="Failed"/>, <see cref="Error"/>, <see cref="Timeout"/>, <see cref="Aborted"/></item>
///         <item><b>Not executed</b> (not <see cref="TestOutcomeExtensions.IsExecuted"/>):
///             <see cref="NotRunnable"/>, <see cref="NotExecuted"/>, <see cref="Pending"/></item>
///     </list>
///     All remaining outcomes (<see cref="Inconclusive"/>, <see cref="Disconnected"/>,
///     <see cref="InProgress"/>, <see cref="Completed"/>) are treated as executed but neither
///     passed nor failed; they appear as plain passing test cases in JUnit output.
///     <para>
///         Values map directly to TRX <c>outcome</c> attribute strings (e.g. <c>"Passed"</c>,
///         <c>"Failed"</c>) and to JUnit <c>failure</c>, <c>error</c>, and <c>skipped</c>
///         child elements.
///     </para>
/// </remarks>
public enum TestOutcome
{
    /// <summary>
    ///     Test case errored
    /// </summary>
    Error,

    /// <summary>
    ///     Test case failed
    /// </summary>
    Failed,

    /// <summary>
    ///     Test case timed out
    /// </summary>
    Timeout,

    /// <summary>
    ///     Test case aborted
    /// </summary>
    Aborted,

    /// <summary>
    ///     Test case inconclusive
    /// </summary>
    Inconclusive,

    /// <summary>
    ///     Test case passed but run was aborted
    /// </summary>
    PassedButRunAborted,

    /// <summary>
    ///     Test case not runnable
    /// </summary>
    NotRunnable,

    /// <summary>
    ///     Test case not executed
    /// </summary>
    NotExecuted,

    /// <summary>
    ///     Test case disconnected
    /// </summary>
    Disconnected,

    /// <summary>
    ///     Test case passed with warning
    /// </summary>
    Warning,

    /// <summary>
    ///     Test case passed
    /// </summary>
    Passed,

    /// <summary>
    ///     Test case completed
    /// </summary>
    Completed,

    /// <summary>
    ///     Test case in progress
    /// </summary>
    InProgress,

    /// <summary>
    ///     Test case pending
    /// </summary>
    Pending
}

/// <summary>
///     Extensions for the <see cref="TestOutcome" /> enum.
/// </summary>
/// <remarks>
///     Centralizes outcome classification logic so that serializers and consumers share a single,
///     consistent definition of which outcomes count as passed, failed, or executed.
///     All methods are pure functions with no state; safe for concurrent calls.
/// </remarks>
public static class TestOutcomeExtensions
{
    /// <summary>
    ///     Determines if the test outcome is considered passed.
    /// </summary>
    /// <remarks>
    ///     Returns <see langword="true"/> for <see cref="TestOutcome.Passed"/>,
    ///     <see cref="TestOutcome.PassedButRunAborted"/>, and <see cref="TestOutcome.Warning"/>.
    ///     Returns <see langword="false"/> for all other outcomes. This classification is used by
    ///     serializers to count passed tests in summary counters. Stateless; safe for concurrent calls.
    /// </remarks>
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates a pass</returns>
    public static bool IsPassed(this TestOutcome outcome)
    {
        // Treat outcomes where the test logic completed without failure as passed
        return outcome switch
        {
            TestOutcome.Passed => true,
            TestOutcome.PassedButRunAborted => true,
            TestOutcome.Warning => true,
            _ => false
        };
    }

    /// <summary>
    ///     Determines if the test outcome is considered failed.
    /// </summary>
    /// <remarks>
    ///     Returns <see langword="true"/> for <see cref="TestOutcome.Failed"/>,
    ///     <see cref="TestOutcome.Error"/>, <see cref="TestOutcome.Timeout"/>, and
    ///     <see cref="TestOutcome.Aborted"/>. Returns <see langword="false"/> for all other outcomes.
    ///     This classification is used by serializers to count failed tests and to map outcomes to
    ///     JUnit <c>error</c> elements for infrastructure failures. Stateless; safe for concurrent calls.
    /// </remarks>
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates a fail</returns>
    public static bool IsFailed(this TestOutcome outcome)
    {
        // Treat outcomes representing an abnormal termination or assertion failure as failed
        return outcome switch
        {
            TestOutcome.Failed => true,
            TestOutcome.Error => true,
            TestOutcome.Timeout => true,
            TestOutcome.Aborted => true,
            _ => false
        };
    }

    /// <summary>
    ///     Determines if the test outcome is considered executed.
    /// </summary>
    /// <remarks>
    ///     Returns <see langword="false"/> for <see cref="TestOutcome.NotRunnable"/>,
    ///     <see cref="TestOutcome.NotExecuted"/>, and <see cref="TestOutcome.Pending"/>.
    ///     Returns <see langword="true"/> for all other outcomes, including outcomes that are
    ///     neither passed nor failed (such as <see cref="TestOutcome.Inconclusive"/>). This
    ///     classification is used by JUnit serialization to map not-executed outcomes to the
    ///     <c>skipped</c> element and by TRX summary counters to distinguish executed tests from
    ///     scheduled or skipped ones. Stateless; safe for concurrent calls.
    /// </remarks>
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates the test was executed</returns>
    public static bool IsExecuted(this TestOutcome outcome)
    {
        // Treat outcomes where the test was never attempted as not executed
        return outcome switch
        {
            TestOutcome.NotRunnable => false,
            TestOutcome.NotExecuted => false,
            TestOutcome.Pending => false,
            _ => true
        };
    }
}
