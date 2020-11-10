using System.Linq;
using MarkdownParser.Infrastructure;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Concrete.Bold
{
    public class BoldElementFactory : MarkdownElementFactory<MarkdownElementBold>
    {
        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementBold parsed)
        {
            if (!MarkdownCollector.TryCollectUntil(context, token => token.GetType() == typeof(BoldToken),
                out var matchedTokenIndex,
                out var innerTokens))
            {
                parsed = default;
                return false;
            }

            var tokens = innerTokens.ToArray();
            var innerElements = MarkdownCollector.CreateElementsFrom(tokens);

            var matchedToken = context.Tokens[matchedTokenIndex];
            var elementTokens = tokens.Prepend(context.CurrentToken).Append(matchedToken).ToArray();
            parsed = new MarkdownElementBold(innerElements.ToArray(), elementTokens);
            return true;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken.GetType() == typeof(BoldToken);

        public BoldElementFactory(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}