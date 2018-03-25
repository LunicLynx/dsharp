namespace DSharp.Syntax
{
    internal class MemberReferenceExpressionSyntax : ExpressionSyntax
    {
        public SyntaxNode Owner { get; }

        public Token DotToken { get; }

        public SyntaxNode Member { get; }

        public MemberReferenceExpressionSyntax(SyntaxNode owner, Token dotToken, SyntaxNode member)
        {
            Owner = owner;
            DotToken = dotToken;
            Member = member;
        }
    }
}