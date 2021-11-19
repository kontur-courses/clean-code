using Markdown.Tokens;

namespace Markdown.Factories
{
    public class HtmlTokenFactory : ITokenFactory<HtmlToken>
    {
        public HtmlToken NewToken(string value)
        {
            return new HtmlToken(value);
        }
    }
}