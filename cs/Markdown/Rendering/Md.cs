using MarkdownParser;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using Rendering.Html;
using Rendering.Html.Abstract;
using Rendering.Html.Impl;

namespace Rendering
{
    public static class Md
    {
        // PLace for your beautiful shiny DI container here :)
        public static HtmlMarkdownConverter Html => new HtmlMarkdownConverter(
            new MarkdownDocumentParser(
                new Tokenizer(new ITokenBuilder[]
                    {new BoldTokenBuilder(), new ItalicTokenBuilder()}),
                new MarkdownCollector(new IMarkdownElementFactory[]
                    {new BoldElementFactory(), new ItalicElementFactory()})),
            new HtmlRenderer(new IMarkdownElementRenderer[]
                {new BoldElementRenderer(), new ItalicElementRenderer()}));
    }
}