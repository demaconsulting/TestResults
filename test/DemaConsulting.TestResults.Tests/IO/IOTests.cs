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
///     Tests for the IO subsystem
/// </summary>
public sealed class IOTests
{
    /// <summary>
    ///     Test that the IO subsystem can identify TRX content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly identifies TRX-format content.
    ///     Proves that the subsystem can distinguish TRX from other formats.
    /// </remarks>
    [Fact]
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
        Assert.Equal(TestResultFormat.Trx, format);
    }

    /// <summary>
    ///     Test that the IO subsystem can identify JUnit content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly identifies JUnit-format content.
    ///     Proves that the subsystem can distinguish JUnit from other formats.
    /// </remarks>
    [Fact]
    public void IO_Identify_JUnitContent_ReturnsJUnit()
    {
        // Arrange: Minimal JUnit content
        const string content = "<testsuites />";

        // Act: identify the format of the JUnit content
        var format = Serializer.Identify(content);

        // Assert: format should be identified as JUnit
        Assert.Equal(TestResultFormat.JUnit, format);
    }

    /// <summary>
    ///     Test that the IO subsystem can deserialize TRX content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly deserializes TRX-format content into TestResults.
    ///     Proves that the subsystem end-to-end flow works for TRX files.
    /// </remarks>
    [Fact]
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
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that the IO subsystem can deserialize JUnit content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly deserializes JUnit-format content into TestResults.
    ///     Proves that the subsystem end-to-end flow works for JUnit files.
    /// </remarks>
    [Fact]
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
        Assert.NotNull(results);
        Assert.Single(results.Results);
        Assert.Equal("Test1", results.Results[0].Name);
        Assert.Equal(TestOutcome.Passed, results.Results[0].Outcome);
    }

    /// <summary>
    ///     Test that the IO subsystem can serialize TestResults to TRX content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly serializes an in-memory TestResults object
    ///     to TRX-format content. Proves that the subsystem end-to-end flow works for TRX output.
    /// </remarks>
    [Fact]
    public void IO_Serialize_TestResults_ProducesTrxContent()
    {
        // Arrange: in-memory TestResults with one passing test
        var results = new TestResults
        {
            Name = "MySuite",
            Results =
            [
                new TestResult { Name = "Test1", Outcome = TestOutcome.Passed }
            ]
        };

        // Act: serialize the TestResults to TRX content
        var content = TrxSerializer.Serialize(results);

        // Assert: serialized content should be valid TRX XML containing the test data
        Assert.NotNull(content);
        Assert.Contains("TestRun", content);
        Assert.Contains("Test1", content);
    }

    /// <summary>
    ///     Test that the IO subsystem can serialize TestResults to JUnit content
    /// </summary>
    /// <remarks>
    ///     Tests that the IO subsystem correctly serializes an in-memory TestResults object
    ///     to JUnit-format content. Proves that the subsystem end-to-end flow works for JUnit output.
    /// </remarks>
    [Fact]
    public void IO_Serialize_TestResults_ProducesJUnitContent()
    {
        // Arrange: in-memory TestResults with one passing test
        var results = new TestResults
        {
            Name = "MySuite",
            Results =
            [
                new TestResult { Name = "Test1", Outcome = TestOutcome.Passed }
            ]
        };

        // Act: serialize the TestResults to JUnit content
        var content = JUnitSerializer.Serialize(results);

        // Assert: serialized content should be valid JUnit XML containing the test data
        Assert.NotNull(content);
        Assert.Contains("testsuites", content);
        Assert.Contains("Test1", content);
    }

    /// <summary>
    ///     Test that the IO subsystem returns Unknown for null content
    /// </summary>
    /// <remarks>
    ///     Tests that Identify handles null input gracefully without throwing.
    ///     Proves that callers can safely pass null without exception handling.
    /// </remarks>
    [Fact]
    public void IO_Identify_NullContent_ReturnsUnknown()
    {
        // Act: identify null content
        var format = Serializer.Identify(null!);

        // Assert: format should be Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that the IO subsystem returns Unknown for invalid XML content
    /// </summary>
    /// <remarks>
    ///     Tests that Identify handles malformed XML gracefully without throwing.
    ///     Proves that callers are not required to pre-validate XML before calling Identify.
    /// </remarks>
    [Fact]
    public void IO_Identify_InvalidXmlContent_ReturnsUnknown()
    {
        // Act: identify malformed XML content
        var format = Serializer.Identify("this is not xml");

        // Assert: format should be Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that the IO subsystem returns Unknown for unrecognized XML content
    /// </summary>
    /// <remarks>
    ///     Tests that Identify returns Unknown for valid XML that does not match any known format.
    ///     Proves that unrecognized formats are reported as Unknown, not as an error.
    /// </remarks>
    [Fact]
    public void IO_Identify_UnknownXmlContent_ReturnsUnknown()
    {
        // Act: identify valid XML that is not a known format
        var format = Serializer.Identify("<unknown />");

        // Assert: format should be Unknown
        Assert.Equal(TestResultFormat.Unknown, format);
    }

    /// <summary>
    ///     Test that the IO subsystem throws ArgumentNullException for null content
    /// </summary>
    /// <remarks>
    ///     Tests that Deserialize rejects null input with a clear ArgumentNullException.
    ///     Proves that null inputs are caught at the entry point before any XML parsing.
    /// </remarks>
    [Fact]
    public void IO_Deserialize_NullContent_ThrowsArgumentNullException()
    {
        // Act and Assert: deserializing null should throw ArgumentNullException
        Assert.Throws<ArgumentNullException>(() => Serializer.Deserialize(null!));
    }

    /// <summary>
    ///     Test that the IO subsystem throws ArgumentException for whitespace content
    /// </summary>
    /// <remarks>
    ///     Tests that Deserialize rejects whitespace-only input with a clear ArgumentException.
    ///     Proves that empty/blank inputs are caught before any XML parsing attempt.
    /// </remarks>
    [Fact]
    public void IO_Deserialize_WhitespaceContent_ThrowsArgumentException()
    {
        // Act and Assert: deserializing whitespace should throw ArgumentException
        Assert.Throws<ArgumentException>(() => Serializer.Deserialize("   "));
    }

    /// <summary>
    ///     Test that the IO subsystem throws InvalidOperationException for unrecognized XML content
    /// </summary>
    /// <remarks>
    ///     Tests that Deserialize rejects valid XML with an unrecognized root element by throwing
    ///     InvalidOperationException. Proves that the subsystem correctly reports unrecognized
    ///     formats rather than silently returning empty results.
    ///     Satisfies requirement <c>TestResults-IO-Deserialize</c>.
    /// </remarks>
    [Fact]
    public void IO_Deserialize_UnknownContent_ThrowsInvalidOperationException()
    {
        // Arrange: valid XML with an unrecognized root element
        const string content = """
            <?xml version="1.0"?>
            <UnknownRoot>
              <SomeData>value</SomeData>
            </UnknownRoot>
            """;

        // Act and Assert: deserializing unrecognized content should throw InvalidOperationException
        Assert.Throws<InvalidOperationException>(() => Serializer.Deserialize(content));
    }

    /// <summary>
    ///     Tests that core test data is preserved when TRX results are round-tripped through JUnit.
    /// </summary>
    /// <remarks>
    ///     Proves that test count, names, class names, and outcomes (for outcomes with a direct
    ///     JUnit mapping) survive a TRX → JUnit conversion. This validates the cross-format
    ///     conversion path used by callers that need to re-emit test results in JUnit format
    ///     after reading a TRX file. This test exercises TrxSerializer, JUnitSerializer, and
    ///     Serializer together and therefore belongs at the IO subsystem level.
    /// </remarks>
    [Fact]
    public void IO_TrxSerializedResults_RoundTripsViaJUnit_PreservesCoreTestData()
    {
        // Arrange: create test results with Passed, Failed, and NotExecuted outcomes
        var original = new TestResults
        {
            Name = "CrossFormatRun",
            UserName = "cross.format.user",
            Results =
            [
                new TestResult
                {
                    Name = "PassedTest",
                    ClassName = "Suite.PassedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.Passed,
                    StartTime = new DateTime(2025, 4, 1, 9, 0, 0, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(1.0)
                },
                new TestResult
                {
                    Name = "FailedTest",
                    ClassName = "Suite.FailedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.Failed,
                    StartTime = new DateTime(2025, 4, 1, 9, 0, 2, DateTimeKind.Utc),
                    Duration = TimeSpan.FromSeconds(0.5),
                    ErrorMessage = "Assertion failed"
                },
                new TestResult
                {
                    Name = "SkippedTest",
                    ClassName = "Suite.SkippedClass",
                    CodeBase = "path/to/TestAssembly.dll",
                    ComputerName = "BuildAgent01",
                    Outcome = TestOutcome.NotExecuted,
                    StartTime = new DateTime(2025, 4, 1, 9, 0, 4, DateTimeKind.Utc),
                    Duration = TimeSpan.Zero
                }
            ]
        };

        // Act: serialize to TRX, deserialize back, then re-serialize to JUnit, then deserialize again
        var trxContent = TrxSerializer.Serialize(original);
        var fromTrx = Serializer.Deserialize(trxContent);
        var junitContent = JUnitSerializer.Serialize(fromTrx);
        var fromJUnit = Serializer.Deserialize(junitContent);

        // Assert: core structure is preserved through the conversion chain
        Assert.NotNull(fromJUnit);
        Assert.Equal(3, fromJUnit.Results.Count);

        // Assert: PassedTest core data is preserved
        var passed = fromJUnit.Results.First(r => r.Name == "PassedTest");
        Assert.Equal("Suite.PassedClass", passed.ClassName);
        Assert.Equal(TestOutcome.Passed, passed.Outcome);

        // Assert: FailedTest core data is preserved
        var failed = fromJUnit.Results.First(r => r.Name == "FailedTest");
        Assert.Equal("Suite.FailedClass", failed.ClassName);
        Assert.Equal(TestOutcome.Failed, failed.Outcome);

        // Assert: SkippedTest core data is preserved
        var skipped = fromJUnit.Results.First(r => r.Name == "SkippedTest");
        Assert.Equal("Suite.SkippedClass", skipped.ClassName);
        Assert.Equal(TestOutcome.NotExecuted, skipped.Outcome);
    }
}

