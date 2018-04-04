using System;
using System.Collections.Generic;
using System.Linq;
using DSharp.Syntax;

namespace DSharp
{
    class SemanticModelVisitorPhase3 : SyntaxVisitor
    {
        private Scope _scope;
        private string _name;
        private IHasMembers _owner;
        private IDeclaration _type;

        private List<ParameterModel> _parameters;
        private ParameterModel _parameter;
        private MemberModel _member;
        private List<ExpressionModel> _arguments;
        private ExpressionModel _value;
        private StatementModel _statement;
        private MethodGroup _methodGroup;

        public AssemblyModel Assembly { get; }

        public SemanticModelVisitorPhase3(AssemblyModel assembly)
        {
            Assembly = assembly;
        }

        public override void VisitArgumentList(ArgumentListNode argumentListNode)
        {
            var arguments = new List<ExpressionModel>();

            var o = (SyntaxNode)argumentListNode.Children[1];
            o.Accept(this);

            arguments.Add(_value);

            _arguments = arguments;
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
            var statements = new List<StatementModel>();
            foreach (var statement in blockStatementSyntax.Statements)
            {
                statement.Accept(this);
                statements.Add(_statement);
            }

            _statement = new BlockStatementModel(statements);
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
            expressionStatementSyntax.Expression.Accept(this);

            _statement = new ExpressionStatementModel(_value);
        }

        public override void VisitName(NameSyntaxNode nameSyntaxNode)
        {
            _name = nameSyntaxNode.IdentifierToken.Content;

            _type = ResolveType(_name);
            if (_type is ClassModel)
            {
                _value = new TypeReferenceExpressionModel(_type);
            }
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpressionSyntax memberReferenceExpressionSyntax)
        {
            memberReferenceExpressionSyntax.Owner.Accept(this);

            var owner = _value;

            memberReferenceExpressionSyntax.Member.Accept(this);

            var members = owner.Type.Members.Where(m => m.Name == _name);

            var first = members.First();
            if (first is MethodDeclarationModel)
            {
                _methodGroup = new MethodGroup(owner, _name, members.Cast<MethodDeclarationModel>().ToList());
            }
        }

        public override void VisitMethodDeclerationSyntax(MethodDeclarationNode methodDeclarationNode)
        {
            methodDeclarationNode.TypeName.Accept(this);

            var returnType = _type;

            var methodName = methodDeclarationNode.IdentifierToken.Content;
            methodDeclarationNode.ParameterList.Accept(this);

            //var model = new MethodDeclarationModel(returnType, methodName, _parameters);
            //_owner.Members.Add(model);
            var method = _owner.Members.OfType<MethodDeclarationModel>().Single(md => md.ReturnType == returnType && md.Name == methodName && md.Parameters.SequenceEqual(_parameters));

            // phase 3
            methodDeclarationNode.Body.Accept(this);

            var s = _statement;

            method.Body = s;
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
            throw new System.NotImplementedException();
        }

        public override void VisitStringLiteral(StringLiteralSyntax stringLiteralSyntax)
        {
            var stringLiteral = stringLiteralSyntax.StringLiteralToken.Content;
            var str = stringLiteral.Substring(1, stringLiteral.Length - 2);

            var t = ResolveType("string");
            _value = new ConstantExpressionModel(t, str);
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
            invokeExpressionSyntax.Owner.Accept(this);

            invokeExpressionSyntax.ArgumentList.Accept(this);

            var methodDeclarationModel = _methodGroup.Overloads[0];

            _value = new InvokeMethodExpressionModel(_methodGroup.Owner, methodDeclarationModel, _arguments);
        }

        public IDeclaration Resolve(string name)
        {
            return _scope.Resolve(name);
        }

        private ClassModel ResolveType(string typeName)
        {
            if (typeName == "void")
                typeName = "System.Void";
            if (typeName == "string")
                typeName = "System.String";

            var resolve = _scope.Resolve(typeName);
            if (resolve is ClassModel @class)
            {
                return @class;
            }

            return null;
        }
    }
}