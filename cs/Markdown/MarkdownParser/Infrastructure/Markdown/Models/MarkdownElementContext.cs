using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Markdown.Models
{
    public class MarkdownElementContext
    {
        public Token[] NextTokens { get; set; }
        public Token CurrentToken { get; set; }
    }
}