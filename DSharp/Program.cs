using System;
using System.Collections.Generic;
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
            var workspace = new Workspace();

            AddCorLib(workspace);

            var assembly = new AssemblyModel(workspace, "Test");
            workspace.Assemblies.Add(assembly);
            assembly.References.Add("corlib");

            // 1 types and nested types
            var phase1 = new SemanticModelVisitorPhase1(assembly);
            tree.Accept(phase1);

            // 2 base types and members
            var phase2 = new SemanticModelVisitorPhase2(assembly);
            tree.Accept(phase2);

            // 3 function bodies
            var phase3 = new SemanticModelVisitorPhase3(assembly);
            tree.Accept(phase3);

            var executor = new AssemblyExecutor(assembly);
            executor.Execute(assembly, "DSharp.Program::Main");
        }

        private static void AddCorLib(Workspace workspace)
        {
            var assembly = new AssemblyModel(workspace, "corlib");
            workspace.Assemblies.Add(assembly);
            var stringType = new ClassModel("System", "String");
            assembly.Members.Add(stringType);
            var voidType = new ClassModel("System", "Void");
            assembly.Members.Add(voidType);
            var classModel = new ClassModel("System", "Console");

            var mdm = new MethodDeclarationModel(voidType, "WriteLine",
                new List<ParameterModel> { new ParameterModel(stringType, "value") },
                ((Action<string>)Console.WriteLine).Method);
            classModel.Members.Add(mdm);
            assembly.Members.Add(classModel);
        }
    }
}
