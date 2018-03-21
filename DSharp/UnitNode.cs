using System.Collections.Generic;

namespace DSharp
{
    class UnitNode : SyntaxNode
    {
        public IList<UsingNode> Usings { get; }

        public IList<SyntaxNode> Members { get; }

        public UnitNode(IList<UsingNode> usings, IList<SyntaxNode> members)
        {
            Usings = usings;
            Members = members;
        }
    }
}