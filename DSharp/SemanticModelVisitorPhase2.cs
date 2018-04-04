using System;
using System.Collections.Generic;
using DSharp.Syntax;

namespace DSharp
{
    class SemanticModelVisitorPhase2 : SyntaxVisitor
    {
        public AssemblyModel Assembly { get; }

        private IHasMembers _owner;
        private Scope _scope;
        private string _name;
        private IDeclaration _type;
        private List<ParameterModel> _parameters;
        private ParameterModel _parameter;

        public SemanticModelVisitorPhase2(AssemblyModel assembly)
        {
            Assembly = assembly;
        }

        public override void VisitArgumentList(ArgumentListNode argumentListNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitArrayType(ArrayTypeNode arrayTypeNode)
        {
            arrayTypeNode.TypeName.Accept(this);
            var typeName = _name;

            var declaration = ResolveType(typeName);

            _name = _name + "[]";
            _type = new ClassModel(declaration.Namespace, declaration.Name + "[]");
        }

        public override void VisitBlockStatement(BlockStatementSyntax blockStatementSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitClass(ClassSyntaxNode classSyntaxNode)
        {
            var name = classSyntaxNode.IdentifierToken.Content;
            _owner = (IHasMembers)Resolve(name);

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

            var returnType = ResolveType(typeName);

            var methodName = methodDeclarationNode.IdentifierToken.Content;
            methodDeclarationNode.ParameterList.Accept(this);

            var model = new MethodDeclarationModel(returnType, methodName, _parameters);
            _owner.Members.Add(model);

            // phase 3
            // methodDeclarationNode.Body.Accept(this);
        }

        private ClassModel ResolveType(string typeName)
        {
            if (typeName == "void")
                typeName = "System.Void";
            if (typeName == "string")
                typeName = "System.String";

            var declaration = (ClassModel)_scope.Resolve(typeName);
            return declaration;
        }

        public override void VisitNamespace(NamespaceSyntaxNode namespaceSyntaxNode)
        {
            namespaceSyntaxNode.Name.Accept(this);
            var name = _name;
            _scope = new Scope(_scope, name);
            foreach (var member in namespaceSyntaxNode.Members)
            {
                member.Accept(this);
            }

            _scope = _scope.Parent;
        }

        public override void VisitParameterList(ParameterListSyntax parameterListSyntax)
        {
            var paras = new List<ParameterModel>();
            foreach (var parameter in parameterListSyntax.Parameters)
            {
                parameter.Accept(this);
                paras.Add(_parameter);
            }

            _parameters = paras;
        }

        public override void VisitParameter(ParameterSyntax parameterSyntax)
        {
            parameterSyntax.TypeName.Accept(this);
            var typeName = _name;
            var type = _type;
            var name = parameterSyntax.IdentifierToken.Content;
            _parameter = new ParameterModel(type, name);
        }

        public override void VisitQualifiedName(QualifiedNameSyntaxNode qualifiedNameSyntaxNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitStringLiteral(StringLiteralSyntax stringLiteralSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitUnit(UnitNode unitNode)
        {
            _scope = new Scope(Assembly);

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
            return _scope.Resolve(name);
        }
    }
}