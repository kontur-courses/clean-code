using System;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure.Impl.Bold
{
    public class BoldElementProvider : FramedMarkdownElementProvider<MarkdownElementBold>
    {
        protected override bool TryParseInternal(MarkdownElementContext context, out MarkdownElementBold parsed)
        {
            var result = MarkdownCollector.TryCollectUntil(context, token => token.GetType() == PostfixTokenType,
                out var matchedTokenIndex,
                out var innerTokens);

            if (!result)
            {
                parsed = default;
                return false;
            }

            var tokens = innerTokens.ToArray();
            var innerElements = MarkdownCollector.ParseElementsFrom(tokens);

            var matchedToken = context.Tokens[matchedTokenIndex];
            var elementTokens = tokens.Prepend(context.CurrentToken).Append(matchedToken).ToArray();
            parsed = new MarkdownElementBold(innerElements.ToArray(), elementTokens);
            return true;
        }

        public override Type PrefixTokenType { get; } = typeof(TokenBold);
        public override Type PostfixTokenType { get; } = typeof(TokenBold);

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken.GetType() == PrefixTokenType;

        public BoldElementProvider(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}