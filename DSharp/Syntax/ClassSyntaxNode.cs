using System.Collections.Generic;

namespace DSharp.Syntax
{
    class ClassSyntaxNode : SyntaxNode
    {
        public Token ClassToken { get; }

        public Token IdentifierToken { get; }

        public Token LeftBraceToken { get; }

        public IList<SyntaxNode> Members { get; }

        public Token RightBraceToken { get; }

        public ClassSyntaxNode(Token classToken, Token identifierToken, Token leftBraceToken, IList<SyntaxNode> members, Token rightBraceToken)
        {
            ClassToken = classToken;
            IdentifierToken = identifierToken;
            LeftBraceToken = leftBraceToken;
            Members = members;
            RightBraceToken = rightBraceToken;
        }
    }
}