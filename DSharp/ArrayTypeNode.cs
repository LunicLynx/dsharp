namespace DSharp
{
    internal class ArrayTypeNode : SyntaxNode
    {
        public SyntaxNode TypeName { get; }

        public Token LeftBracketNode { get; }

        public Token RightBracketNode { get; }

        public ArrayTypeNode(SyntaxNode typeName, Token leftBracketNode, Token rightBracketNode)
        {
            TypeName = typeName;
            LeftBracketNode = leftBracketNode;
            RightBracketNode = rightBracketNode;
        }
    }
}