namespace Markdown.Inline
{
    public class TextTag : IInlineTag
    {
        public string Text { get; }
        public TextTag(string text) => Text = text;
    }
}
