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
///     Tests for the default property values of <see cref="TestResults" />
/// </summary>
[TestClass]
public class TestResultsTests
{
    /// <summary>
    ///     Tests that <see cref="TestResults.Id" /> defaults to a non-empty GUID
    /// </summary>
    [TestMethod]
    public void TestResults_Id_Default_IsNotEmpty()
    {
        // Arrange: create a new TestResults with default property values

        // Act: create a new TestResults instance
        var results = new TestResults();

        // Assert: Id should be auto-generated and not the empty GUID
        Assert.AreNotEqual(Guid.Empty, results.Id);
    }

    /// <summary>
    ///     Tests that two separate <see cref="TestResults" /> instances have different <see cref="TestResults.Id" /> values,
    ///     proving that each instance generates its own unique identifier
    /// </summary>
    [TestMethod]
    public void TestResults_Id_TwoInstances_AreUnique()
    {
        // Arrange: create two independent TestResults instances

        // Act: create two independent TestResults instances
        var results1 = new TestResults();
        var results2 = new TestResults();

        // Assert: each instance should have a distinct Id
        Assert.AreNotEqual(results1.Id, results2.Id);
    }

    /// <summary>
    ///     Tests that <see cref="TestResults.Name" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResults_Name_Default_IsEmpty()
    {
        // Arrange: create a new TestResults with default property values

        // Act: create a new TestResults instance
        var results = new TestResults();

        // Assert: Name should default to string.Empty
        Assert.AreEqual(string.Empty, results.Name);
    }

    /// <summary>
    ///     Tests that <see cref="TestResults.UserName" /> defaults to an empty string
    /// </summary>
    [TestMethod]
    public void TestResults_UserName_Default_IsEmpty()
    {
        // Arrange: create a new TestResults with default property values

        // Act: create a new TestResults instance
        var results = new TestResults();

        // Assert: UserName should default to string.Empty
        Assert.AreEqual(string.Empty, results.UserName);
    }

    /// <summary>
    ///     Tests that <see cref="TestResults.Results" /> defaults to a non-null empty list
    /// </summary>
    [TestMethod]
    public void TestResults_Results_Default_IsNotNull()
    {
        // Arrange: create a new TestResults with default property values

        // Act: create a new TestResults instance
        var results = new TestResults();

        // Assert: Results list should be initialized (not null)
        Assert.IsNotNull(results.Results);
    }

    /// <summary>
    ///     Tests that <see cref="TestResults.Results" /> defaults to an empty list
    /// </summary>
    [TestMethod]
    public void TestResults_Results_Default_IsEmpty()
    {
        // Arrange: create a new TestResults with default property values

        // Act: create a new TestResults instance
        var results = new TestResults();

        // Assert: Results list should contain no elements
        Assert.HasCount(0, results.Results);
    }
}
