namespace Markdown.TokenizerClasses
{
    public enum TokenType
    {
        Text,
        Digit,
        Underscore,
        EscapeChar,
        Space,
        CarriageReturn,
        Newline,
        EOF,
        Null
    }
}