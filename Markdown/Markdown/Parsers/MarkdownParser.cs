using System.Linq;
using Markdown.Factories;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class MarkdownParser : IParser<MarkdownMarking>
    {
        private readonly ITokenFactory<MarkdownToken> tokenFactory;
        private readonly IMarkingFactory<MarkdownMarking> markingFactory;

        public MarkdownParser(ITokenFactory<MarkdownToken> tokenFactory,
            IMarkingFactory<MarkdownMarking> markingFactory)
        {
            this.tokenFactory = tokenFactory;
            this.markingFactory = markingFactory;
        }

        public MarkdownMarking Parse(string markdown)
        {
            var lines = markdown
                .Split('\n')
                .Select(line => line.Split(' '));

            var tokensLines = lines
                .Select(line => line
                    .Select(word => tokenFactory.NewToken(word))
                    .ToList())
                .ToList();

            return markingFactory.NewMarking(tokensLines);
        }
    }
}