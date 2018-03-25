namespace DSharp.Syntax
{
    internal class StringLiteral : SyntaxNode
    {
        public Token CurrentToken { get; }

        public StringLiteral(Token currentToken)
        {
            CurrentToken = currentToken;
        }
    }
}