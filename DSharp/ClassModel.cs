using System.Collections.Generic;

namespace DSharp
{
    public class ClassModel : IDeclaration
    {
        public string Namespace { get; }
        public string Name { get; }

        public string FullName => string.IsNullOrEmpty(Namespace)
            ? Name
            : Namespace + "." + Name;

        public IList<MemberModel> Members { get; } = new List<MemberModel>();

        public ClassModel(string ns, string name)
        {
            Namespace = ns;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var model = obj as ClassModel;
            return model != null &&
                   FullName == model.FullName;
        }
    }
}