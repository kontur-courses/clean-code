using System.Linq;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Concrete.Italic
{
    public sealed class ItalicElementFactory : MarkdownElementFactory<MarkdownElementItalic>,
        IMarkdownCollectorDependent
    {
        private MarkdownCollector markdownCollector;

        protected override bool TryCreateFromValidContext(MarkdownElementContext context,
            out MarkdownElementItalic parsed)
        {
            if (markdownCollector.TryCollectUntil(context, token => token is ItalicToken,
                out var matchedTokenIndex,
                out var collected))
            {
                parsed = new MarkdownElementItalic((ItalicToken) context.CurrentToken,
                    collected.ToArray(),
                    (ItalicToken) context.Tokens[matchedTokenIndex]);
                return true;
            }

            parsed = default;
            return false;
        }

        protected override bool CheckPreRequisites(MarkdownElementContext context) =>
            context.CurrentToken is ItalicToken;

        public void SetCollector(MarkdownCollector collector) =>
            markdownCollector = collector;
    }
}