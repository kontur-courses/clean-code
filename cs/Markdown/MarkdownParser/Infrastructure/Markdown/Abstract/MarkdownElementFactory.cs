using MarkdownParser.Infrastructure.Markdown.Models;

namespace MarkdownParser.Infrastructure.Markdown.Abstract
{
    public abstract class MarkdownElementFactory<TElem> : IMarkdownElementFactory
        where TElem : MarkdownElement
    {
        public bool TryCreate(MarkdownElementContext context, out TElem parsed)
        {
            if (CheckPreRequisites(context))
                return TryCreateFromValidContext(context, out parsed);
            parsed = default;
            return false;
        }

        bool IMarkdownElementFactory.TryCreate(MarkdownElementContext context, out MarkdownElement element)
        {
            var result = TryCreate(context, out var parsed);
            element = parsed;
            return result;
        }

        protected virtual bool CheckPreRequisites(MarkdownElementContext context) => true;

        protected abstract bool TryCreateFromValidContext(MarkdownElementContext context, out TElem parsed);
    }
}