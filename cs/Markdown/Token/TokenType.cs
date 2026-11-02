namespace Markdown.Tokens;

public enum TokenType
{
    Text = 0,
    ItalicOpen = 1,
    ItalicClose = 2,
    BoldOpen = 3,
    BoldClose = 4,
    HeaderOpen = 5,
    HeaderClose = 6,
    LinkOpen = 7,
    LinkClose = 8,
    UrlOpen = 9,
    UrlClose = 10,
    Newline = 11
}
