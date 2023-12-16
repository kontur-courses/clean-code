using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class MarkdownParser : IMarkingParser
    {
        private readonly Dictionary<string, Tag> supportedTags;
        private readonly HashSet<char> symbolsToShield;

        public MarkdownParser(HashSet<char> symbolsToShield)
        {
            supportedTags = TagCreator.GetAllSupportedTags();
            this.symbolsToShield = symbolsToShield;
        }

        public IEnumerable<IToken> ParseText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
