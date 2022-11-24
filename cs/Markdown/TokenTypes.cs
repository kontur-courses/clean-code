namespace Markdown
{
    internal enum TokenType
    {
        Text,
        Space,
        Newline,
        Sharp,
        Object
    }

    internal enum TokenObjectType
    {
        Italic,
        Strong,
        Header
    }
}