namespace DSharp.Syntax
{
    internal class MethodDeclarationNode : SyntaxNode
    {
        public Token ModifierToken { get; }

        public SyntaxNode TypeName { get; }

        public Token IdentifierToken { get; }

        public object ParameterList { get; }

        public BlockStatementSyntax Body { get; }

        public MethodDeclarationNode(Token modifierToken, SyntaxNode typeName, Token identifierToken, object parameterList, BlockStatementSyntax body)
        {
            ModifierToken = modifierToken;
            TypeName = typeName;
            IdentifierToken = identifierToken;
            ParameterList = parameterList;
            Body = body;
        }
    }
}