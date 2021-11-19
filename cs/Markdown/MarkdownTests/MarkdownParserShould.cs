using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MarkdownParserShould
    {
        [Test]
        public void ReturnEmptyRoot_WhenInputStringIsEmpty()
        {
            var inputString = "";
            var emptyRoot = new MarkdownTree(new Tag(TagKind.Root, TagSide.None), "");
            var emptyChild = new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None), "");
            emptyRoot.AddChild(emptyChild);

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(emptyRoot);
        }

        [Test]
        public void ReturnOneChildTree_WhenInputStringIsPlainText()
        {
            var inputString = "Step, step up, step, step up";
            var oneChildTree = new MarkdownTree(new Tag(TagKind.Root, TagSide.None), "");
            var child = new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None),
                "Step, step up, step, step up");
            oneChildTree.AddChild(child);

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(oneChildTree);
        }
    }
}
