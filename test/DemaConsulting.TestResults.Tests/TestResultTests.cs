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
///     Tests for the default property values of <see cref="TestResult" />
/// </summary>
[TestClass]
public class TestResultTests
{
    /// <summary>
    ///     Tests that <see cref="TestResult.TestId" /> defaults to a non-empty GUID
    /// </summary>
    [TestMethod]
    public void TestResult_TestId_Default_IsNotEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - TestId should be auto-generated and not the empty GUID
        Assert.AreNotEqual(Guid.Empty, result.TestId);
    }

    /// <summary>
    ///     Tests that two separate <see cref="TestResult" /> instances have different <see cref="TestResult.TestId" /> values,
    ///     proving that each instance generates its own unique identifier
    /// </summary>
    [TestMethod]
    public void TestResult_TestId_TwoInstances_AreUnique()
    {
        // Arrange - create two independent TestResult instances

        // Act
        var result1 = new TestResult();
        var result2 = new TestResult();

        // Assert - each instance should have a distinct TestId
        Assert.AreNotEqual(result1.TestId, result2.TestId);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.ExecutionId" /> defaults to a non-empty GUID
    /// </summary>
    [TestMethod]
    public void TestResult_ExecutionId_Default_IsNotEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - ExecutionId should be auto-generated and not the empty GUID
        Assert.AreNotEqual(Guid.Empty, result.ExecutionId);
    }

    /// <summary>
    ///     Tests that two separate <see cref="TestResult" /> instances have different
    ///     <see cref="TestResult.ExecutionId" /> values, proving that each instance generates its own unique identifier
    /// </summary>
    [TestMethod]
    public void TestResult_ExecutionId_TwoInstances_AreUnique()
    {
        // Arrange - create two independent TestResult instances

        // Act
        var result1 = new TestResult();
        var result2 = new TestResult();

        // Assert - each instance should have a distinct ExecutionId
        Assert.AreNotEqual(result1.ExecutionId, result2.ExecutionId);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.Name" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_Name_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - Name should default to string.Empty
        Assert.AreEqual(string.Empty, result.Name);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.CodeBase" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_CodeBase_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - CodeBase should default to string.Empty
        Assert.AreEqual(string.Empty, result.CodeBase);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.ClassName" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_ClassName_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - ClassName should default to string.Empty
        Assert.AreEqual(string.Empty, result.ClassName);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.ComputerName" /> defaults to the current machine name
    /// </summary>
    [TestMethod]
    public void TestResult_ComputerName_Default_IsEnvironmentMachineName()
    {
        // Arrange - record the expected machine name before construction
        var expectedComputerName = Environment.MachineName;

        // Act
        var result = new TestResult();

        // Assert - ComputerName should match the environment's machine name
        Assert.AreEqual(expectedComputerName, result.ComputerName);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.Duration" /> defaults to <see cref="TimeSpan.Zero" />
    /// </summary>
    [TestMethod]
    public void TestResult_Duration_Default_IsZero()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - Duration should default to TimeSpan.Zero
        Assert.AreEqual(TimeSpan.Zero, result.Duration);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.SystemOutput" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_SystemOutput_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - SystemOutput should default to string.Empty
        Assert.AreEqual(string.Empty, result.SystemOutput);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.SystemError" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_SystemError_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - SystemError should default to string.Empty
        Assert.AreEqual(string.Empty, result.SystemError);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.Outcome" /> defaults to <see cref="TestOutcome.NotExecuted" />
    /// </summary>
    [TestMethod]
    public void TestResult_Outcome_Default_IsNotExecuted()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - Outcome should default to TestOutcome.NotExecuted
        Assert.AreEqual(TestOutcome.NotExecuted, result.Outcome);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.ErrorMessage" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_ErrorMessage_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - ErrorMessage should default to string.Empty
        Assert.AreEqual(string.Empty, result.ErrorMessage);
    }

    /// <summary>
    ///     Tests that <see cref="TestResult.ErrorStackTrace" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResult_ErrorStackTrace_Default_IsEmpty()
    {
        // Arrange - create a new TestResult with default property values

        // Act
        var result = new TestResult();

        // Assert - ErrorStackTrace should default to string.Empty
        Assert.AreEqual(string.Empty, result.ErrorStackTrace);
    }
}
