using NUnit.Framework;
using Markdown;
using FluentAssertions;
using MarkdownUnitTests.Extensions;
using Markdown.Models.Interfaces;
using Markdown.Models;

namespace MarkdownUnitTests
{
    [TestFixture]
    public class InlineParserTests
    {
        private IInlineParser parser;

        [SetUp]
        public void Setup()
        {
            parser = new InlineParser();
        }

        [Test]
        public void ParseToTokens_ThrowsArgumentNullException_WhenNull()
        {
            string line = null;

            var action = () => parser.ParseToTokens(line);

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ParseToTokens_ReturnsEmptyList_WhenEmptyLine()
        {
            var line = "";

            var result = parser.ParseToTokens(line);

            result.Should().BeEmpty();
        }

        [TestCase("text")]
        [TestCase("   text with spaces   ")]
        [TestCase("13012007")]
        public void ParseToTokens_ReturnsListWithOneElement_WhenValidLineWithoutTags(string line)
        {
            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenBackslashBeforeTag()
        {
            var line = "\\_text\\_";

            var result = parser.ParseToTokens(line);
            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenOnlyBackslashes()
        {
            var line = "\\\\";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [Test]
        public void ParseToTokens_ReturnsList_WhenSingleEmTokenWithText()
        {
            var line = "<em>text</em>";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            TagToken tagToken = result[0].AsTag();
            tagToken.IsTag().Should().BeTrue();
            tagToken.Tag.MarkdownSymbol.Should().Be("_");
            tagToken.Children.Should().HaveCount(1);
            tagToken.Children[0].IsTag().Should().BeFalse();
            tagToken.Children[0].AsText().Content.Should().Be("text");
        }

        [Test]
        public void ParseToTokens_ReturnsList_WhenSingleStrongTokenWithText()
        {
            var line = "<strong>text</strong>";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            var tagToken = result[0].AsTag();
            tagToken.IsTag().Should().BeTrue();
            tagToken.Tag.MarkdownSymbol.Should().Be("__");
            tagToken.Children.Should().HaveCount(1);
            tagToken.Children[0].IsTag().Should().BeFalse();
            tagToken.Children[0].AsText().Content.Should().Be("text");
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenTagWithSpaces(string tagText)
        {
            var line = $"{tagText} text {tagText}";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenUnclosedTag(string tagText)
        {
            var line = $"{tagText}text";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);

            line = $"text{tagText}";

            result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenTagsInInvalidPlace(string tagText)
        {
            var line = $"{tagText}tex{tagText}t";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeFalse();
            result[0].AsText().Content.Should().Be(line);
        }

        [Test]
        public void ParseToTokens_ReturnsListWithManyTokens_WhenMultiplyTags()
        {
            var line = "_a lot_ of __tags__";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(3);

            result[0].IsTag().Should().BeTrue();
            result[0].AsTag().Tag.MarkdownSymbol.Should().Be("_");
            result[0].AsTag().Children[0].AsText().Content.Should().Be("a lot");

            result[1].IsTag().Should().BeFalse();
            result[1].AsText().Content.Should().Be(" of ");

            result[2].IsTag().Should().BeTrue();
            result[2].AsTag().Tag.MarkdownSymbol.Should().Be("__");
            result[2].AsTag().Children[0].AsText().Content.Should().Be("tags");
        }

        [Test]
        public void ParseToTokens_ReturnsListWithManyTagTokens_WhenStrongContainsEmInside()
        {
            var line = "__strong and _em_ text__";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeTrue();
            result[0].AsTag().Tag.MarkdownSymbol.Should().Be("__");

            result[0].AsTag().Children.Should().HaveCount(3);
            result[0].AsTag().Children[0].AsText().Content.Should().Be("strong and ");

            result[0].AsTag().Children[1].IsTag().Should().BeTrue();
            result[0].AsTag().Children[1].AsTag().Tag.MarkdownSymbol.Should().Be("_");
            result[0].AsTag().Children[1].AsTag().Children[0].AsText().Content.Should().Be("em");

            result[0].AsTag().Children[2].AsText().Content.Should().Be(" text");
        }

        [Test]
        public void ParseToTokens_ReturnsListWithOneTagToken_WhenEmContainsStrongInside()
        {
            var result = parser.ParseToTokens("_text __strong__ text_");

            result.Should().HaveCount(1);
            result[0].IsTag().Should().BeTrue();
            result[0].AsTag().Tag.MarkdownSymbol.Should().Be("_");

            result[0].AsTag().Children.Should().HaveCount(1);
            result[0].AsTag().Children[0].IsTag().Should().BeFalse();
            result[0].AsTag().Children[0].AsText().Content.Should().Be("text __strong__ text");
        }
    }
}