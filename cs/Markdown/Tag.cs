namespace Markdown
{
    public class Tag
    {
        public readonly string Markdown;
        public readonly int Position;
        public readonly string Value;

        public Tag(string value, int position, string markdown = null)
        {
            Position = position;
            Value = value;
            Markdown = markdown;
        }
    }
}