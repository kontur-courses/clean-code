using Markdown.Tokens;

namespace Markdown.Factories.Markdown
{
    public class MarkdownTokenFactory : ITokenFactory<MarkdownToken>
    {
        public MarkdownToken NewToken(string value)
        {
            return new MarkdownToken(value);
        }
    }
}