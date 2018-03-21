namespace DSharp
{
    public class Token
    {
        public int Start { get; }

        public int End { get; }

        public int Length => End - Start;

        public TokenType TokenType { get; }

        public string Content { get; }

        public Token(int start, int end, TokenType tokenType, string content)
        {
            Start = start;
            End = end;
            TokenType = tokenType;
            Content = content;
        }
    }
}