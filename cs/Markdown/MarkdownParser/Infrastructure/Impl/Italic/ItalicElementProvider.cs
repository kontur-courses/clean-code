using System;
using System.Linq;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure.Impl.Italic
{
    public class ItalicElementProvider : FramedMarkdownElementProvider<MarkdownElementItalic>
    {
        protected override bool TryParseInternal(MarkdownElementContext context, out MarkdownElementItalic parsed)
        {
            if (MarkdownCollector.TryCollectUntil(context, token => token is TokenItalic,
                out var matchedTokenIndex,
                out var collected))
            {
                parsed = new MarkdownElementItalic((TokenItalic) context.CurrentToken, 
                    collected.ToArray(),
                    (TokenItalic) context.Tokens[matchedTokenIndex]);
                return true;
            }

            parsed = default;
            return false;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is TokenItalic;

        public override Type PrefixTokenType { get; } = typeof(TokenItalic);
        public override Type PostfixTokenType { get; } = typeof(TokenItalic);

        public ItalicElementProvider(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}