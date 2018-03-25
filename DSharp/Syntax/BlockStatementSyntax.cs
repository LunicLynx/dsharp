using System.Collections.Generic;

namespace DSharp.Syntax
{
    internal class BlockStatementSyntax
    {
        public Token LeftBraceToken { get; }

        public List<StatementSyntax> Statements { get; }

        public Token RightBraceToken { get; }

        public BlockStatementSyntax(Token leftBraceToken, List<StatementSyntax> statements, Token rightBraceToken)
        {
            LeftBraceToken = leftBraceToken;
            Statements = statements;
            RightBraceToken = rightBraceToken;
        }
    }
}