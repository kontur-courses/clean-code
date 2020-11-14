using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkdownParser.Concrete.Bold;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Concrete.Italic;
using MarkdownParser.Infrastructure.Markdown;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;
using NUnit.Framework;

namespace MarkdownParserTests.Collector
{
    public class BoldAndItalicProvidersTests
    {
        private MarkdownCollector collector;

        [SetUp]
        public void Setup()
        {
            collector = new MarkdownCollector(new IMarkdownElementFactory[]
            {
                new BoldElementFactory(),
                new ItalicElementFactory()
            });
        }

        [Test]
        public void ItalicText_ReturnElementItalic()
        {
            var tokens = TokensBuilder().Italic().Text("abc").Italic();

            collector.CreateElementsFrom(tokens)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<MarkdownElementItalic>()
                .And
                .BeEquivalentTo(new MarkdownElementItalic(tokens));
        }

        [Test]
        public void ItalicWithBoldInside_TreatBoldAsText()
        {
            var tokens = TokensBuilder().Italic().Bold().Text("abc").Bold().Italic();
            collector.CreateElementsFrom(tokens)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<MarkdownElementItalic>()
                .And
                .BeEquivalentTo(new MarkdownElementItalic(tokens));
        }

        [Test]
        public void ItalicNotClosed_TreatAsText()
        {
            var tokens = TokensBuilder().Italic().Text("abc");
            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(
                    new MarkdownText(tokens[0]),
                    new MarkdownText(tokens[1]));
        }

        [Test]
        public void BoldText_ReturnElementBold()
        {
            var tokens = TokensBuilder().Bold().Text("abc").Bold();

            collector.CreateElementsFrom(tokens)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<MarkdownElementBold>()
                .And
                .BeEquivalentTo(new MarkdownElementBold(new List<MarkdownElement>
                    {new MarkdownText(tokens[1])}, tokens));
        }

        [Test]
        public void BoldWithItalicInside_TreatItalicAsInnerElement()
        {
            var tokens = TokensBuilder().Bold().Italic().Text("abc").Italic().Bold();

            collector.CreateElementsFrom(tokens)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<MarkdownElementBold>()
                .And
                .BeEquivalentTo(
                    new MarkdownElementBold(
                        new List<MarkdownElement>
                        {
                            new MarkdownElementItalic(tokens[1], tokens.Range(2, 1), tokens[-2])
                        },
                        tokens));
        }

        [Test]
        public void BoldWithNotClosedItalicInside_TreatItalicAsText()
        {
            var tokens = TokensBuilder().Bold().Italic().Text("abc").Bold();
            collector.CreateElementsFrom(tokens)
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeOfType<MarkdownElementBold>()
                .And
                .BeEquivalentTo(
                    new MarkdownElementBold(
                        new List<MarkdownElement>
                        {
                            new MarkdownText(tokens[1]),
                            new MarkdownText(tokens[2]),
                        },
                        tokens));
        }

        [Test]
        public void BoldNotClosed_TreatAsText()
        {
            var tokens = TokensBuilder().Bold().Text("abc");

            collector.CreateElementsFrom(tokens)
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(
                    new MarkdownText(tokens[0]),
                    new MarkdownText(tokens[1]));
        }

        [Test]
        public void BoldIntersectsWithItalic_TreatBothAsText()
        {
            var tokens = TokensBuilder()
                .Bold().Text("abc ")
                .Italic().Text("def")
                .Bold().Text(" ghi")
                .Italic().Text(" jkl");

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void BoldInDifferentWords_TreatAsText()
        {
            var tokens = TokensBuilder()
                .Text("a").Bold().Text("b c").Bold().Text("d");

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void OpeningBoldBeforeWhitespace_Ignored()
        {
            var tokens = TokensBuilder()
                .Text("a").Bold().Text(" bc").Bold();

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void ClosingBoldAfterWhitespace_Ignored()
        {
            var tokens = TokensBuilder()
                .Text("a").Bold().Text("bc ").Bold();

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void ItalicInDifferentWords_TreatAsText()
        {
            var tokens = TokensBuilder()
                .Text("a").Italic().Text("b c").Italic().Text("d");

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void OpeningItalicBeforeWhitespace_Ignored()
        {
            var tokens = TokensBuilder()
                .Text("a").Italic().Text(" bc").Italic();

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        [Test]
        public void ClosingItalicAfterWhitespace_Ignored()
        {
            var tokens = TokensBuilder()
                .Text("a").Italic().Text("bc ").Italic();

            collector.CreateElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(tokens.ToArray().Select(t => new MarkdownText(t)));
        }

        private static TokensCollectionBuilder TokensBuilder() => new TokensCollectionBuilder();
    }
}