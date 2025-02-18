﻿// Copyright(c) 2025 DEMA Consulting
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
///     Tests for TrxSerializer class
/// </summary>
[TestClass]
public sealed class TrxSerializerTests
{
    /// <summary>
    ///     Test for basic serialization
    /// </summary>
    [TestMethod]
    public void TestBasic()
    {
        // Construct a basic test suites object
        var suites = new TestSuites
        {
            Name = "Basic",
            UserName = "user",
            Suites =
            [
                new TestSuite
                {
                    Name = "Suite",
                    Cases =
                    [
                        new TestCase
                        {
                            Name = "Test",
                            ClassName = "Class",
                            CodeBase = "Code",
                            StartTime = new DateTime(2025, 2, 18, 3, 0, 0, 0, DateTimeKind.Utc),
                            Duration = 1.0,
                            SystemOutput = "Output",
                            Outcome = TestOutcome.Passed
                        }
                    ]
                }
            ]
        };

        // Serialize the test suites object
        var result = TrxSerializer.Serialize(suites);
        Assert.IsNotNull(result);
    }
}