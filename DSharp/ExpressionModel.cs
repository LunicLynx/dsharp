namespace DSharp
{
    class ExpressionModel
    {
        public IDeclaration Type { get; }

        public ExpressionModel(IDeclaration type)
        {
            Type = type;
        }
    }
}