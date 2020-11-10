using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Markdown.Abstract;

namespace HtmlMarkdownRenderer
{
    public class MarkdownHtmlRenderer
    {
        private readonly IDictionary<Type, IMarkdownElementRenderer> renderers;

        public MarkdownHtmlRenderer(IEnumerable<IMarkdownElementRenderer> renderers)
        {
            this.renderers = renderers.ToDictionary(r => r.ValidElementType);
        }

        public string Render(MarkdownElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            if (!renderers.TryGetValue(element.GetType(), out var renderer))
                throw new ArgumentOutOfRangeException($"Can't get renderer for element {element.GetType()}");

            return renderer.RenderElement(element);
        }
    }
}