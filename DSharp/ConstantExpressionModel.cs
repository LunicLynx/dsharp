namespace DSharp
{
    class ConstantExpressionModel : ExpressionModel
    {
        public object Value { get; }

        public ConstantExpressionModel(IDeclaration type, object value) : base(type)
        {
            Value = value;
        }
    }
}