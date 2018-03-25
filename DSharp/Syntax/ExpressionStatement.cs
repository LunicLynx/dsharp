namespace DSharp.Syntax
{
    internal class ExpressionStatement : Statement
    {
        public SyntaxNode Expression { get; }

        public ExpressionStatement(SyntaxNode expression)
        {
            Expression = expression;
        }
    }
}