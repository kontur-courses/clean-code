using System;
using MarkdownParser;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Header;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Concrete.Link;
using MarkdownParser.Concrete.Special;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Workers;
using Rendering.Html;
using Rendering.Html.Abstract;
using Rendering.Html.Impl;

namespace Rendering
{
    public static class Md
    {
        // PLace for your beautiful shiny DI container here :)
        private static readonly Lazy<HtmlMarkdownConverter> htmlConverter = new Lazy<HtmlMarkdownConverter>(() =>
            new HtmlMarkdownConverter(
                new MarkdownDocumentParser(
                    new Tokenizer(new ITokenBuilder[]
                    {
                        new BoldTokenBuilder(),
                        new ItalicTokenBuilder(),
                        new HeaderTokenBuilder(),
                        new SpecialTokenBuilder(SpecialTokenType.OpeningRoundBracket),
                        new SpecialTokenBuilder(SpecialTokenType.ClosingRoundBracket),
                        new SpecialTokenBuilder(SpecialTokenType.OpeningSquareBracket),
                        new SpecialTokenBuilder(SpecialTokenType.ClosingSquareBracket),
                    }),
                    new MarkdownCollector(new IMdElementFactory[]
                    {
                        new BoldElementFactory(),
                        new ItalicElementFactory(),
                        new HeaderElementFactory(),
                        new LinkElementFactory(),
                    })),
                new HtmlRenderer(new IMarkdownElementRenderer[]
                {
                    new BoldElementRenderer(),
                    new ItalicElementRenderer(),
                    new HeaderElementRenderer(),
                    new LinkElementRenderer(),
                })));

        public static IMarkdownConverter ToHtml => htmlConverter.Value;
    }
}