using System;
using Markdown.Renderer;
using Markdown.Translator;

namespace Markdown
{
    public class Md
    {
        private readonly IMarkdownTranslator markdownTranslator;
        private readonly IRenderer renderer;

        public Md()
        {
            renderer = new HtmlRenderer();
            markdownTranslator = new MarkdownTranslator();
        }

        public string Render(string paragraph)
        {
            if (paragraph == null)
                throw new ArgumentException("Paragraph can't be null");

            var translatedParagraph = markdownTranslator.Translate(paragraph);

            return renderer.Render(translatedParagraph);
        }
    }
}