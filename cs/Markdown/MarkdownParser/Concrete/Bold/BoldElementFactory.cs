using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Concrete.Bold
{
    public sealed class BoldElementFactory : MarkdownElementFactory<MarkdownElementBold>, IMarkdownCollectorDependent
    {
        private MarkdownCollector markdownCollector;

        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementBold parsed)
        {
            if (!markdownCollector.TryCollectUntil(context, token => token.GetType() == typeof(BoldToken),
                out var matchedTokenIndex,
                out var innerTokens) || innerTokens.Count == 0)
            {
                parsed = default;
                return false;
            }

            var tokens = innerTokens.ToArray();
            var innerElements = markdownCollector.CreateElementsFrom(tokens);

            var matchedToken = context.Tokens[matchedTokenIndex];
            var elementTokens = tokens.Prepend(context.CurrentToken).Append(matchedToken).ToArray();
            parsed = new MarkdownElementBold(innerElements.ToArray(), elementTokens);
            return true;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken.GetType() == typeof(BoldToken);

        public void SetCollector(MarkdownCollector collector) =>
            markdownCollector = collector;
    }
}