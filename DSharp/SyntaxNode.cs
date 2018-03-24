using System.Collections.Generic;

namespace DSharp
{
    public class SyntaxNode
    {
        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();
    }
}