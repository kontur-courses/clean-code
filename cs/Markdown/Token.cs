namespace Markdown
{
    public class Token
    {
        public readonly string value, openTag, closeTag;
        public readonly int priority, shift;

        public Token(string value, string openTag, string closeTag, int priority, int shift)
        {
            this.value = value;
            this.openTag = openTag;
            this.closeTag = closeTag;

            this.priority = priority;
            this.shift = shift;
        }
    }
}