using System.Collections.Generic;

namespace DSharp
{
    class MethodGroup
    {
        public ExpressionModel Owner { get; }
        public string Name { get; }
        public IList<MethodDeclarationModel> Overloads { get; }

        public MethodGroup(ExpressionModel owner, string name, IList<MethodDeclarationModel> overloads)
        {
            Owner = owner;
            Name = name;
            Overloads = overloads;
        }
    }
}