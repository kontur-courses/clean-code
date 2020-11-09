using MarkdownParser.Infrastructure.Abstract;

namespace MarkdownParser.Infrastructure.Models
{
    public sealed class MarkdownText : MarkdownElement
    {
        public MarkdownText(Token text) : base(new[] {text})
        {
        }
    }
}