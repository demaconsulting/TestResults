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
///     Tests for the IO subsystem
/// </summary>
[TestClass]
public sealed class IOTests
{
    /// <summary>
    ///     Test that the IO subsystem can identify TRX content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly identifies TRX-format content.
    ///     Proves that the subsystem can distinguish TRX from other formats.
    /// </remarks>
    [TestMethod]
    public void IO_Identify_TrxContent_ReturnsTrx()
    {
        // Arrange: Minimal TRX content
        const string content = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" />
            """;

        // Act: identify the format of the TRX content
        var format = Serializer.Identify(content);

        // Assert: format should be identified as TRX
        Assert.AreEqual(TestResultFormat.Trx, format);
    }

    /// <summary>
    ///     Test that the IO subsystem can identify JUnit content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly identifies JUnit-format content.
    ///     Proves that the subsystem can distinguish JUnit from other formats.
    /// </remarks>
    [TestMethod]
    public void IO_Identify_JUnitContent_ReturnsJUnit()
    {
        // Arrange: Minimal JUnit content
        const string content = "<testsuites />";

        // Act: identify the format of the JUnit content
        var format = Serializer.Identify(content);

        // Assert: format should be identified as JUnit
        Assert.AreEqual(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that the IO subsystem can deserialize TRX content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly deserializes TRX-format content into TestResults.
    ///     Proves that the subsystem end-to-end flow works for TRX files.
    /// </remarks>
    [TestMethod]
    public void IO_Deserialize_TrxContent_ReturnsTestResults()
    {
        // Arrange: Minimal TRX content with one test result
        const string content = """
            <?xml version="1.0"?>
            <TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" id="da21858f-2c78-442a-8ea6-51fe73762e0e" name="Test Run" runUser="User">
              <Results>
                <UnitTestResult testId="12345678-1234-1234-1234-123456789abc" testName="Test1" outcome="Passed" executionId="87654321-4321-4321-4321-cba987654321" computerName="TestComputer" testType="13CDC9D9-DDB5-4fa4-A97D-D965CCFC6D4B" duration="00:00:01" startTime="2025-01-01T00:00:00.000Z" endTime="2025-01-01T00:00:01.000Z" testListId="19431567-8539-422a-85D7-44EE4E166BDA">
                  <Output />
                </UnitTestResult>
              </Results>
              <TestDefinitions>
                <UnitTest name="Test1" id="12345678-1234-1234-1234-123456789abc">
                  <Execution id="87654321-4321-4321-4321-cba987654321" />
                  <TestMethod codeBase="TestAssembly" className="TestClass" name="Test1" />
                </UnitTest>
              </TestDefinitions>
            </TestRun>
            """;

        // Act: deserialize the TRX content
        var results = Serializer.Deserialize(content);

        // Assert: deserialized results should contain one test result
        Assert.IsNotNull(results);
        Assert.HasCount(1, results.Results);
        Assert.AreEqual("Test1", results.Results[0].Name);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that the IO subsystem can deserialize JUnit content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly deserializes JUnit-format content into TestResults.
    ///     Proves that the subsystem end-to-end flow works for JUnit files.
    /// </remarks>
    [TestMethod]
    public void IO_Deserialize_JUnitContent_ReturnsTestResults()
    {
        // Arrange: Minimal JUnit content with one test result
        const string content = """
            <?xml version="1.0"?>
            <testsuites name="MySuite">
              <testsuite name="MyClass" tests="1" failures="0" errors="0">
                <testcase name="Test1" classname="MyClass" time="1.0" />
              </testsuite>
            </testsuites>
            """;

        // Act: deserialize the JUnit content
        var results = Serializer.Deserialize(content);

        // Assert: deserialized results should contain one test result
        Assert.IsNotNull(results);
        Assert.HasCount(1, results.Results);
        Assert.AreEqual("Test1", results.Results[0].Name);
        Assert.AreEqual(TestOutcome.Passed, results.Results[0].Outcome);
    }
}
