using System;
using System.Collections.Generic;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure.Impl.Bold
{
    public class BoldElementProvider : FramedMarkdownElementProvider<MarkdownElementBold>
    {
        protected override bool TryParseInternal(MarkdownElementContext context, out MarkdownElementBold parsed)
        {
            var result = MarkdownCollector.TryParseUntil(context, token => token is TokenBold, out var innerParsed,
                out var lastVisitedTokenIndex);

            parsed = result
                ? new MarkdownElementBold(lastVisitedTokenIndex, innerParsed)
                : default;

            return result;
        }

        public override Type PrefixTokenType { get; } = typeof(TokenBold);
        public override Type PostfixTokenType { get; } = typeof(TokenBold);

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is TokenBold;

        public BoldElementProvider(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}