using System;

namespace MarkdownParser.Infrastructure.Abstract
{
    public abstract class PrefixedMarkdownElementProvider<TElem> : MarkdownElementProvider<TElem>
        where TElem : MarkdownElement
    {
        public abstract Type PrefixTokenType { get; }

        protected PrefixedMarkdownElementProvider(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}