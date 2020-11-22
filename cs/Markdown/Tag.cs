namespace Markdown
{
    public class Tag
    {
        public readonly string Markdown;
        public readonly int Position;
        public readonly string Text;
        public readonly string Value;

        public Tag(string value, string text, int position, string markdown = null)
        {
            Position = position;
            Text = text;
            Value = value;
            Markdown = markdown;
        }
    }
}