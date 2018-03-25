namespace DSharp.Syntax
{
    public enum TokenType
    {
        None,
        Identifier,
        NumberLiteral,
        StringLiteral,
        SemiColon,
        EndOfFile,
        LeftBrace,
        RightBrace,
        LeftParanthese,
        RightParanthese,
        Dot,
        LeftBracket,
        RightBracket,
        UsingKeyword,
        NamespaceKeyword,
        ClassKeyword,
        StaticKeyword,
        VoidKeyword,
        StringKeyword
    }
}