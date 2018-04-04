using System.Collections.Generic;

namespace DSharp
{
    public class ParameterModel
    {
        public IDeclaration Type { get; }
        public string Name { get; }

        public ParameterModel(IDeclaration type, string name)
        {
            Type = type;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            var model = obj as ParameterModel;
            return model != null &&
                   EqualityComparer<IDeclaration>.Default.Equals(Type, model.Type) &&
                   Name == model.Name;
        }
    }
}