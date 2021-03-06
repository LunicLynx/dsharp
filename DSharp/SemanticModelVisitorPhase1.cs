﻿using System;
using System.Linq;
using System.Linq.Expressions;
using DSharp.Syntax;

namespace DSharp
{
    /// <summary>
    /// Creates the skeleton of everything that isn't dependend on anything else. So in all only the types
    /// without any basetypes or members. And also nested types.
    /// </summary>
    public class SemanticModelVisitorPhase1 : SyntaxVisitor
    {
        public AssemblyModel Assembly { get; }
        private string _name;

        public SemanticModelVisitorPhase1(AssemblyModel assembly)
        {
            Assembly = assembly;
        }

        public override void VisitArgumentList(ArgumentListNode argumentListNode)
        {
            var arguments = argumentListNode.Children.OfType<SyntaxNode>();
            foreach (var argument in arguments)
            {
                argument.Accept(this);
            }
        }

        public override void VisitArrayType(ArrayTypeNode arrayTypeNode)
        {
            arrayTypeNode.TypeName.Accept(this);
        }

        public override void VisitBlockStatement(BlockStatementSyntax blockStatementSyntax)
        {
            foreach (var statement in blockStatementSyntax.Statements)
            {
                statement.Accept(this);
            }
        }

        //private ClassModel _parent;

        public override void VisitClass(ClassSyntaxNode classSyntaxNode)
        {
            var ns = _name;
            var name = classSyntaxNode.IdentifierToken.Content;

            var classModel = new ClassModel(ns, name);
            //_parent = classModel;
            Assembly.Members.Add(classModel);

            /*foreach (var member in classSyntaxNode.Members)
            {
                member.Accept(this);
            }*/
        }

        private ExpressionModel _expression;

        public override void VisitExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax)
        {
            new ExpressionStatementModel(_expression);
            expressionStatementSyntax.Expression.Accept(this);
        }

        public override void VisitName(NameSyntaxNode nameSyntaxNode)
        {
            _name = nameSyntaxNode.IdentifierToken.Content;
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpressionSyntax memberReferenceExpressionSyntax)
        {
            memberReferenceExpressionSyntax.Owner.Accept(this);
            memberReferenceExpressionSyntax.Member.Accept(this);
        }

        public override void VisitMethodDeclerationSyntax(MethodDeclarationNode methodDeclarationNode)
        {
            throw new NotImplementedException();
            //methodDeclarationNode.TypeName.Accept(this);

            //var name = methodDeclarationNode.IdentifierToken.Content;

            //var model = new MethodModel(name);
            //_parent.Members.Add(model);

            //methodDeclarationNode.ParameterList.Accept(this);
            //methodDeclarationNode.Body.Accept(this);
        }

        public override void VisitNamespace(NamespaceSyntaxNode namespaceSyntaxNode)
        {
            // figure out the whole namespace name

            namespaceSyntaxNode.Name.Accept(this);
            var name = _name;

            foreach (var member in namespaceSyntaxNode.Members)
            {
                member.Accept(this);
            }
        }

        public override void VisitParameterList(ParameterListSyntax parameterListSyntax)
        {
            foreach (var parameter in parameterListSyntax.Parameters)
            {
                parameter.Accept(this);
            }
        }

        public override void VisitParameter(ParameterSyntax parameterSyntax)
        {
            parameterSyntax.TypeName.Accept(this);
        }

        public override void VisitQualifiedName(QualifiedNameSyntaxNode qualifiedNameSyntaxNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitStringLiteral(StringLiteralSyntax stringLiteralSyntax)
        {
            //var stringLiteral = stringLiteralSyntax.StringLiteralToken.Content;
            //var str = stringLiteral.Substring(1, stringLiteral.Length - 2);

            //new ConstantExpressionModel(str);
            throw new NotImplementedException();
        }

        public override void VisitUnit(UnitNode unitNode)
        {
            foreach (var memberNode in unitNode.Members)
            {
                memberNode.Accept(this);
            }
        }

        public override void VisitUsing(UsingNode usingNode)
        {
            // not important for phase 1
            throw new NotImplementedException();
        }

        public override void VisitInvokeExpression(InvokeExpressionSyntax invokeExpressionSyntax)
        {
            invokeExpressionSyntax.Owner.Accept(this);

            // if owner is methodgroup
            // we need to find the right overload
            invokeExpressionSyntax.ArgumentList.Accept(this);
        }
    }
}