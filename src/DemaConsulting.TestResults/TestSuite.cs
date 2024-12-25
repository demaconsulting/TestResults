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
///     Represents a test suite.
/// </summary>
public sealed class TestSuite
{
    /// <summary>
    ///     Gets or sets the name of the test suite.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets the optional name of the source file that contains the test suite.
    /// </summary>
    public string? SourceFile { get; set; }

    /// <summary>
    ///    Gets or sets the optional time-stamp when the test suite was executed
    /// </summary>
    public DateTime? TimeStamp { get; set; }

    /// <summary>
    ///     Gets or sets the optional duration of the test suite execution in seconds.
    /// </summary>
    public double? Duration { get; set; }

    /// <summary>
    ///     Gets or sets the optional host-name where the test suite was executed.
    /// </summary>
    public string? HostName { get; set; }

    /// <summary>
    ///     Gets or sets the optional test suite ID.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    ///     Gets or sets the optional standard output of the test case.
    /// </summary>
    public string[] StdOut { get; set; } = [];

    /// <summary>
    ///     Gets or sets the optional standard error output of the test case.
    /// </summary>
    public string[] StdErr { get; set; } = [];

    /// <summary>
    ///     Gets or sets the list of test-cases in this test suite.
    /// </summary>
    public List<TestCase> Cases { get; set; } = [];

    /// <summary>
    ///    Gets the number of test cases in this test suite.
    /// </summary>
    /// <returns>Count of test cases in this test suite</returns>
    public int GetTests() => Cases.Count;

    /// <summary>
    ///     Gets the number of test cases in this test suite that failed.
    /// </summary>
    /// <returns>Count of failures</returns>
    public int GetFailures() => Cases.Count(c => c.GetOutcome() == TestOutcome.Failed);

    /// <summary>
    ///     Gets the number of error test cases this test suite that had errors.
    /// </summary>
    /// <returns>Count of errors</returns>
    public int GetErrors() => Cases.Count(c => c.GetOutcome() == TestOutcome.Error);

    /// <summary>
    ///     Gets the number of test cases in this test suite that were skipped.
    /// </summary>
    /// <returns>Count of skipped</returns>
    public int GetSkipped() => Cases.Count(c => c.GetOutcome() == TestOutcome.Ignored);

    /// <summary>
    ///    Gets the duration of the test suite execution in seconds.
    /// </summary>
    /// <returns>Duration in seconds</returns>
    public double GetDuration() => Duration ?? Cases.Aggregate(0.0, (n, c) => n + c.Duration ?? 0.0);
}