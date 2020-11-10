using System.Linq;
using MarkdownParser.Infrastructure;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Concrete.Italic
{
    public class ItalicElementFactory : MarkdownElementFactory<MarkdownElementItalic>
    {
        protected override bool TryCreateFromValidContext(MarkdownElementContext context, out MarkdownElementItalic parsed)
        {
            if (MarkdownCollector.TryCollectUntil(context, token => token is ItalicToken,
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

        public ItalicElementFactory(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}