namespace DSharp.Syntax
{
    public class StringLiteralSyntax : SyntaxNode
    {
        public Token StringLiteralToken { get; }

        public StringLiteralSyntax(Token stringLiteralToken)
        {
            StringLiteralToken = stringLiteralToken;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitStringLiteral(this);
        }
    }
}