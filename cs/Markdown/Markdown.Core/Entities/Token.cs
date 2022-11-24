namespace Markdown.Core.Entities
{
    public class Token
    {
        public readonly string Value, OpenTag, CloseTag;
        public readonly int Priority, Shift;
        public readonly bool IsParseInside;

        public Token(string value, string openTag, string closeTag, int priority, int shift, bool isParseInside)
        {
            Value = value;
            OpenTag = openTag;
            CloseTag = closeTag;

            Priority = priority;
            Shift = shift;
            IsParseInside = isParseInside;
        }
    }
}
