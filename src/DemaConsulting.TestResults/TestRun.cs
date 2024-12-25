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
///     Represents test results.
/// </summary>
public sealed class TestRun
{
    /// <summary>
    ///    Gets or sets the optional name of the entire test run.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///    Gets or sets the optional time-stamp when the test run was executed
    /// </summary>
    public DateTime? TimeStamp { get; set; }

    /// <summary>
    ///     Gets or sets the optional duration of the test run execution in seconds.
    /// </summary>
    public double? Duration { get; set; }

    /// <summary>
    ///     Gets or sets the list of test suites in this test run.
    /// </summary>
    public List<TestSuite> Suites { get; set; } = [];

    /// <summary>
    ///     Gets the number of tests in the test run.
    /// </summary>
    /// <returns>Count of test cases in this run</returns>
    public int GetTests() => Suites.Aggregate(0, (n, c) => n + c.GetTests());

    /// <summary>
    ///     Gets the number of test cases in the test run that failed.
    /// </summary>
    /// <returns>Count of failures</returns>
    public int GetFailures() => Suites.Aggregate(0, (n, c) => n + c.GetFailures());

    /// <summary>
    ///     Gets the number of test cases in the test run that had errors.
    /// </summary>
    /// <returns>Count of errors</returns>
    public int GetErrors() => Suites.Aggregate(0, (n, c) => n + c.GetErrors());

    /// <summary>
    ///     Gets the number of test cases in the test run that were skipped.
    /// </summary>
    /// <returns>Count of skipped</returns>
    public int GetSkipped() => Suites.Aggregate(0, (n, c) => n + c.GetSkipped());

    /// <summary>
    ///    Gets the duration of the test run execution in seconds.
    /// </summary>
    /// <returns>Duration in seconds</returns>
    public double GetDuration() => Duration ?? Suites.Aggregate(0.0, (n, c) => n + c.GetDuration());
}