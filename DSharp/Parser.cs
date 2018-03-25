using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DSharp.Syntax;

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

        public SyntaxNode Parse()
        {
            return ParseUnit();
        }


        private Statement ParseStatement()
        {
            return ParseExpressionStatement();

        }

        private ExpressionStatement ParseExpressionStatement()
        {
            var syntaxNode = ParseExpression(new[] { TokenType.SemiColon });
            var semiColonToken = CurrentToken;
            NextToken();
            return new ExpressionStatement(syntaxNode);
        }

        private SyntaxNode ParseExpression(TokenType[] endTypes)
        {
            SyntaxNode result = null;
            while (true)
            {
                var tokenType = CurrentToken.TokenType;
                switch (tokenType)
                {
                    case TokenType.Dot:
                        var dotToken = CurrentToken;
                        NextToken();
                        var identifierToken = new NameSyntaxNode(CurrentToken);
                        NextToken();
                        result = new MemberReferenceExpression(result, dotToken, identifierToken);
                        break;
                    case TokenType.LeftParanthese:
                        // if(result != null)
                        result = ParseArgumentList();
                        break;
                    case TokenType.Identifier:
                        result = new NameSyntaxNode(CurrentToken);
                        NextToken();
                        break;
                    case TokenType.StringLiteral:
                        result = new StringLiteral(CurrentToken);
                        NextToken();
                        break;
                }

                if (endTypes.Contains(tokenType))
                    break;
                //if (tokenType == TokenType.SemiColon)
                //break;
            }
            return result;
        }

        private SyntaxNode ParseArgumentList()
        {
            var args = new List<object>();
            var leftParantheseToken = CurrentToken;
            NextToken();

            args.Add(leftParantheseToken);

            while (CurrentToken.TokenType != TokenType.RightParanthese)
            {
                //if(CurrentToken.TokenType == TokenType.Comma)
                args.Add(ParseExpression(new [] {/*TokenType.Comma,*/ TokenType.RightParanthese}));
            }

            var rightParantheseToken = CurrentToken;
            NextToken();
            args.Add(rightParantheseToken);

            //return (ArgumentListNode)Activator.CreateInstance(typeof(ArgumentListNode), args.ToArray());
            return new ArgumentListNode(args.ToArray());
        }

        private bool IsInvokeExpression(Stack<object> stack)
        {
            var objects = stack.ToArray();
            return
                objects.Length >= 2 &&
                objects[0] is Token leftParantheseToken && leftParantheseToken.TokenType == TokenType.LeftParanthese &&
                objects[1] is SyntaxNode;
        }

        private bool IsIdentifier(Stack<object> stack)
        {
            return stack.Peek() is Token token && token.TokenType == TokenType.Identifier;
        }

        private bool IsMemberReference(Stack<object> stack)
        {

            var objects = stack.ToArray();

            return
                objects.Length >= 3 &&
                (objects[0] is NameSyntaxNode /* || Expression */) &&
                objects[1] is Token dotToken && dotToken.TokenType == TokenType.Dot &&
                objects[2] is NameSyntaxNode;
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
}