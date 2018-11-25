namespace Markdown
{
    public class Token
    {
        public readonly string Value, OpenTag, CloseTag;
        public readonly int Priority, OriginalTextLen;

        public Token(string value, string openTag, string closeTag, int priority, int originalTextLen)
        {
            Value = value;
            OpenTag = openTag;
            CloseTag = closeTag;

            Priority = priority;
            OriginalTextLen = originalTextLen;
        }
    }
}