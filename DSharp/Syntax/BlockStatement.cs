using System.Collections.Generic;

namespace DSharp.Syntax
{
    internal class BlockStatement
    {
        public Token LeftBraceToken { get; }

        public List<Statement> Statements { get; }

        public Token RightBraceToken { get; }

        public BlockStatement(Token leftBraceToken, List<Statement> statements, Token rightBraceToken)
        {
            LeftBraceToken = leftBraceToken;
            Statements = statements;
            RightBraceToken = rightBraceToken;
        }
    }
}