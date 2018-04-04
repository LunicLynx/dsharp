using System.Collections.Generic;

namespace DSharp
{
    public class Workspace
    {
        public IList<AssemblyModel> Assemblies { get; } = new List<AssemblyModel>();
    }
}