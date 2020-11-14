using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using Rendering.Html.Abstract;

namespace Rendering.Html
{
    public class HtmlRenderer
    {
        private readonly IDictionary<Type, IMarkdownElementRenderer> renderers;

        public HtmlRenderer(IEnumerable<IMarkdownElementRenderer> renderers)
        {
            this.renderers = renderers.ToDictionary(r => r.ValidElementType);
            foreach (var dependent in this.renderers.Values.OfType<IHtmlRendererDependent>())
                dependent.SetRenderer(this);
        }

        public string Render(MarkdownElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            if (element is MarkdownText textElem)
                return RenderText(textElem);

            if (!renderers.TryGetValue(element.GetType(), out var renderer))
                throw new ArgumentOutOfRangeException($"Can't get renderer for element {element.GetType()}");

            return renderer.RenderElement(element);
        }

        public string Render(MarkdownDocumentLine line)
        {
            var lineBuilder = new StringBuilder();
            foreach (var element in line.Elements)
            {
                var rendered = Render(element);
                lineBuilder.Append(rendered);
            }

            return lineBuilder.ToString();
        }

        public string Render(MarkdownDocument document) =>
            string.Join(Environment.NewLine, document.Lines.Select(Render));

        public string RenderText(MarkdownText textElem) =>
            string.Join(string.Empty, textElem.Tokens.Select(t => t.RawValue));
    }
}