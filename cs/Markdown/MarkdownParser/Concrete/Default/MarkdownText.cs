using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Default
{
    public sealed class MarkdownText : MarkdownElement
    {
        public MarkdownText(Token text) : base(new[] {text})
        {
        }
    }
}