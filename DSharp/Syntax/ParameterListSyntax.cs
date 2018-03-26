using System.Collections.Generic;

namespace DSharp.Syntax
{
    public class ParameterListSyntax : SyntaxNode
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

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitParameterList(this);
        }
    }
}