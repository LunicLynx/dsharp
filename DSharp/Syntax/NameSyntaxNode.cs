namespace DSharp.Syntax
{
    class NameSyntaxNode : SyntaxNode
    {
        public Token IdentifierToken { get; }

        public NameSyntaxNode(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }
    }
}