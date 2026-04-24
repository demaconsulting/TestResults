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

namespace DemaConsulting.TestResults.IO;

/// <summary>
///     String writer that reports UTF-8 as its encoding.
/// </summary>
/// <remarks>
///     <see cref="System.Xml.XmlWriter"/> reads the <see cref="System.IO.TextWriter.Encoding"/>
///     property of the underlying writer to determine which encoding declaration to emit in the
///     XML prolog. The default <see cref="StringWriter"/> reports UTF-16 (the .NET in-memory
///     string encoding), which would cause serializers to write <c>encoding="utf-16"</c> even
///     when the resulting string is later consumed or stored as UTF-8. This class overrides
///     <see cref="Encoding"/> to return <see cref="System.Text.Encoding.UTF8"/> so that the
///     XML declaration correctly declares <c>encoding="utf-8"</c>.
/// </remarks>
internal sealed class Utf8StringWriter : StringWriter
{
    /// <summary>
    ///     Gets the UTF-8 encoding, overriding the default UTF-16 reported by
    ///     <see cref="StringWriter"/> so that XML serializers emit the correct
    ///     <c>encoding="utf-8"</c> declaration in the XML prolog.
    /// </summary>
    public override Encoding Encoding => Encoding.UTF8;
}
