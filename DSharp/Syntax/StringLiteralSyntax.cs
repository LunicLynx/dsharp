namespace DSharp.Syntax
{
    internal class StringLiteralSyntax : SyntaxNode
    {
        public Token CurrentToken { get; }

        public StringLiteralSyntax(Token currentToken)
        {
            CurrentToken = currentToken;
        }
    }
}