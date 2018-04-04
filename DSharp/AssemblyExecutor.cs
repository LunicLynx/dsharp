using System;
using System.Collections.Generic;
using System.Linq;

namespace DSharp
{
    class AssemblyExecutor
    {
        private object _returnValue;
        private object _this;
        private Dictionary<string, object> _parameters;
        public AssemblyModel Model { get; }

        public AssemblyExecutor(AssemblyModel model)
        {
            Model = model;
        }

        public void Execute(AssemblyModel assembly, string entryPoint)
        {
            var strings = entryPoint.Split("::");
            var typeName = strings[0];
            var methodName = strings[1];

            var type = assembly.Members.Single(m => m.FullName == typeName);

            var method = (MethodDeclarationModel)type.Members.Single(m => m.Name == methodName);

            Execute(method, null, new object[] { new object[0] });
        }

        public object Execute(MethodDeclarationModel method, object owner, object[] args)
        {
            if (method.IsInterop)
            {
                _returnValue = method.Invoke(owner, args);
            }
            else
            {
                _this = owner;
                _parameters = new Dictionary<string, object>();
                for (var index = 0; index < method.Parameters.Count; index++)
                {
                    var parameter = method.Parameters[index];
                    _parameters[parameter.Name] = args[index];
                }
                Execute(method.Body);
            }

            return _returnValue;
        }

        private void Execute(StatementModel statement)
        {
            switch (statement)
            {
                case BlockStatementModel blockStatement:
                    Execute(blockStatement);
                    break;
                case ExpressionStatementModel expressionStatement:
                    Execute(expressionStatement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Execute(BlockStatementModel blockStatement)
        {
            foreach (var statement in blockStatement.Statements)
            {
                Execute(statement);
            }
        }

        private void Execute(ExpressionStatementModel expressionStatement)
        {
            Execute(expressionStatement.Expression);
        }

        private object Execute(ExpressionModel expression)
        {
            switch (expression)
            {
                case InvokeMethodExpressionModel invokeMethodExpression:
                    return Execute(invokeMethodExpression);
                case ConstantExpressionModel constantExpression:
                    return Execute(constantExpression);
                case TypeReferenceExpressionModel _:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private object Execute(ConstantExpressionModel constantExpressionModel)
        {
            return constantExpressionModel.Value;
        }

        private object Execute(InvokeMethodExpressionModel invokeMethodExpression)
        {
            var owner = Execute(invokeMethodExpression.Owner);
            var values = new List<object>();
            foreach (var expression in invokeMethodExpression.Arguments)
            {
                var value = Execute(expression);
                values.Add(value);
            }

            return Execute(invokeMethodExpression.Method, owner, values.ToArray());
        }
    }
}