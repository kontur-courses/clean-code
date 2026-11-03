namespace Markdown.Inline
{
    public class StrongTag : InlineTag
    {
        public string Text { get; }
        public StrongTag(string text) => Text = text;
    }
}
