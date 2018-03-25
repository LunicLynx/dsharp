using System.Collections.Generic;

namespace DSharp.Syntax
{
    internal class ParameterListSyntax
    {
        public Token LeftParentheseToken { get; }

        public List<SyntaxNode> Parameters { get; }

        public Token RightParentheseToken { get; }

        public ParameterListSyntax(Token leftParentheseToken, List<SyntaxNode> parameters, Token rightParentheseToken)
        {
            LeftParentheseToken = leftParentheseToken;
            Parameters = parameters;
            RightParentheseToken = rightParentheseToken;
        }
    }
}