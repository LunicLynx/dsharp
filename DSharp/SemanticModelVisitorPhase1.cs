using System;
using DSharp.Syntax;

namespace DSharp
{
    public class SemanticModelVisitorPhase1 : SyntaxVisitor
    {
        private AssemblyModel _model = new AssemblyModel();
        private string _name;


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
            throw new NotImplementedException();
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitName(NameSyntaxNode nameSyntaxNode)
        {
            throw new NotImplementedException();
        }

        public override void VisitMemberReferenceExpression(MemberReferenceExpressionSyntax memberReferenceExpressionSyntax)
        {
            throw new NotImplementedException();
        }

        public override void VisitMethodDeclerationSyntax(MethodDeclarationNode methodDeclarationNode)
        {
            throw new NotImplementedException();
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
    }

    internal class AssemblyModel
    {
    }
}