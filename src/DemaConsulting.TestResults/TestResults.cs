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
///     Represents a collection of test results for a complete test run.
/// </summary>
public sealed class TestResults
{
    // Identity and metadata — unique identifier and human-readable name for the run

    /// <summary>
    ///     Gets or sets the ID of the test results.
    ///     Defaults to a newly generated <see cref="Guid" /> so every test run is uniquely identifiable.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     Gets or sets the name of the tests.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the name of the user account running the tests.
    ///     Defaults to <see cref="string.Empty" /> so the property is always non-null.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    // Results collection — the ordered list of individual test outcomes for this run

    /// <summary>
    ///     Gets or sets the list containing each <see cref="TestResult" />.
    ///     Defaults to an empty list so callers can add results without null-checking first.
    /// </summary>
    public List<TestResult> Results { get; set; } = [];
}
