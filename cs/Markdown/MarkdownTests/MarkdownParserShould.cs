using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MarkdownParserShould
    {
        [Test]
        public void ReturnEmptyRootWhenInputStringIsEmpty()
        {
            var inputString = "";
            var emptyRoot = new MarkdownTree(new Tag(TagKind.Root, TagSide.None));

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(emptyRoot);
        }
    }
}
