namespace Markdown
{
    public class Token
    {
        public readonly int StartPosition;
        public readonly int? EndPosition;
        public readonly string Value;
        public readonly Styles Style;
        public readonly string Separator;


        public Token(string value, Styles style, string separator, int start, int? end = null)
        {
            StartPosition = start;
            EndPosition = end;
            Value = value;
            Style = style;
            Separator = separator;
        }
    }
}