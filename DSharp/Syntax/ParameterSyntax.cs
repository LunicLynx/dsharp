namespace DSharp.Syntax
{
    public class ParameterSyntax : SyntaxNode
    {
        public SyntaxNode TypeName { get; }

        public Token IdentifierToken { get; }

        public ParameterSyntax(SyntaxNode typeName, Token identifierToken)
        {
            TypeName = typeName;
            IdentifierToken = identifierToken;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitParameter(this);
        }
    }
}