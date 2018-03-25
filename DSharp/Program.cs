using System;
using System.IO;
using System.Xml;
using System.Text;

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

            var x = "asd";

            //var foo = new T();
            //var fF = foo.F.F.F;

            var scanner = new Scanner(source);
            var parser = new Parser(scanner.Scan());
            var tree = parser.Parse();

            // create semantic model


            Console.WriteLine("Hello World!");
        }

    }

    class T
    {
        public T F { get; }
    }
}
