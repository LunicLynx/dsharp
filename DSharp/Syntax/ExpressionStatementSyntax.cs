namespace DSharp.Syntax
{
    internal class ExpressionStatementSyntax : StatementSyntax
    {
        public SyntaxNode Expression { get; }

        public ExpressionStatementSyntax(SyntaxNode expression)
        {
            Expression = expression;
        }
    }
}