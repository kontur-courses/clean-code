namespace Markdown
{
    public class Token
    {
        public readonly string Value, OpenTag, CloseTag;
        public readonly int Priority, Shift;

        public Token(string value, string openTag, string closeTag, int priority, int shift)
        {
            Value = value;
            OpenTag = openTag;
            CloseTag = closeTag;

            Priority = priority;
            Shift = shift;
        }
    }
}