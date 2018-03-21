using System;

namespace DSharp
{
    public class InvalidCharacterException : Exception
    {
        public char C { get; }

        public InvalidCharacterException(char c)
        {
            C = c;
        }
    }
}