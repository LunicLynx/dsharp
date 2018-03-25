namespace DSharp.Syntax
{
    internal class ParameterSyntax : SyntaxNode
    {
        public SyntaxNode TypeName { get; }

        public Token IdentifierToken { get; }

        public ParameterSyntax(SyntaxNode typeName, Token identifierToken)
        {
            TypeName = typeName;
            IdentifierToken = identifierToken;
        }
    }
}