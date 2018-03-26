namespace DSharp.Syntax
{
    public class NameSyntaxNode : SyntaxNode
    {
        public Token IdentifierToken { get; }

        public NameSyntaxNode(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitName(this);
        }
    }
}