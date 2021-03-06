﻿namespace DSharp.Syntax
{
    public class MethodDeclarationNode : SyntaxNode
    {
        public Token ModifierToken { get; }

        public SyntaxNode TypeName { get; }

        public Token IdentifierToken { get; }

        public ParameterListSyntax ParameterList { get; }

        public BlockStatementSyntax Body { get; }

        public MethodDeclarationNode(Token modifierToken, SyntaxNode typeName, Token identifierToken, ParameterListSyntax parameterList, BlockStatementSyntax body)
        {
            ModifierToken = modifierToken;
            TypeName = typeName;
            IdentifierToken = identifierToken;
            ParameterList = parameterList;
            Body = body;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMethodDeclerationSyntax(this);
        }
    }
}