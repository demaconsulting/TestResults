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

namespace DemaConsulting.TestResults;

/// <summary>
///     Represents a test case
/// </summary>
public sealed class TestCase
{
    /// <summary>
    ///    Gets or sets the name of the test case.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Gets the class name containing this test case.
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    ///     Gets the name of the source file that contains the test suite.
    /// </summary>
    public string? SourceFile { get; set; }

    /// <summary>
    ///    Gets the line number in the source file where the test suite is defined.
    /// </summary>
    public int? LineNumber { get; set; }

    /// <summary>
    ///     Gets the number of assertions checked during the test case execution.
    /// </summary>
    public int? Assertions { get; set; }

    /// <summary>
    ///    Gets the time-stamp when the test case was executed
    /// </summary>
    public DateTime? TimeStamp { get; set; }

    /// <summary>
    ///     Gets the duration of the test case execution in seconds.
    /// </summary>
    public double? Duration { get; set; }

    /// <summary>
    ///     Gets the optional skipped reason for the test case.
    /// </summary>
    public string? Skipped { get; set; }

    /// <summary>
    ///     Gets the optional test failures for the test case.
    /// </summary>
    public TestFailure[] Failures { get; set; } = [];

    /// <summary>
    ///     Gets the optional test errors for the test case.
    /// </summary>
    public TestError[] Errors { get; set; } = [];

    /// <summary>
    ///     Gets the optional outcome of the test case.
    /// </summary>
    public TestOutcome? Outcome { get; set; }

    /// <summary>
    ///     Gets or sets the optional standard output of the test case.
    /// </summary>
    public string[] StdOut { get; set; } = [];

    /// <summary>
    ///     Gets or sets the optional standard error output of the test case.
    /// </summary>
    public string[] StdErr { get; set; } = [];

    /// <summary>
    ///     Gets the outcome of the test case based on the errors, failures, and skipped status.
    /// </summary>
    /// <returns>Test case outcome</returns>
    public TestOutcome GetOutcome()
    {
        // Use the outcome if specified
        if (Outcome != null)
            return Outcome.Value;

        // If there are any errors, then the outcome is an error
        if (Errors.Length != 0)
            return TestOutcome.Error;

        // If there are any failures, then the outcome is a failure
        if (Failures.Length != 0)
            return TestOutcome.Failed;

        // If the test was skipped, then the outcome is skipped
        if (Skipped != null)
            return TestOutcome.Ignored;

        return TestOutcome.Passed;
    }
}
