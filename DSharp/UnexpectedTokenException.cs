using System;
using DSharp.Syntax;

namespace DSharp
{
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
}