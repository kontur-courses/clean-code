namespace Markdown.Parser
{
    public enum TokenType
    {
        PlainText = 0,

        BoldStart,
        BoldEnd,

        ItalicStart,
        ItalicEnd,

        Empty
    }
}