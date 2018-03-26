namespace DSharp.Syntax
{
    public class StringLiteralSyntax : SyntaxNode
    {
        public Token CurrentToken { get; }

        public StringLiteralSyntax(Token currentToken)
        {
            CurrentToken = currentToken;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitStringLiteral(this);
        }
    }
}