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

namespace DemaConsulting.TestResults.Tests;

/// <summary>
///     Test helpers class
/// </summary>
internal static class TestHelpers
{
    /// <summary>
    ///     Get an embedded resource as a string
    /// </summary>
    /// <param name="resourceName">Resource name</param>
    /// <returns>Resource string</returns>
    public static string GetEmbeddedResource(string resourceName)
    {
        // Open the resource
        using var stream = typeof(TestHelpers).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return string.Empty;
        }

        // Read the resource
        using var reader = new StreamReader(stream);
#if NET6_0_OR_GREATER
        return reader.ReadToEnd().ReplaceLineEndings();
#else
        return reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n");
#endif
    }
}
