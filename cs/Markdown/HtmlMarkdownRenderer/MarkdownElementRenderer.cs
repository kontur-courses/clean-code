using System;
using MarkdownParser.Infrastructure.Abstract;

namespace HtmlMarkdownRenderer
{
    public abstract class MarkdownElementRenderer<TElem> : IMarkdownElementRenderer where TElem : MarkdownElement
    {
        public abstract string RenderElement(TElem element);
        public Type ValidElementType { get; } = typeof(TElem);

        string IMarkdownElementRenderer.RenderElement(MarkdownElement element) => element switch
        {
            null => throw new ArgumentNullException(nameof(element)),
            TElem elem => RenderElement(elem),
            _ => throw new InvalidOperationException($"Can't render element of type {element.GetType()}")
        };
    }
}