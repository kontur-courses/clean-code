using MarkdownParser.Infrastructure.Models;

namespace MarkdownParser.Infrastructure.Abstract
{
    public abstract class MarkdownElementProvider<TElem> : IMarkdownElementProvider
        where TElem : MarkdownElement
    {
        protected readonly MarkdownCollector MarkdownCollector;

        protected MarkdownElementProvider(MarkdownCollector markdownCollector)
        {
            MarkdownCollector = markdownCollector;
        }

        public bool TryParse(MarkdownElementContext context, out TElem parsed)
        {
            if (CheckPreRequisites(context)) 
                return TryParseInternal(context, out parsed);
            parsed = default;
            return false;
        }

        bool IMarkdownElementProvider.TryParse(MarkdownElementContext context, out MarkdownElement element)
        {
            var result = TryParse(context, out var parsed);
            element = parsed;
            return result;
        }

        protected virtual bool CheckPreRequisites(MarkdownElementContext context) => true;

        protected abstract bool TryParseInternal(MarkdownElementContext context, out TElem parsed);
    }
}