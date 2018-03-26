using System;
using System.IO;
using System.Xml;
using System.Text;
using DSharp.Syntax;

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

            BuildSemanticModel(tree);

            // create semantic model


            Console.WriteLine("Hello World!");
        }

        private static void BuildSemanticModel(SyntaxNode tree)
        {
            // 1 types and nested types
            var phase1 = new SemanticModelVisitorPhase1();
            tree.Accept(phase1);

            // 2 base types and members

            // 3 function bodies
        }
    }

    class T
    {
        public T F { get; }
    }
}
