using NUnit.Framework;
using Markdown;
using FluentAssertions;
using MarkdownTests.Extensions;
namespace MarkdownTests.Unit
{
    public class InlineParserUnitTests
    {
        private IInlineParser parser;
        [SetUp]
        public void Setup()
        {
            var registry = new TagRegistry();
            parser = new InlineParser(registry);
        }

        [TestCase(null)]
        public void ParseToTokens_ThrowArgumentNullException_WhenNull(string line)
        {
            var action = () => parser.ParseToTokens(line);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestCase("")]
        public void ParseToTokens_ReturnsEmptyList_WhenEmptyLine(string line)
        {
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
            result[0].Content.Should().Be(line);
        }

        [Test]
        public void ParseToTokens_ReturnsList_WhenSingleEmTokenWithText()
        {
            var line = "<em>text</em>";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            var tagToken = result[0];
            tagToken.IsTag().Should().BeTrue();
            tagToken.Tag.MarkdownSymbol.Should().Be("_");
            tagToken.Children.Should().HaveCount(1);
            tagToken.Children[0].IsTag().Should().BeFalse();
            tagToken.Children[0].Content.Should().Be("text");
        }

        
    }
}