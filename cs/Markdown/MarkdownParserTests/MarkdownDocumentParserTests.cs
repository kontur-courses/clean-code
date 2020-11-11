using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using MarkdownParser;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Tokenization;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using NUnit.Framework;

namespace MarkdownParserTests
{
    public class MarkdownDocumentParserTests
    {
        private MarkdownDocumentParser parser;

        [SetUp]
        public void SetUp()
        {
            var tokenizer = new Tokenizer(new ITokenBuilder[]
            {
                new BoldTokenBuilder(),
                new ItalicTokenBuilder(),
            });
            var collector = new MarkdownCollector(new IMarkdownElementFactory[]
            {
                new BoldElementFactory(),
                new ItalicElementFactory(),
            });
            parser = new MarkdownDocumentParser(tokenizer, collector);
        }

        [Test]
        public void BoldText_BoldElementWithContentInside()
        {
            var tokens = new TokensCollectionBuilder().Bold().Text("abc def").Bold();
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .BeEquivalentTo(new MarkdownElementBold(tokens: tokens, content:
                    new List<MarkdownElement> {new MarkdownText(tokens[1])}));
        }

        [Test]
        public void ItalicText_ItalicWithTokensInside()
        {
            var tokens = new TokensCollectionBuilder().Italic().Text("abc def").Italic();
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .BeEquivalentTo(new MarkdownElementItalic(tokens));
        }

        [Test]
        public void BoldTextWithItalicInside_BoldElementWithItalicElement()
        {
            var tokens = new TokensCollectionBuilder().Bold().Text("a ").Italic().Text("b").Italic().Text(" c").Bold();
            TestContext.Progress.WriteLine(tokens.ToString());
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .BeEquivalentTo(new MarkdownElementBold(tokens: tokens, content:
                    new List<MarkdownElement>
                    {
                        new MarkdownText(tokens[1]),
                        new MarkdownElementItalic(tokens.Range(2, 3)),
                        new MarkdownText(tokens[5]),
                    }));
        }

        [Test]
        public void ItalicTextWithBoldInside_ItalicWithBoldAsText()
        {
            var tokens = new TokensCollectionBuilder().Italic().Text("a ").Bold().Text("b").Bold().Text(" c").Italic();
            TestContext.Progress.WriteLine(tokens.ToString());
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .BeEquivalentTo(new MarkdownElementItalic(tokens));
        }

        [Test]
        public void EmptyBold_AsText()
        {
            var tokens = new TokensCollectionBuilder().Bold().Bold();
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .BeEquivalentTo(new MarkdownText(tokens[0]), new MarkdownText(tokens[1]));
        }

        [Test]
        public void EmptyItalic_AsText()
        {
            var tokens = new TokensCollectionBuilder().Italic().Italic();
            parser.Parse(tokens.ToString()).Elements
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(
                    new MarkdownText(new BoldToken(0, "__")),
                    because: "__ это тег жирного текста, это нормальное поведение");
        }
    }
}