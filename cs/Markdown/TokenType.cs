namespace Markdown;

public enum TokenType
{
    Heading,
    BoldStart,
    BoldEnd,
    ItalicStart,
    ItalicEnd,
    Text,
    NewLine,
    ListStart,
    ListItem,
    ListEnd,
    EndOfFile
}
