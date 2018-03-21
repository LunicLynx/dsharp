using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DSharp
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _index = 0;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.ToList();
        }

        public void Parse()
        {
            var unitNode = ParseUnit();
        }

        private (IList<UsingNode> usings, IList<SyntaxNode> members) ParseNamespaceBody()
        {
            var usings = new List<UsingNode>();
            var members = new List<SyntaxNode>();

            while (true)
            {
                //EnsureTokenType(TokenType.UsingKeyword, TokenType.NamespaceKeyword, TokenType.EndOfFile);
                switch (CurrentToken.TokenType)
                {
                    case TokenType.UsingKeyword:
                        var usingNode = ParseUsing();
                        usings.Add(usingNode);
                        break;
                    case TokenType.NamespaceKeyword:
                        var ns = ParseNamespace();
                        members.Add(ns);
                        break;
                    case TokenType.ClassKeyword:
                        var c = ParseClass();
                        members.Add(c);
                        break;
                    case TokenType.EndOfFile:
                    case TokenType.RightBrace:
                        break;
                    default:
                        // TODO error
                        NextToken();
                        break;
                }

                if (CurrentToken.TokenType == TokenType.EndOfFile || CurrentToken.TokenType == TokenType.RightBrace)
                    break;
            }
            return (usings, members);
        }

        private SyntaxNode ParseClass()
        {
            var classToken = CurrentToken;
            NextToken();

            var identifierToken = CurrentToken;
            NextToken();

            // TODO class inheritance crap

            EnsureTokenType(TokenType.LeftBrace);
            var leftBraceToken = CurrentToken;
            NextToken();

            var members = new List<SyntaxNode>();
            while (CurrentToken.TokenType != TokenType.RightBrace)
            {
                var member = ParseClassMember();
                members.Add(member);
            }

            var rightBraceToken = CurrentToken;
            NextToken();

            return new ClassSyntaxNode(classToken, identifierToken, leftBraceToken, members, rightBraceToken);
        }

        private SyntaxNode ParseClassMember()
        {
            Token modifierToken = null;

            var modifiers = new[] { TokenType.StaticKeyword };
            if (modifiers.Contains(CurrentToken.TokenType))
            {
                modifierToken = CurrentToken;
                NextToken();
            }

            var typeName = ParseType();

            var identifierToken = CurrentToken;
            NextToken();

            // { -> prop
            // ; -> field
            // = -> field
            // => -> get expression prop
            // ( -> method
            // [ -> indexer

            switch (CurrentToken.TokenType)
            {
                case TokenType.LeftParanthese:
                    return ParseMethod(modifierToken, typeName, identifierToken);
            }

            return null;
        }

        private SyntaxNode ParseType()
        {
            SyntaxNode typeName;
            switch (CurrentToken.TokenType)
            {
                case TokenType.VoidKeyword:
                    var keywordTypeToken = CurrentToken;
                    NextToken();
                    typeName = new KeywordTypeNode(keywordTypeToken);
                    break;
                default:
                    typeName = ParseNameOrQualifiedName();
                    break;
            }

            // array ?
            if (CurrentToken.TokenType == TokenType.LeftBracket)
            {
                var leftBracketNode = CurrentToken;
                NextToken();
                var rightBracketNode = CurrentToken;
                NextToken();

                typeName = new ArrayTypeNode(typeName, leftBracketNode, rightBracketNode);
            }

            return typeName;
        }

        private SyntaxNode ParseMethod(Token modifierToken, SyntaxNode typeName, Token identifierToken)
        {
            var parameterList = ParseParameterList();

            // { -> Block
            // => ExpresionBody

            var body = ParseBlock();
            return new MethodDeclarationNode(modifierToken, typeName, identifierToken, parameterList, body);
        }

        private BlockStatement ParseBlock()
        {
            var leftBraceToken = CurrentToken;
            NextToken();

            var statements = new List<Statement>();

            while (CurrentToken.TokenType != TokenType.RightBrace)
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }

            var rightBraceToken = CurrentToken;
            NextToken();
            return new BlockStatement(leftBraceToken, statements, rightBraceToken);
        }

        private Statement ParseStatement()
        {
            return null;
        }

        private object ParseParameterList()
        {
            var leftParentheseToken = CurrentToken;
            NextToken();

            var parameters = new List<SyntaxNode>();
            var ends = new[] { TokenType.RightParanthese };
            while (!ends.Contains(CurrentToken.TokenType))
            {
                var parameter = ParseParameter();
                parameters.Add(parameter);
            }

            var rightParentheseToken = CurrentToken;
            NextToken();
            return new ParameterListSyntax(leftParentheseToken, parameters, rightParentheseToken);
        }

        private SyntaxNode ParseParameter()
        {
            var typeName = ParseType();
            var identifierToken = CurrentToken;
            NextToken();

            return new ParameterSyntax(typeName, identifierToken);
        }

        private UnitNode ParseUnit()
        {
            var (usings, members) = ParseNamespaceBody();
            return new UnitNode(usings, members);
        }

        private NamespaceSyntaxNode ParseNamespace()
        {
            var namespaceToken = CurrentToken;
            NextToken();
            var name = ParseNameOrQualifiedName();
            var leftBraceToken = CurrentToken;
            NextToken();
            var (usings, members) = ParseNamespaceBody();
            var rightBraceToken = CurrentToken;
            NextToken();
            return new NamespaceSyntaxNode(namespaceToken, name, leftBraceToken, usings, members, rightBraceToken);
        }

        private void NextToken()
        {
            _index++;
        }

        private Token CurrentToken => _tokens[_index];
        private Token PeekToken => _tokens[_index + 1];

        private UsingNode ParseUsing()
        {
            var usingToken = CurrentToken;
            NextToken();

            var name = ParseNameOrQualifiedName();

            EnsureTokenType(TokenType.SemiColon);
            var semicolonToken = CurrentToken;
            NextToken();

            return new UsingNode(usingToken, name, semicolonToken);
        }

        private NameSyntaxNode ParseNameOrQualifiedName()
        {
            var name = ParseName();

            while (CurrentToken.TokenType == TokenType.Dot)
            {
                name = ParseQualifiedName(name);
            }

            return name;
        }

        private QualifiedNameSyntaxNode ParseQualifiedName(NameSyntaxNode qualifier)
        {
            var dotToken = CurrentToken;
            NextToken();

            var name = ParseName();

            return new QualifiedNameSyntaxNode(qualifier, dotToken, name);
        }

        private void EnsureTokenType(params TokenType[] tokenTypes)
        {
            if (!tokenTypes.Contains(CurrentToken.TokenType))
                throw new UnexpectedTokenException(tokenTypes, CurrentToken.TokenType);
        }

        private NameSyntaxNode ParseName()
        {
            var result = new NameSyntaxNode(CurrentToken);
            NextToken();
            return result;
        }
    }

    internal class BlockStatement
    {
        public Token LeftBraceToken { get; }

        public List<Statement> Statements { get; }

        public Token RightBraceToken { get; }

        public BlockStatement(Token leftBraceToken, List<Statement> statements, Token rightBraceToken)
        {
            LeftBraceToken = leftBraceToken;
            Statements = statements;
            RightBraceToken = rightBraceToken;
        }
    }

    internal class Statement
    {
    }

    internal class MethodDeclarationNode : SyntaxNode
    {
        public Token ModifierToken { get; }

        public SyntaxNode TypeName { get; }

        public Token IdentifierToken { get; }

        public object ParameterList { get; }

        public BlockStatement Body { get; }

        public MethodDeclarationNode(Token modifierToken, SyntaxNode typeName, Token identifierToken, object parameterList, BlockStatement body)
        {
            ModifierToken = modifierToken;
            TypeName = typeName;
            IdentifierToken = identifierToken;
            ParameterList = parameterList;
            Body = body;
            throw new System.NotImplementedException();
        }
    }

    internal class ArrayTypeNode : SyntaxNode
    {
        public SyntaxNode TypeName { get; }

        public Token LeftBracketNode { get; }

        public Token RightBracketNode { get; }

        public ArrayTypeNode(SyntaxNode typeName, Token leftBracketNode, Token rightBracketNode)
        {
            TypeName = typeName;
            LeftBracketNode = leftBracketNode;
            RightBracketNode = rightBracketNode;
        }
    }

    internal class KeywordTypeNode : NameSyntaxNode
    {
        public KeywordTypeNode(Token keywordTypeToken) : base(keywordTypeToken)
        {

        }
    }

    internal class ParameterListSyntax
    {
        public Token LeftParentheseToken { get; }

        public List<SyntaxNode> Parameters { get; }

        public Token RightParentheseToken { get; }

        public ParameterListSyntax(Token leftParentheseToken, List<SyntaxNode> parameters, Token rightParentheseToken)
        {
            LeftParentheseToken = leftParentheseToken;
            Parameters = parameters;
            RightParentheseToken = rightParentheseToken;
        }
    }

    
}