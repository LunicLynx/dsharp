namespace DSharp.Syntax
{
    public class ArrayTypeNode : SyntaxNode
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

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitArrayType(this);
        }
    }
}