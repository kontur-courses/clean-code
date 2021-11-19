using Markdown.Tokens;

namespace Markdown.Factories
{
    public class MarkdownTokenFactory : ITokenFactory<MarkdownToken>
    {
        public MarkdownToken NewToken(string value)
        {
            return new MarkdownToken(value);
        }
    }
}