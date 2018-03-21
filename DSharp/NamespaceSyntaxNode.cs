using System.Collections.Generic;

namespace DSharp
{
    class NamespaceSyntaxNode : SyntaxNode
    {
        public Token NamespaceToken { get; }

        public NameSyntaxNode Name { get; }

        public Token LeftBraceToken { get; }

        public IList<UsingNode> Usings { get; }

        public IList<SyntaxNode> Members { get; }

        public Token RightBraceToken { get; }

        public NamespaceSyntaxNode(Token namespaceToken, NameSyntaxNode name, Token leftBraceToken, IList<UsingNode> usings, IList<SyntaxNode> members, Token rightBraceToken)
        {
            NamespaceToken = namespaceToken;
            Name = name;
            LeftBraceToken = leftBraceToken;
            Usings = usings;
            Members = members;
            RightBraceToken = rightBraceToken;
        }
    }
}