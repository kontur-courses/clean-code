using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Models
{
    public sealed class MarkdownText : MarkdownElement
    {
        public string Text { get; }

        public MarkdownText(string text,int lastTokenIndex) : base(lastTokenIndex)
        {
            Text = text;
        }
    }
}