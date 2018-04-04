using System.Collections.Generic;

namespace DSharp
{
    internal class BlockStatementModel : StatementModel
    {
        public List<StatementModel> Statements { get; }

        public BlockStatementModel(List<StatementModel> statements)
        {
            Statements = statements;
        }
    }
}