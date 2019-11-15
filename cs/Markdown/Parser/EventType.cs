namespace Markdown.Parser
{
    public enum EventType
    {
        PlainText = 0,

        BoldStart,
        BoldEnd,

        ItalicStart,
        ItalicEnd
    }
}