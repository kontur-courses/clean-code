namespace Markdown.Inline
{
    public class ItalicTag : InlineTag
    {
        public string Text { get; }
        public ItalicTag(string text) => Text = text;
    }
}
