using System.Collections.Generic;

namespace DSharp.Syntax
{
    public class BlockStatementSyntax : SyntaxNode
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

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }
    }
}