using Markdown.Tags;

namespace Markdown.Parsers
{
    public class StartLinkParser : IParser
    {
        public IToken TryGetToken()
        {
            return new TagLink(null);
        }
    }
}
