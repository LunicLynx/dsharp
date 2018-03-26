namespace DSharp.Syntax
{
    public abstract class SyntaxVisitor
    {
        public abstract void VisitArgumentList(ArgumentListNode argumentListNode);

        public abstract void VisitArrayType(ArrayTypeNode arrayTypeNode);

        public abstract void VisitBlockStatement(BlockStatementSyntax blockStatementSyntax);

        public abstract void VisitClass(ClassSyntaxNode classSyntaxNode);

        public abstract void VisitExpressionStatement(ExpressionStatementSyntax expressionStatementSyntax);

        public abstract void VisitName(NameSyntaxNode nameSyntaxNode);

        public abstract void VisitMemberReferenceExpression(
            MemberReferenceExpressionSyntax memberReferenceExpressionSyntax);

        public abstract void VisitMethodDeclerationSyntax(MethodDeclarationNode methodDeclarationNode);

        public abstract void VisitNamespace(NamespaceSyntaxNode namespaceSyntaxNode);

        public abstract void VisitParameterList(ParameterListSyntax parameterListSyntax);

        public abstract void VisitParameter(ParameterSyntax parameterSyntax);

        public abstract void VisitQualifiedName(QualifiedNameSyntaxNode qualifiedNameSyntaxNode);

        public abstract void VisitStringLiteral(StringLiteralSyntax stringLiteralSyntax);

        public abstract void VisitUnit(UnitNode unitNode);

        public abstract void VisitUsing(UsingNode usingNode);
    }
}