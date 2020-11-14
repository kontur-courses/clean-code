using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
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
            result[0].Should().BeEquivalentTo(new Tag("<strong>", 1));
            result[1].Should().BeEquivalentTo(new Tag("</strong>", 7));
        }
    }
}