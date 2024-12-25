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

using DemaConsulting.TestResults.IO;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///   Tests for the JUnit XML deserializer.
/// </summary>
[TestClass]
public sealed class TestJUnitDeserializer
{
    /// <summary>
    ///     Test deserializing a minimal JUnit XML file.
    /// </summary>
    [TestMethod]
    public void Test_Deserialize_Minimal()
    {
        // Deserialize the JUnit XML
        var run = JUnitDeserializer.Deserialize(
            TestHelpers.GetEmbeddedResource(
                "DemaConsulting.TestResults.Tests.IO.Examples.JUnit.minimal.xml"));

        // Assert contents
        Assert.AreEqual(1, run.Suites.Count);
        Assert.AreEqual("TestSuite1", run.Suites[0].Name);
        Assert.AreEqual(1, run.Suites[0].Cases.Count);
        Assert.AreEqual("testCase1", run.Suites[0].Cases[0].Name);

        // Assert results
        Assert.AreEqual(1, run.GetTests());
        Assert.AreEqual(0, run.GetFailures());
        Assert.AreEqual(0, run.GetErrors());
        Assert.AreEqual(0, run.GetSkipped());
        Assert.AreEqual(0.0, run.GetDuration(), 0.0001);
    }

    /// <summary>
    ///     Test deserializing a common JUnit XML file.
    /// </summary>
    [TestMethod]
    public void Test_Deserialize_Common()
    {
        // Deserialize the JUnit XML
        var run = JUnitDeserializer.Deserialize(
            TestHelpers.GetEmbeddedResource(
                "DemaConsulting.TestResults.Tests.IO.Examples.JUnit.common.xml"));

        // Assert contents
        Assert.AreEqual("Test Run", run.Name);
        Assert.AreEqual(2, run.Suites.Count);
        Assert.AreEqual("TestSuite1", run.Suites[0].Name);
        Assert.AreEqual(2, run.Suites[0].Cases.Count);
        Assert.AreEqual("testCase1", run.Suites[0].Cases[0].Name);
        Assert.AreEqual("testCase2", run.Suites[0].Cases[1].Name);
        Assert.AreEqual("TestSuite2", run.Suites[1].Name);
        Assert.AreEqual(2, run.Suites[1].Cases.Count);
        Assert.AreEqual("testCase3", run.Suites[1].Cases[0].Name);
        Assert.AreEqual("testCase4", run.Suites[1].Cases[1].Name);

        // Assert results
        Assert.AreEqual(4, run.GetTests());
        Assert.AreEqual(0, run.GetFailures());
        Assert.AreEqual(0, run.GetErrors());
        Assert.AreEqual(0, run.GetSkipped());
        Assert.AreEqual(20.0, run.GetDuration(), 0.0001);
    }

    /// <summary>
    ///     Test deserializing a common JUnit XML file.
    /// </summary>
    [TestMethod]
    public void Test_Deserialize_JenkinsStyle()
    {
        // Deserialize the JUnit XML
        var run = JUnitDeserializer.Deserialize(
            TestHelpers.GetEmbeddedResource(
                "DemaConsulting.TestResults.Tests.IO.Examples.JUnit.jenkins-style.xml"));

        // Assert contents
        Assert.AreEqual(1, run.Suites.Count);
        Assert.AreEqual("PatientRegistration.Patient Registration (Example)", run.Suites[0].Name);
        Assert.AreEqual("samplename", run.Suites[0].HostName);
        Assert.AreEqual(3, run.Suites[0].Cases.Count);
        Assert.AreEqual("Create patient", run.Suites[0].Cases[0].Name);
        Assert.AreEqual(TestOutcome.Passed, run.Suites[0].Cases[0].GetOutcome());
        Assert.AreEqual(1, run.Suites[0].Cases[0].StdOut.Length);
        StringAssert.Contains(run.Suites[0].Cases[0].StdOut[0], "Scenario: Create patient");
        Assert.AreEqual("Close patient", run.Suites[0].Cases[1].Name);
        Assert.AreEqual(TestOutcome.Passed, run.Suites[0].Cases[1].GetOutcome());
        Assert.AreEqual(1, run.Suites[0].Cases[1].StdOut.Length);
        StringAssert.Contains(run.Suites[0].Cases[1].StdOut[0], "Scenario: Close patient");
        Assert.AreEqual("Edit patient", run.Suites[0].Cases[2].Name);
        Assert.AreEqual(TestOutcome.Error, run.Suites[0].Cases[2].GetOutcome());
        Assert.AreEqual(1, run.Suites[0].Cases[2].StdOut.Length);
        StringAssert.Contains(run.Suites[0].Cases[2].StdOut[0], "Scenario: Edit patient");

        // Assert results
        Assert.AreEqual(3, run.GetTests());
        Assert.AreEqual(0, run.GetFailures());
        Assert.AreEqual(1, run.GetErrors());
        Assert.AreEqual(0, run.GetSkipped());
    }
}