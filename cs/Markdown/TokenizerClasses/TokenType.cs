namespace Markdown.TokenizerClasses
{
    public enum TokenType
    {
        Text,
        Num,
        Underscore,
        DoubleUnderscore,
        EscapeChar,
        Space,
        CarriageReturn,
        Newline,
        EOF,
        Null
    }
}