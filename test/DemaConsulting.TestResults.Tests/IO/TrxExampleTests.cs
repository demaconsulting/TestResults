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


using DemaConsulting.TestResults.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Trx example deserialization tests
/// </summary>
[TestClass]
public class TrxExampleTests
{
    /// <summary>
    ///     Test deserializing the first example file
    /// </summary>
    [TestMethod]
    public void Deserialize_Example1Trx_ReturnsAllTestResults()
    {
        // Deserialize the first example file.
        var results = TrxSerializer.Deserialize(
            TestHelpers.GetEmbeddedResource(
                "DemaConsulting.TestResults.Tests.IO.Examples.example1.trx"));

        // Confirm we got 6 test results
        Assert.HasCount(6, results.Results);

        // Confirm the first test result
        Assert.AreEqual("test1", results.Results[0].Name);
        Assert.AreEqual("test1", results.Results[0].ClassName);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.AreEqual(44.7811567, results.Results[0].Duration.TotalSeconds, 0.001);

        // Confirm the second test result
        Assert.AreEqual("test2", results.Results[1].Name);
        Assert.AreEqual("test2", results.Results[1].ClassName);
        Assert.AreEqual(TestOutcome.Inconclusive, results.Results[1].Outcome);
        Assert.AreEqual(44.7811567, results.Results[1].Duration.TotalSeconds, 0.001);

        // Confirm the third test result
        Assert.AreEqual("test3", results.Results[2].Name);
        Assert.AreEqual("test3", results.Results[2].ClassName);
        Assert.AreEqual(TestOutcome.Failed, results.Results[2].Outcome);
        Assert.AreEqual(44.7811567, results.Results[2].Duration.TotalSeconds, 0.001);
        Assert.Contains("This is sample output for the unit test", results.Results[2].SystemOutput);
        Assert.Contains("This unit test failed for a bad reason", results.Results[2].ErrorMessage);
        Assert.Contains(@"at test3() in c:\tests\test3.js:line 1", results.Results[2].ErrorStackTrace);

        // Confirm the fourth test result
        Assert.AreEqual("test4", results.Results[3].Name);
        Assert.AreEqual("test4", results.Results[3].ClassName);
        Assert.AreEqual(TestOutcome.Timeout, results.Results[3].Outcome);
        Assert.AreEqual(44.7811567, results.Results[3].Duration.TotalSeconds, 0.001);
        Assert.Contains("This is sample output for the unit test", results.Results[3].SystemOutput);
        Assert.Contains("This unit test failed because it timed out", results.Results[3].ErrorMessage);
        Assert.Contains(@"at test4() in c:\tests\test4.js:line 1", results.Results[3].ErrorStackTrace);

        // Confirm the fifth test result
        Assert.AreEqual("test5", results.Results[4].Name);
        Assert.AreEqual("test5", results.Results[4].ClassName);
        Assert.AreEqual(TestOutcome.Pending, results.Results[4].Outcome);
        Assert.AreEqual(0.0, results.Results[4].Duration.TotalSeconds, 0.001);

        // Confirm the sixth test result
        Assert.AreEqual("test6", results.Results[5].Name);
        Assert.AreEqual("test6", results.Results[5].ClassName);
        Assert.AreEqual(TestOutcome.NotExecuted, results.Results[5].Outcome);
        Assert.AreEqual(0.0, results.Results[5].Duration.TotalSeconds, 0.001);
    }

    /// <summary>
    ///     Test deserializing the second example file
    /// </summary>
    [TestMethod]
    public void TrxExampleTests_Deserialize_Example2Trx_ReturnsAllTestResults()
    {
        // Deserialize the second example file.
        var results = TrxSerializer.Deserialize(
            TestHelpers.GetEmbeddedResource(
                "DemaConsulting.TestResults.Tests.IO.Examples.example2.trx"));

        // Confirm we got 4 test results
        Assert.HasCount(4, results.Results);

        // Confirm the first test result
        Assert.AreEqual("AddShouldReturnCorrectValue", results.Results[0].Name);
        Assert.AreEqual("Gidget.Tests.MathTests", results.Results[0].ClassName);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.AreEqual(0.0042209, results.Results[0].Duration.TotalSeconds, 0.001);

        // Confirm the second test result
        Assert.AreEqual("OnePlusOneShouldNotEqualFive", results.Results[1].Name);
        Assert.AreEqual("Gidget.Tests.MathTests", results.Results[1].ClassName);
        Assert.AreEqual(TestOutcome.Passed, results.Results[1].Outcome);
        Assert.AreEqual(0.0008597, results.Results[1].Duration.TotalSeconds, 0.001);

        // Confirm the third test result
        Assert.AreEqual("AddShouldReturnCorrectValue", results.Results[2].Name);
        Assert.AreEqual("Gidget.Tests.MathTests", results.Results[2].ClassName);
        Assert.AreEqual(TestOutcome.Passed, results.Results[2].Outcome);
        Assert.AreEqual(0.0000173, results.Results[2].Duration.TotalSeconds, 0.001);

        // Confirm the fourth test result
        Assert.AreEqual("AddShouldReturnCorrectValue", results.Results[3].Name);
        Assert.AreEqual("Gidget.Tests.MathTests", results.Results[3].ClassName);
        Assert.AreEqual(TestOutcome.Passed, results.Results[3].Outcome);
        Assert.AreEqual(0.0000016, results.Results[3].Duration.TotalSeconds, 0.001);
    }
}
