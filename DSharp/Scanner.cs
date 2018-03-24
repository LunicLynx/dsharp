using System;
using System.Collections.Generic;

namespace DSharp
{
    public class Scanner
    {
        private readonly string _input;
        private int _index = 0;
        private int _currentTokenStart = 0;

        private char _c;



        public Scanner(string input)
        {
            _input = input;
        }

        public IEnumerable<Token> Scan()
        {
            while (NextChar())
            {
                if (IsIdentifierStart(_c))
                {
                    yield return ScanIdentifierOrKeyword();
                }
                else if (IsNumberStart(_c))
                {
                    yield return ScanNumber();
                }
                else if (IsStringStart(_c))
                {
                    yield return ScanString();
                }
                else if (IsControlChar(_c))
                {
                    yield return ScanControlChar();
                }
                else if (IsWhiteSpace(_c))
                {
                    DiscardWhiteSpace();
                }
                else
                {
                    throw new InvalidCharacterException(_c);
                }
                _currentTokenStart = _index;
            }

            yield return new Token(_currentTokenStart, _index, TokenType.EndOfFile, string.Empty);
        }

        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }

        private void DiscardWhiteSpace()
        {
            ReadToken(IsWhiteSpace, _ => TokenType.None);
        }

        private Token ScanControlChar()
        {
            var tokenType = ControlChars[_c];
            return new Token(_currentTokenStart, _index, tokenType, new string(_c, 1));
        }

        private static readonly IDictionary<char, TokenType> ControlChars = new Dictionary<char, TokenType>
        {
            {'{', TokenType.LeftBrace},
            {'}', TokenType.RightBrace},
            {'(', TokenType.LeftParanthese},
            {')', TokenType.RightParanthese},
            {'[', TokenType.LeftBracket},
            {']', TokenType.RightBracket},
            {';', TokenType.SemiColon},
            {'.', TokenType.Dot}
        };

        private static readonly IDictionary<string, TokenType> Keywords = new Dictionary<string, TokenType> { };

        private static bool IsControlChar(char c)
        {
            return ControlChars.ContainsKey(c);
        }

        private Token ScanString()
        {
            while (NextChar())
            {
                if (_index >= _input.Length) throw new UnexpectedEndOfFileException();
                if (IsTabOrLineBreak(_c)) throw new InvalidCharacterException(_c);
                if (IsStringEnd(_c)) break;
            }

            var content = _input.Substring(_currentTokenStart, _index - _currentTokenStart);
            return new Token(_currentTokenStart, _index, TokenType.StringLiteral, content);
        }

        private static bool IsStringStart(char c)
        {
            return c == '"';
        }

        private static bool IsStringEnd(char c)
        {
            return c == '"';
        }

        private static bool IsTabOrLineBreak(char c)
        {
            return c == '\t' || c == '\n' || c == '\r';
        }

        private Token ScanNumber()
        {
            return ReadToken(IsNumberFollow, _ => TokenType.NumberLiteral);
        }

        private static bool IsIdentifierStart(char c)
        {
            return IsLetter(c) || c == '_';
        }

        private static bool IsLetter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        private static bool IsIdentifierFollow(char c)
        {
            return IsIdentifierStart(c) || IsDigit(c);
        }

        private static bool IsDigit(char c)
        {
            return c <= '0' && c >= '9';
        }

        private static bool IsNumberStart(char c)
        {
            return IsDigit(c);
        }

        private static bool IsNumberFollow(char c)
        {
            return IsDigit(c);
        }

        private Token ScanIdentifierOrKeyword()
        {
            return ReadToken(IsIdentifierFollow, GetIdentifierOrKeywordTokenType);
        }

        private TokenType GetIdentifierOrKeywordTokenType(string content)
        {
            switch (content)
            {
                case "using": return TokenType.UsingKeyword;
                case "namespace": return TokenType.NamespaceKeyword;
                case "class": return TokenType.ClassKeyword;
                case "static": return TokenType.StaticKeyword;
                case "void": return TokenType.VoidKeyword;
                case "string": return TokenType.StringKeyword;
            }

            return TokenType.Identifier;
        }

        private Token ReadToken(Func<char, bool> predicate, Func<string, TokenType> getTokenType)
        {
            while (_index < _input.Length && predicate(_input[_index]))
            {
                if (!NextChar())
                    break;
            }

            var tokenContent = _input.Substring(_currentTokenStart, _index - _currentTokenStart);
            var tokenType = getTokenType(tokenContent);
            return new Token(_currentTokenStart, _index, tokenType, tokenContent);
        }

        private bool NextChar()
        {
            if (_index >= _input.Length) return false;
            _c = _input[_index++];
            return _index <= _input.Length;
        }

    }
}