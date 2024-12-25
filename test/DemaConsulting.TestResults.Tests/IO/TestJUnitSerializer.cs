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
///   Tests for the JUnit XML serializer.
/// </summary>
[TestClass]
public class TestJUnitSerializer
{
    /// <summary>
    ///     Test serializing a minimal test run.
    /// </summary>
    [TestMethod]
    public void Test_Serialize_Minimal()
    {
        // Construct the test run
        var run = new TestRun
        {
            Suites =
            [
                new TestSuite
                {
                    Name = "TestSuite1",
                    Cases = [ new TestCase { Name = "testCase1" } ]
                }
            ]
        };

        // Serialize the test run
        var text = JUnitSerializer.Serialize(run);
        Assert.IsNotNull(text);

        // Verify the results of the serialization
        Assert.AreEqual(
            """
            ﻿<?xml version="1.0" encoding="utf-8"?>
            <testsuites tests="1" failures="0" errors="0" skipped="0">
              <testsuite name="TestSuite1" tests="1" failures="0" errors="0" skipped="0">
                <testcase name="testCase1" />
              </testsuite>
            </testsuites>
            """,
            text);
    }

    /// <summary>
    ///     Test serializing a common test run.
    /// </summary>
    [TestMethod]
    public void Test_Serialize_Common()
    {
        // Construct the test run
        var run = new TestRun
        {
            TimeStamp = new DateTime(2024, 12, 25, 12, 0, 0, DateTimeKind.Utc),
            Duration = 20.0,
            Suites =
            [
                new TestSuite
                {
                    Name = "TestSuite1",
                    TimeStamp = new DateTime(2024, 12, 25, 12, 0, 0, DateTimeKind.Utc),
                    Duration = 10.0,
                    Cases =
                    [
                        new TestCase { Name = "testCase1", ClassName = "Tests.Suite1", Duration = 5.0 },
                        new TestCase { Name = "testCase2", ClassName = "Tests.Suite1", Duration = 5.0 }
                    ]
                },
                new TestSuite
                {
                    Name = "TestSuite2",
                    TimeStamp = new DateTime(2024, 12, 25, 12, 0, 10, DateTimeKind.Utc),
                    Duration = 10.0,
                    Cases =
                    [
                        new TestCase { Name = "testCase3", ClassName = "Tests.Suite2", Duration = 5.0 },
                        new TestCase { Name = "testCase4", ClassName = "Tests.Suite2", Duration = 5.0 }
                    ]
                }
            ]
        };

        // Serialize the test run
        var text = JUnitSerializer.Serialize(run);
        Assert.IsNotNull(text);

        // Verify the results of the serialization
        Assert.AreEqual(
            """
            ﻿<?xml version="1.0" encoding="utf-8"?>
            <testsuites tests="4" failures="0" errors="0" skipped="0" timestamp="2024-12-25T12:00:00Z" time="20">
              <testsuite name="TestSuite1" tests="2" failures="0" errors="0" skipped="0" timestamp="2024-12-25T12:00:00Z" time="10">
                <testcase name="testCase1" time="5" />
                <testcase name="testCase2" time="5" />
              </testsuite>
              <testsuite name="TestSuite2" tests="2" failures="0" errors="0" skipped="0" timestamp="2024-12-25T12:00:10Z" time="10">
                <testcase name="testCase3" time="5" />
                <testcase name="testCase4" time="5" />
              </testsuite>
            </testsuites>
            """,
            text);
    }

    /// <summary>
    ///     Test serializing results with output.
    /// </summary>
    [TestMethod]
    public void Test_Serialize_Output()
    {
        // Construct the test run
        var run = new TestRun
        {
            Suites =
            [
                new TestSuite
                {
                    Name = "TestSuite1",
                    Cases =
                    [
                        new TestCase
                        {
                            Name = "testCase1",
                            StdOut = ["\nLine 1\nLine 2\nLine 3\n"],
                            StdErr = ["\nTest Failure\nAnother Test Failure\n"]
                        }
                    ]
                }
            ]
        };

        // Serialize the test run
        var text = JUnitSerializer.Serialize(run);
        Assert.IsNotNull(text);

        // Verify the results of the serialization
        Assert.AreEqual(
            """
            ﻿<?xml version="1.0" encoding="utf-8"?>
            <testsuites tests="1" failures="0" errors="0" skipped="0">
              <testsuite name="TestSuite1" tests="1" failures="0" errors="0" skipped="0">
                <testcase name="testCase1">
                  <system-out><![CDATA[
            Line 1
            Line 2
            Line 3
            ]]></system-out>
                  <system-err><![CDATA[
            Test Failure
            Another Test Failure
            ]]></system-err>
                </testcase>
              </testsuite>
            </testsuites>
            """,
            text);
    }
}