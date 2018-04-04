using System.Collections.Generic;

namespace DSharp
{
    public class VirtualDecl : IDeclaration
    {
        public string FullName { get; }

        public VirtualDecl(object members)
        {

        }

        public IList<MemberModel> Members { get; }
    }
}