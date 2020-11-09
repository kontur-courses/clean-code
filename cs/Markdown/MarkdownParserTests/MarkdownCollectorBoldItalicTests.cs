using System.Collections.Generic;
using FluentAssertions;
using MarkdownParser.Infrastructure;
using MarkdownParser.Infrastructure.Abstract;
using MarkdownParser.Infrastructure.Impl.Bold;
using MarkdownParser.Infrastructure.Impl.Italic;
using MarkdownParser.Infrastructure.Models;
using NUnit.Framework;

namespace MarkdownParserTests
{
    public class BoldAndItalicProvidersTests
    {
        private MarkdownCollector collector;

        [SetUp]
        public void Setup()
        {
            collector = new MarkdownCollector();
            collector.RegisterProvider(new BoldElementProvider(collector));
            collector.RegisterProvider(new ItalicElementProvider(collector));
        }

        [Test]
        public void ItalicText_ReturnElementItalic()
        {
            var tokens = TokensBuilder().Italic().Text("abc").Italic();

            collector.ParseElementsFrom(tokens)
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
            collector.ParseElementsFrom(tokens)
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
            collector.ParseElementsFrom(tokens)
                .Should()
                .BeEquivalentTo(
                    new MarkdownText(tokens[0]),
                    new MarkdownText(tokens[1]));
        }

        [Test]
        public void BoldText_ReturnElementBold()
        {
            var tokens = TokensBuilder().Bold().Text("abc").Bold();

            collector.ParseElementsFrom(tokens)
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

            collector.ParseElementsFrom(tokens)
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
            collector.ParseElementsFrom(tokens)
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

            collector.ParseElementsFrom(tokens)
                .Should()
                .HaveCount(2)
                .And
                .BeEquivalentTo(
                    new MarkdownText(tokens[0]),
                    new MarkdownText(tokens[1]));
        }

        private static TokensBuilder TokensBuilder() => new TokensBuilder();
    }
}