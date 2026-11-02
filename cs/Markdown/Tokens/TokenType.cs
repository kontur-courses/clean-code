namespace Markdown.Tokens;

public enum TokenType
{
    Text,
    Underscore, // _
    DoubleUnderscore, // __
    LinkTextOpen, // [
    LinkTextClose, // ]
    LinkUrl // (url)
}