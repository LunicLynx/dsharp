namespace DSharp.Syntax
{
    internal class MemberReferenceExpression : Expression
    {
        public SyntaxNode Owner { get; }

        public Token DotToken { get; }

        public SyntaxNode Member { get; }

        public MemberReferenceExpression(SyntaxNode owner, Token dotToken, SyntaxNode member)
        {
            Owner = owner;
            DotToken = dotToken;
            Member = member;
        }
    }
}