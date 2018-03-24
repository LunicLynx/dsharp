namespace DSharp
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