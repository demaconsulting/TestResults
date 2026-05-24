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
using Xunit;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Trx example deserialization tests
/// </summary>
public sealed class TrxExampleTests
{
    /// <summary>
    ///     Test deserializing the first example file
    /// </summary>
    /// <remarks>
    ///     Tests that TrxSerializer can correctly deserialize a real-world TRX file (example1.trx)
    ///     with multiple test outcomes including passed, failed, inconclusive, timeout, pending, and not executed.
    ///     Proves that the deserializer correctly parses test names, class names, outcomes, durations, 
    ///     error messages, stack traces, and system output.
    /// </remarks>
    [Fact]
    public void TrxExampleTests_Deserialize_Example1Trx_ReturnsAllTestResults()
    {
        // Arrange: Load the first example TRX file from embedded resources
        var trxContent = TestHelpers.GetEmbeddedResource(
            "DemaConsulting.TestResults.Tests.IO.Examples.example1.trx");

        // Act: Deserialize the TRX content
        var results = TrxSerializer.Deserialize(trxContent);

        // Assert: Verify we got all 6 test results
        Assert.Equal(6, results.Results.Count);

        // Assert: Verify first test result (Passed)
        Assert.Equal("test1", results.Results[0].Name);
        Assert.Equal("test1", results.Results[0].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.Equal(44.7811567, results.Results[0].Duration.TotalSeconds, 3);

        // Assert: Verify second test result (Inconclusive)
        Assert.Equal("test2", results.Results[1].Name);
        Assert.Equal("test2", results.Results[1].ClassName);
        Assert.Equal(TestOutcome.Inconclusive, results.Results[1].Outcome);
        Assert.Equal(44.7811567, results.Results[1].Duration.TotalSeconds, 3);

        // Assert: Verify third test result (Failed with error details)
        Assert.Equal("test3", results.Results[2].Name);
        Assert.Equal("test3", results.Results[2].ClassName);
        Assert.Equal(TestOutcome.Failed, results.Results[2].Outcome);
        Assert.Equal(44.7811567, results.Results[2].Duration.TotalSeconds, 3);
        Assert.Contains("This is sample output for the unit test", results.Results[2].SystemOutput);
        Assert.Contains("This unit test failed for a bad reason", results.Results[2].ErrorMessage);
        Assert.Contains(@"at test3() in c:\tests\test3.js:line 1", results.Results[2].ErrorStackTrace);

        // Assert: Verify fourth test result (Timeout with error details)
        Assert.Equal("test4", results.Results[3].Name);
        Assert.Equal("test4", results.Results[3].ClassName);
        Assert.Equal(TestOutcome.Timeout, results.Results[3].Outcome);
        Assert.Equal(44.7811567, results.Results[3].Duration.TotalSeconds, 3);
        Assert.Contains("This is sample output for the unit test", results.Results[3].SystemOutput);
        Assert.Contains("This unit test failed because it timed out", results.Results[3].ErrorMessage);
        Assert.Contains(@"at test4() in c:\tests\test4.js:line 1", results.Results[3].ErrorStackTrace);

        // Assert: Verify fifth test result (Pending)
        Assert.Equal("test5", results.Results[4].Name);
        Assert.Equal("test5", results.Results[4].ClassName);
        Assert.Equal(TestOutcome.Pending, results.Results[4].Outcome);
        Assert.Equal(0.0, results.Results[4].Duration.TotalSeconds, 3);

        // Assert: Verify sixth test result (NotExecuted)
        Assert.Equal("test6", results.Results[5].Name);
        Assert.Equal("test6", results.Results[5].ClassName);
        Assert.Equal(TestOutcome.NotExecuted, results.Results[5].Outcome);
        Assert.Equal(0.0, results.Results[5].Duration.TotalSeconds, 3);
    }

    /// <summary>
    ///     Test deserializing the second example file
    /// </summary>
    /// <remarks>
    ///     Tests that TrxSerializer can correctly deserialize a real-world TRX file (example2.trx)
    ///     with multiple passed test results from the same test class.
    ///     Proves that the deserializer correctly handles multiple tests with the same name and 
    ///     accurately parses test names, class names, outcomes, and precise durations.
    /// </remarks>
    [Fact]
    public void TrxExampleTests_Deserialize_Example2Trx_ReturnsAllTestResults()
    {
        // Arrange: Load the second example TRX file from embedded resources
        var trxContent = TestHelpers.GetEmbeddedResource(
            "DemaConsulting.TestResults.Tests.IO.Examples.example2.trx");

        // Act: Deserialize the TRX content
        var results = TrxSerializer.Deserialize(trxContent);

        // Assert: Verify we got all 4 test results
        Assert.Equal(4, results.Results.Count);

        // Assert: Verify first test result
        Assert.Equal("AddShouldReturnCorrectValue", results.Results[0].Name);
        Assert.Equal("Gidget.Tests.MathTests", results.Results[0].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
        Assert.Equal(0.0042209, results.Results[0].Duration.TotalSeconds, 3);

        // Assert: Verify second test result
        Assert.Equal("OnePlusOneShouldNotEqualFive", results.Results[1].Name);
        Assert.Equal("Gidget.Tests.MathTests", results.Results[1].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[1].Outcome);
        Assert.Equal(0.0008597, results.Results[1].Duration.TotalSeconds, 3);

        // Assert: Verify third test result
        Assert.Equal("AddShouldReturnCorrectValue", results.Results[2].Name);
        Assert.Equal("Gidget.Tests.MathTests", results.Results[2].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[2].Outcome);
        Assert.Equal(0.0000173, results.Results[2].Duration.TotalSeconds, 3);

        // Assert: Verify fourth test result
        Assert.Equal("AddShouldReturnCorrectValue", results.Results[3].Name);
        Assert.Equal("Gidget.Tests.MathTests", results.Results[3].ClassName);
        Assert.Equal(TestOutcome.Passed, results.Results[3].Outcome);
        Assert.Equal(0.0000016, results.Results[3].Duration.TotalSeconds, 3);
    }
}
