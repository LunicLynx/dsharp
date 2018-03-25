namespace DSharp.Syntax
{
    class QualifiedNameSyntaxNode : NameSyntaxNode
    {
        public NameSyntaxNode Qualifier { get; }
        public Token DotToken { get; }

        public QualifiedNameSyntaxNode(NameSyntaxNode qualifier, Token dotToken, NameSyntaxNode name) : base(name.IdentifierToken)
        {
            Qualifier = qualifier;
            DotToken = dotToken;
        }
    }
}