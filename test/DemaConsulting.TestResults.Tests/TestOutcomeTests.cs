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

using Xunit;

namespace DemaConsulting.TestResults.Tests;

/// <summary>
///     Tests for the TestOutcome enum
/// </summary>
public class TestOutcomeTests
{
    /// <summary>
    ///     Test the IsPassed method for all outcomes
    /// </summary>
    [Fact]
    public void TestOutcome_IsPassed_AllOutcomes_ReturnsExpectedResult()
    {
        // Arrange: no setup required; testing enum extension directly

        // Act: verify each outcome returns the expected IsPassed result (combined with Assert)
        Assert.False(TestOutcome.Error.IsPassed());
        Assert.False(TestOutcome.Failed.IsPassed());
        Assert.False(TestOutcome.Timeout.IsPassed());
        Assert.False(TestOutcome.Aborted.IsPassed());
        Assert.False(TestOutcome.Inconclusive.IsPassed());
        Assert.True(TestOutcome.PassedButRunAborted.IsPassed());
        Assert.False(TestOutcome.NotRunnable.IsPassed());
        Assert.False(TestOutcome.NotExecuted.IsPassed());
        Assert.False(TestOutcome.Disconnected.IsPassed());
        Assert.True(TestOutcome.Warning.IsPassed());
        Assert.True(TestOutcome.Passed.IsPassed());
        Assert.False(TestOutcome.Completed.IsPassed());
        Assert.False(TestOutcome.InProgress.IsPassed());
        Assert.False(TestOutcome.Pending.IsPassed());
    }

    /// <summary>
    ///     Test the IsFailed method for all outcomes
    /// </summary>
    [Fact]
    public void TestOutcome_IsFailed_AllOutcomes_ReturnsExpectedResult()
    {
        // Arrange: no setup required; testing enum extension directly

        // Act: verify each outcome returns the expected IsFailed result (combined with Assert)
        Assert.True(TestOutcome.Error.IsFailed());
        Assert.True(TestOutcome.Failed.IsFailed());
        Assert.True(TestOutcome.Timeout.IsFailed());
        Assert.True(TestOutcome.Aborted.IsFailed());
        Assert.False(TestOutcome.Inconclusive.IsFailed());
        Assert.False(TestOutcome.PassedButRunAborted.IsFailed());
        Assert.False(TestOutcome.NotRunnable.IsFailed());
        Assert.False(TestOutcome.NotExecuted.IsFailed());
        Assert.False(TestOutcome.Disconnected.IsFailed());
        Assert.False(TestOutcome.Warning.IsFailed());
        Assert.False(TestOutcome.Passed.IsFailed());
        Assert.False(TestOutcome.Completed.IsFailed());
        Assert.False(TestOutcome.InProgress.IsFailed());
        Assert.False(TestOutcome.Pending.IsFailed());
    }

    /// <summary>
    ///     Test the IsExecuted method for all outcomes
    /// </summary>
    [Fact]
    public void TestOutcome_IsExecuted_AllOutcomes_ReturnsExpectedResult()
    {
        // Arrange: no setup required; testing enum extension directly

        // Act: verify each outcome returns the expected IsExecuted result (combined with Assert)
        Assert.True(TestOutcome.Error.IsExecuted());
        Assert.True(TestOutcome.Failed.IsExecuted());
        Assert.True(TestOutcome.Timeout.IsExecuted());
        Assert.True(TestOutcome.Aborted.IsExecuted());
        Assert.True(TestOutcome.Inconclusive.IsExecuted());
        Assert.True(TestOutcome.PassedButRunAborted.IsExecuted());
        Assert.False(TestOutcome.NotRunnable.IsExecuted());
        Assert.False(TestOutcome.NotExecuted.IsExecuted());
        Assert.True(TestOutcome.Disconnected.IsExecuted());
        Assert.True(TestOutcome.Warning.IsExecuted());
        Assert.True(TestOutcome.Passed.IsExecuted());
        Assert.True(TestOutcome.Completed.IsExecuted());
        Assert.True(TestOutcome.InProgress.IsExecuted());
        Assert.False(TestOutcome.Pending.IsExecuted());
    }

    /// <summary>
    ///     Test the IsExecuted method for NotExecuted outcome
    /// </summary>
    [Fact]
    public void TestOutcome_IsExecuted_NotExecutedOutcome_ReturnsFalse()
    {
        // Arrange: no setup required; testing enum extension directly

        // Act: verify NotExecuted outcome returns false (combined with Assert)
        Assert.False(TestOutcome.NotExecuted.IsExecuted());
    }
}
