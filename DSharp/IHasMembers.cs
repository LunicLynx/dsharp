using System.Collections.Generic;

namespace DSharp
{
    public interface IHasMembers
    {
        IList<MemberModel> Members { get; }
    }
}