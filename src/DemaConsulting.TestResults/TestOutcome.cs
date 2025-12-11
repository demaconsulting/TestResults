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
///     TestOutcome enum
/// </summary>
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
public static class TestOutcomeExtensions
{
    /// <summary>
    ///     Determines if the test outcome is considered passed.
    /// </summary>
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates a pass</returns>
    public static bool IsPassed(this TestOutcome outcome)
    {
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
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates a fail</returns>
    public static bool IsFailed(this TestOutcome outcome)
    {
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
    /// <param name="outcome">Test outcome</param>
    /// <returns>True if the outcome indicates the test was executed</returns>
    public static bool IsExecuted(this TestOutcome outcome)
    {
        return outcome switch
        {
            TestOutcome.NotRunnable => false,
            TestOutcome.NotExecuted => false,
            TestOutcome.Pending => false,
            _ => true
        };
    }
}