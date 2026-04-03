using System;
using System.IO;

class Program
{
    static void Main()
    {
        var sw = new StringWriter();
        sw.Write("test");
        Console.WriteLine($"Before dispose: {sw}");
        sw.Dispose();
        Console.WriteLine($"After dispose: {sw}");
        Console.WriteLine("StringWriter still works after Dispose - no unmanaged resources");
    }
}
