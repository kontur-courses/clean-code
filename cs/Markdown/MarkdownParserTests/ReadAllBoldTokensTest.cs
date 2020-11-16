using FluentAssertions;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown.MarkdownParserTests
{
    public class ReadAllBoldTokensTest
    {
        [Test]
        public void ReadAllBoldTokens_ShouldReturnEmptyArray_OnSimpleString()
        {
            MarkdownParser.ReadAllBoldTokens("asdf").Should().BeEmpty();
        }

        [Test]
        public void ReadAllBoldTokens_ShouldReturnTwoTags_OnStringWithOneToken()
        {
            var result = MarkdownParser.ReadAllBoldTokens("__asdf__");
            result.Length.Should().Be(2);
            result[0].Should().BeEquivalentTo(new Tag("<strong>", 0, 2));
            result[1].Should().BeEquivalentTo(new Tag("</strong>", 6, 2));
        }

        [Test]
        public void ReadAllBoldTolens_ShouldReturnEmptyArray_OnShieldedOpenToken()
        {
            var result = MarkdownParser.ReadAllBoldTokens(@"\__asdf__");
            result.Length.Should().Be(0);
        }
        
        [Test]
        public void ReadAllBoldTolens_ShouldReturnEmptyArray_OnShieldedCloseToken()
        {
            var result = MarkdownParser.ReadAllBoldTokens(@"__asdf\__");
            result.Length.Should().Be(0);
        }
    }
}