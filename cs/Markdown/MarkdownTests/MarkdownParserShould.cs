using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MarkdownParserShould
    {
        private MarkdownTree _emptyRoot;

        [SetUp]
        public void CreateNewEmptyRoot()
        {
            _emptyRoot = new MarkdownTree(new Tag(TagKind.Root, TagSide.None), "");
        }

        [Test]
        public void ReturnEmptyRoot_WhenInputStringIsEmpty()
        {
            var inputString = "";

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(_emptyRoot);
        }

        [Test]
        public void ReturnOneChildedTree_WhenInputIsPlainText()
        {
            var inputString = "Step, step up, step, step up";
            var child = new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None),
                "Step, step up, step, step up");
            _emptyRoot.AddChild(child);

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(_emptyRoot);
        }

        [Test]
        public void ParseSimpleTextWithHeadersCorrect()
        {
            var inputString = "Hi all!\n#What's up?\n";
            _emptyRoot.AddChild(new MarkdownTree(new Tag(TagKind.PlainText, TagSide.None), "Hi all!\n"));
            _emptyRoot.AddChild(new MarkdownTree(new Tag(TagKind.Header, TagSide.Opening), "What's up?\n"));

            var parserResult = MarkdownParser.Parse(inputString);

            parserResult.Should().BeEquivalentTo(_emptyRoot);
        }
    }
}
