using System.Collections.Generic;

namespace DSharp
{
    class SyntaxNode
    {
        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();
    }
}