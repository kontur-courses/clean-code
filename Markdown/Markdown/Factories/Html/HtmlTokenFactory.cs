using Markdown.Tokens;

namespace Markdown.Factories.Html
{
    public class HtmlTokenFactory : ITokenFactory<HtmlToken>
    {
        public HtmlToken NewToken(string value)
        {
            return new HtmlToken(value);
        }
    }
}