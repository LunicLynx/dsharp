namespace DSharp
{
    internal class ArgumentListNode : SyntaxNode
    {
        public object[] Children { get; }

        public ArgumentListNode(params object[] children)
        {
            Children = children;
        }
    }
}