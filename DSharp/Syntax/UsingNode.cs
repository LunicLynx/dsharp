namespace DSharp.Syntax
{
    public class UsingNode : SyntaxNode
    {
        public Token UsingToken { get; }

        public NameSyntaxNode Name { get; }
        public Token SemicolonToken { get; }

        public UsingNode(Token usingToken, NameSyntaxNode name, Token semicolonToken)
        {
            UsingToken = usingToken;
            Name = name;
            SemicolonToken = semicolonToken;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUsing(this);
        }
    }
}