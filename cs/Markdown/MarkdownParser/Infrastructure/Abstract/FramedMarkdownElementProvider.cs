using System;

namespace MarkdownParser.Infrastructure.Abstract
{
    public abstract class FramedMarkdownElementProvider<TElem> : PrefixedMarkdownElementProvider<TElem>
        where TElem : MarkdownElement
    {
        public abstract Type PostfixTokenType { get; }

        protected FramedMarkdownElementProvider(MarkdownCollector markdownCollector) : base(markdownCollector)
        {
        }
    }
}