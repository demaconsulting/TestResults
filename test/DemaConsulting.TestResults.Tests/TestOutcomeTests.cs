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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests;

/// <summary>
///     Tests for the TestOutcome enum
/// </summary>
[TestClass]
public class TestOutcomeTests
{
    /// <summary>
    ///     Test the IsPassed method for all outcomes
    /// </summary>
    [TestMethod]
    public void Test_Outcome_IsPassed()
    {
        Assert.IsFalse(TestOutcome.Error.IsPassed());
        Assert.IsFalse(TestOutcome.Failed.IsPassed());
        Assert.IsFalse(TestOutcome.Timeout.IsPassed());
        Assert.IsFalse(TestOutcome.Aborted.IsPassed());
        Assert.IsFalse(TestOutcome.Inconclusive.IsPassed());
        Assert.IsTrue(TestOutcome.PassedButRunAborted.IsPassed());
        Assert.IsFalse(TestOutcome.NotRunnable.IsPassed());
        Assert.IsFalse(TestOutcome.NotExecuted.IsPassed());
        Assert.IsFalse(TestOutcome.Disconnected.IsPassed());
        Assert.IsTrue(TestOutcome.Warning.IsPassed());
        Assert.IsTrue(TestOutcome.Passed.IsPassed());
        Assert.IsFalse(TestOutcome.Completed.IsPassed());
        Assert.IsFalse(TestOutcome.InProgress.IsPassed());
        Assert.IsFalse(TestOutcome.Pending.IsPassed());
    }

    /// <summary>
    ///     Test the IsFailed method for all outcomes
    /// </summary>
    [TestMethod]
    public void Test_Outcome_IsFailed()
    {
        Assert.IsTrue(TestOutcome.Error.IsFailed());
        Assert.IsTrue(TestOutcome.Failed.IsFailed());
        Assert.IsTrue(TestOutcome.Timeout.IsFailed());
        Assert.IsTrue(TestOutcome.Aborted.IsFailed());
        Assert.IsFalse(TestOutcome.Inconclusive.IsFailed());
        Assert.IsFalse(TestOutcome.PassedButRunAborted.IsFailed());
        Assert.IsFalse(TestOutcome.NotRunnable.IsFailed());
        Assert.IsFalse(TestOutcome.NotExecuted.IsFailed());
        Assert.IsFalse(TestOutcome.Disconnected.IsFailed());
        Assert.IsFalse(TestOutcome.Warning.IsFailed());
        Assert.IsFalse(TestOutcome.Passed.IsFailed());
        Assert.IsFalse(TestOutcome.Completed.IsFailed());
        Assert.IsFalse(TestOutcome.InProgress.IsFailed());
        Assert.IsFalse(TestOutcome.Pending.IsFailed());
    }

    /// <summary>
    ///     Test the IsExecuted method for all outcomes
    /// </summary>
    [TestMethod]
    public void Test_Outcome_IsExecuted()
    {
        Assert.IsTrue(TestOutcome.Error.IsExecuted());
        Assert.IsTrue(TestOutcome.Failed.IsExecuted());
        Assert.IsTrue(TestOutcome.Timeout.IsExecuted());
        Assert.IsTrue(TestOutcome.Aborted.IsExecuted());
        Assert.IsTrue(TestOutcome.Inconclusive.IsExecuted());
        Assert.IsTrue(TestOutcome.PassedButRunAborted.IsExecuted());
        Assert.IsFalse(TestOutcome.NotRunnable.IsExecuted());
        Assert.IsFalse(TestOutcome.NotExecuted.IsExecuted());
        Assert.IsTrue(TestOutcome.Disconnected.IsExecuted());
        Assert.IsTrue(TestOutcome.Warning.IsExecuted());
        Assert.IsTrue(TestOutcome.Passed.IsExecuted());
        Assert.IsTrue(TestOutcome.Completed.IsExecuted());
        Assert.IsTrue(TestOutcome.InProgress.IsExecuted());
        Assert.IsFalse(TestOutcome.Pending.IsExecuted());
    }
}