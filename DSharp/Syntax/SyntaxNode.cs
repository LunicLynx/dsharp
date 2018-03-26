using System.Collections.Generic;

namespace DSharp.Syntax
{
    public abstract class SyntaxNode
    {
        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();

        public abstract void Accept(SyntaxVisitor visitor);
    }
}