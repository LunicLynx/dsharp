using System.Collections.Generic;

namespace DSharp
{
    class InvokeMethodExpressionModel : ExpressionModel
    {
        public ExpressionModel Owner { get; }
        public MethodDeclarationModel Method { get; }
        public List<ExpressionModel> Arguments { get; }

        public InvokeMethodExpressionModel(ExpressionModel owner, MethodDeclarationModel method, List<ExpressionModel> arguments) : base(method.ReturnType)
        {
            Owner = owner;
            Method = method;
            Arguments = arguments;
        }
    }
}