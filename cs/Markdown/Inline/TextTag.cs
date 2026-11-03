namespace Markdown.Inline
{
    public class TextTag : InlineTag
    {
        public string Text { get; }
        public TextTag(string text) => Text = text;
    }
}
