namespace DSharp
{
    class ExpressionStatementModel : StatementModel
    {
        public ExpressionModel Expression { get; }

        public ExpressionStatementModel(ExpressionModel expression)
        {
            Expression = expression;
        }
    }
}