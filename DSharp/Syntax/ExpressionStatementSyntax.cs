namespace DSharp.Syntax
{
    public class ExpressionStatementSyntax : StatementSyntax
    {
        public SyntaxNode Expression { get; }

        public ExpressionStatementSyntax(SyntaxNode expression)
        {
            Expression = expression;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }
    }
}