using System.Collections.Generic;

namespace DSharp
{
    public class AssemblyModel
    {
        public Workspace Workspace { get; }
        public string Name { get; }
        public IList<ClassModel> Members { get; } = new List<ClassModel>();

        public IList<string> References { get; } = new List<string>();

        public AssemblyModel(Workspace workspace, string name)
        {
            Workspace = workspace;
            Name = name;
        }
    }
}