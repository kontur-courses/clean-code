namespace Markdown
{
    public class TagAttribute
    {
        public TagAttributeType Type { get; }
        public string Text { get; }

        public TagAttribute(TagAttributeType type, string text)
        {
            Type = type;
            Text = text.Escape();
        }
    }
}