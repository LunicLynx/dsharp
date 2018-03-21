using System;
using System.IO;
using System.Text;
using System.Xml;

namespace DSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"
using System;

namespace DSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}
";

            var scanner = new Scanner(source);
            var parser = new Parser(scanner.Scan());
            parser.Parse();

            Console.WriteLine("Hello World!");
        }

    }
}
