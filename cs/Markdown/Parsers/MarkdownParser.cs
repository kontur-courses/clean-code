using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class MarkdownParser : IParser
    {
        private readonly IDictionary<string, TagType> tags;

        public MarkdownParser(IDictionary<string, TagType> tags) 
        {
            this.tags = tags;
        }

        public IEnumerable<IToken> ParseText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
