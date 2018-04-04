using System.Collections.Generic;
using System.Linq;

namespace DSharp
{
    public class Scope
    {
        private AssemblyModel _assembly;
        public AssemblyModel Assembly => _assembly ?? Root.Assembly;
        public Scope Parent { get; }
        public string Name { get; }
        public IList<string> Usings { get; } = new List<string>();
        public IDictionary<string, string> UsingAlias { get; } = new Dictionary<string, string>();

        public Scope Root => Parent ?? this;

        public string FullName
        {
            get
            {
                if (Parent == null) return string.Empty;
                if (Parent.FullName == string.Empty) return Name;
                return Parent.FullName + "." + Name;
            }
        }

        public Scope(AssemblyModel assembly)
        {
            _assembly = assembly;
        }

        public Scope(Scope parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public IDeclaration Resolve(string name)
        {
            // 1. exists in scope local (including using alias, more then one -> ambiguity)
            // 2. exists in scope in reference (more then once -> ambiguity)
            // 3. exists via using (same rules as 1-2) also if found through more then one using local or reference -> ambiguity
            // 4. lookup in parent using 1 - 3

            bool matchesAlias = UsingAlias.ContainsKey(name);

            var decl = Root.ResolveLocalOrReference(string.IsNullOrEmpty(FullName) ? name : FullName + "." + name);
            if (decl != null && matchesAlias)
                ; // ambiguity between local and alias
            if (decl != null)
                return decl;

            // nothing found? // replace alias
            if (matchesAlias)
            {
                var alias = UsingAlias[name];
                decl = Root.ResolveLocalOrReference(string.IsNullOrEmpty(FullName) ? alias : FullName + "." + alias);
            }

            if (decl != null)
                return decl;

            // go throug usings 
            var decls = new List<IDeclaration>();
            foreach (var @using in Usings)
            {
                var d = Root.ResolveLocalOrReference(@using + "." + name);
                // decl must be a type
                // if more then one using resolves to a type -> ambiguity
                if (d != null)
                {
                    decls.Add(d);
                }
            }

            if (decls.Count > 1)
                ; // ambiguity

            if (decls.Count == 1)
                decl = decls.Single();

            return decl ?? Parent?.Resolve(name);
        }

        public Scope Add(string name)
        {
            return new Scope(this, name);
        }

        public IDeclaration ResolveLocalOrReference(string name)
        {
            // resolve local
            var members = Assembly.Members.Where(m => m.FullName.StartsWith(name));
            if (!members.Any())
            {
                foreach (var assemblyReference in Assembly.References)
                {
                    var assembly = Assembly.Workspace.Assemblies.SingleOrDefault(a => a.Name == assemblyReference);
                    if (assembly == null) continue;
                    members = members.Concat(assembly.Members.Where(m => m.FullName.StartsWith(name)));
                }
            }

            if (!members.Any())
                return null;

            if (members.Count() == 1)
                return members.SingleOrDefault(m => m.FullName == name);
            return new VirtualDecl(members);
        }
    }
}