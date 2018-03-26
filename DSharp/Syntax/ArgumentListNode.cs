namespace DSharp.Syntax
{
    public class ArgumentListNode : SyntaxNode
    {
        public object[] Children { get; }
        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitArgumentList(this);
        }

        public ArgumentListNode(params object[] children)
        {
            Children = children;
        }
    }
}