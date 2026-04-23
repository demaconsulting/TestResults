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

using System.Text;
using DemaConsulting.TestResults.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DemaConsulting.TestResults.Tests.IO;

/// <summary>
///     Tests for SerializerHelpers unit
/// </summary>
[TestClass]
public sealed class SerializerHelpersTests
{
    /// <summary>
    ///     Test that Utf8StringWriter reports UTF-8 as its encoding
    /// </summary>
    [TestMethod]
    public void Utf8StringWriter_Encoding_ReturnsUtf8()
    {
        // Arrange: create a Utf8StringWriter
        using var writer = new Utf8StringWriter();

        // Act: read the reported encoding
        var encoding = writer.Encoding;

        // Assert: encoding is UTF-8
        Assert.AreEqual(Encoding.UTF8, encoding);
    }
}
