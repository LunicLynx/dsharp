using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"
using System;

namespace DSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}
";

            var scanner = new Scanner(source);
            var parser = new Parser(scanner.Scan());
            parser.Parse();

            Console.WriteLine("Hello World!");
        }

    }

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
            ParseUnit();
        }

        private UnitNode ParseUnit()
        {
            var usings = new List<UsingNode>();

            while (true)
            {
                EnsureTokenType(TokenType.UsingKeyword, TokenType.NamespaceKeyword, TokenType.EndOfFile);
                switch (CurrentToken.TokenType)
                {
                    case TokenType.UsingKeyword:
                        var usingNode = ParseUsing();
                        usings.Add(usingNode);
                        break;
                    case TokenType.NamespaceKeyword:
                        var ns = ParseNamespace();
                        break;
                }

                if (CurrentToken.TokenType == TokenType.EndOfFile)
                    break;
            }
            return new UnitNode();
        }

        private NamespaceSyntaxNode ParseNamespace()
        {
            var token = CurrentToken;
            NextToken();
            var name = ParseNameOrQualifiedName();
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

            return new UsingNode(usingToken,name);
        }

        private NameSyntaxNode ParseNameOrQualifiedName()
        {
            var result = ParseName();

            while (CurrentToken.TokenType == TokenType.Dot)
            {
                NextToken();
                EnsureTokenType(TokenType.Identifier);
                result = ParseQualifiedName(result);
            }

            EnsureTokenType(TokenType.SemiColon);
            NextToken();

            return result;
        }

        private QualifiedNameSyntaxNode ParseQualifiedName(NameSyntaxNode qualifier)
        {
            var name = ParseName();

            return new QualifiedNameSyntaxNode(qualifier, name);
        }

        private void EnsureTokenType(params TokenType[] tokenTypes)
        {
            if (!tokenTypes.Contains (CurrentToken.TokenType ))
                throw new UnexpectedTokenException(tokenTypes, CurrentToken.TokenType);
        }

        private NameSyntaxNode ParseName()
        {
            var result = new NameSyntaxNode(CurrentToken);
            NextToken();
            return result;
        }
    }

    internal class UnexpectedTokenException : Exception
    {
        public TokenType[] Expected { get; }
        public TokenType Actual { get; }

        public UnexpectedTokenException(TokenType[] expected, TokenType actual)
        {
            Expected = expected;
            Actual = actual;
        }
    }

    class SyntaxNode
    {
        public IList<SyntaxNode> Children { get; } = new List<SyntaxNode>();
    }

    class UsingNode : SyntaxNode
    {
        public Token UsingToken { get; }
        public NameSyntaxNode Name { get; }

        public UsingNode(Token usingToken, NameSyntaxNode name)
        {
            UsingToken = usingToken;
            Name = name;
        }
    }

    class UnitNode : SyntaxNode
    {

    }

    class NameSyntaxNode : SyntaxNode
    {
        public Token IdentifierToken { get; }

        public NameSyntaxNode(Token identifierToken)
        {
            IdentifierToken = identifierToken;
        }
    }

    class NamespaceSyntaxNode : SyntaxNode
    {
        public NamespaceSyntaxNode(Token namespaceToken, NameSyntaxNode name)
        {
            
        }
    }

    class QualifiedNameSyntaxNode : NameSyntaxNode
    {
        public NameSyntaxNode Qualifier { get; }

        public QualifiedNameSyntaxNode(NameSyntaxNode qualifier, NameSyntaxNode name) : base(name.IdentifierToken)
        {
            Qualifier = qualifier;
        }
    }
}
