using System;
using System.Collections.Generic;
using System.Reflection;

namespace DSharp
{
    class MethodDeclarationModel : MemberModel
    {
        public MethodInfo MethodInfo { get; }
        public IDeclaration ReturnType { get; }
        public IList<ParameterModel> Parameters { get; }
        public StatementModel Body { get; set; }

        public MethodDeclarationModel(IDeclaration returnType, string name, IList<ParameterModel> parameters) : base(name)
        {
            ReturnType = returnType;
            Parameters = parameters;
        }

        public MethodDeclarationModel(IDeclaration returnType, string name, IList<ParameterModel> parameters, MethodInfo methodInfo) : base(name)
        {
            ReturnType = returnType;
            Parameters = parameters;
            MethodInfo = methodInfo;
            IsInterop = true;
        }

        public bool IsInterop { get; set; }

        public object Invoke(object self, object[] parameters)
        {
            return MethodInfo.Invoke(self, parameters);
        }
    }
}