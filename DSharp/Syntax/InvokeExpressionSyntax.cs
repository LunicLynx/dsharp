using DSharp.Syntax;

namespace DSharp
{
    public class InvokeExpressionSyntax : SyntaxNode
    {
        public SyntaxNode Owner { get; }
        public SyntaxNode ArgumentList { get; }

        public InvokeExpressionSyntax(SyntaxNode owner, SyntaxNode argumentList)
        {
            Owner = owner;
            ArgumentList = argumentList;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitInvokeExpression(this);
        }
    }
}