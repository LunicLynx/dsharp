using System.Collections.Generic;

namespace DSharp.Syntax
{
    public class SyntaxNode
    {
        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();
    }
}