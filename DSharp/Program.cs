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
            // 1 types and nested types
            var phase1 = new SemanticModelVisitorPhase1();
            tree.Accept(phase1);

            var model = phase1.Model;

            // 2 base types and members
            var phase2 = new SemanticModelVisitorPhase2(model);
            tree.Accept(phase2);


            // 3 function bodies
        }
    }

    class T
    {
        public T F { get; }
    }

    class SemanticModelVisitorPhase2 : SyntaxVisitor
    {
        public AssemblyModel Model { get; }

        public SemanticModelVisitorPhase2(AssemblyModel model)
        {
            Model = model;
        }

        public override void VisitArgumentList(ArgumentListNode argumentListNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitArrayType(ArrayTypeNode arrayTypeNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitBlockStatement(BlockStatementSyntax blockStatementSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitClass(ClassSyntaxNode classSyntaxNode)
        {

            var name = classSyntaxNode.IdentifierToken.Content;
            _owner = Resolve(name);

            foreach (var member in classSyntaxNode.Members)
            {
                member.Accept(this);
            }
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitName(NameSyntaxNode nameSyntaxNode)
        {
            _name = nameSyntaxNode.IdentifierToken.Content;
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpressionSyntax memberReferenceExpressionSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitMethodDeclerationSyntax(MethodDeclarationNode methodDeclarationNode)
        {
            methodDeclarationNode.TypeName.Accept(this);
            var typeName = _name;
            var methodName = methodDeclarationNode.IdentifierToken.Content;
            methodDeclarationNode.ParameterList.Accept(this);


            var model = new MethodDeclarationModel();
            _owner.Members.Add(model);

            // phase 3
            // methodDeclarationNode.Body.Accept(this);
        }

        public override void VisitNamespace(NamespaceSyntaxNode namespaceSyntaxNode)
        {
            namespaceSyntaxNode.Name.Accept(this);
            var name = _name;
            _scope = new Scope(_scope);
            foreach (var member in namespaceSyntaxNode.Members)
            {
                member.Accept(this);
            }

            _scope = _scope.Parent;
        }

        public override void VisitParameterList(ParameterListSyntax parameterListSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitParameter(ParameterSyntax parameterSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitQualifiedName(QualifiedNameSyntaxNode qualifiedNameSyntaxNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitStringLiteral(StringLiteralSyntax stringLiteralSyntax)
        {
            throw new NotImplementedException();
        }

        private Scope _scope;
        private string _name;

        public override void VisitUnit(UnitNode unitNode)
        {
            _scope = new Scope();

            foreach (var @using in unitNode.Usings)
            {
                @using.Accept(this);
            }

            foreach (var member in unitNode.Members)
            {
                member.Accept(this);
            }
        }

        public override void VisitUsing(UsingNode usingNode)
        {
            usingNode.Name.Accept(this);
            var name = _name;
            _scope.Usings.Add(name);
        }

        public override void VisitInvokeExpression(InvokeExpressionSyntax invokeExpressionSyntax)
        {
            throw new NotImplementedException();
        }

        public object Resolve(string name)
        {
            _scope.Resolve(name);
        }
    }

    class MethodDeclarationModel
    {

    }

    internal class Scope
    {
        public Scope Parent { get; }
        public IList<string> Usings { get; } = new List<string>();

        public Scope()
        {

        }

        public Scope(Scope parent)
        {
            Parent = parent;
        }

        public void Resolve(string name)
        {

        }
    }
}


// e.g. all references / other files
// 1 current scope 
//  - this file direct match
//  - other files quallified by this scopes namespace or partial
//  - all references with the same namespace
// 2 if scope is namespace and has using and name isn't qualified look up in usings
// 3 parent scope 
// repeat 1 - 3

class C
{

}

namespace E3
{

    class C
    {

    }
}

namespace A
{

    using B;
    //using E;



    class D : C
    {

    }
}

namespace A.B.E 
{

    class C { }
}

namespace A
{
    //class C
    //{

    //}

    namespace B
    {
        class C
        {

        }

        namespace E
        {
            //class C
            //{

            //}
        }
    }

    namespace E2
    {
        class C
        {

        }
    }
}
