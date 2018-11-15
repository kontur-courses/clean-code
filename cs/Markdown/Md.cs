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
            this.renderer = new HtmlRenderer();
            this.markdownTranslator = new MarkdownTranslator();
        }

        public Md(IRenderer renderer,IMarkdownTranslator markdownTranslator)
        {
            this.renderer = renderer;
            this.markdownTranslator = markdownTranslator;
        }

        public string Render(string paragraph)
        {
            if(paragraph == null)
                throw new ArgumentException("Paragraph can't be null");

            var translatedParagraph = markdownTranslator.Translate(paragraph);

            return renderer.Render(translatedParagraph);
        }
    }
}