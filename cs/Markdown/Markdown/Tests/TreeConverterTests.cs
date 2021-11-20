using FluentAssertions;
using Markdown.Converters;
using Markdown.Tag;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class TreeConverterTests
    {
        [Test]
        public void ConvertToTree_ReturnsNull_WhenTextIsNull()
        {
            TreeConverter.ConvertToTree(null).Should().Be(null);
        }

        [TestCase("", TestName = "text is empty")]
        [TestCase("   ", TestName = "text is whitespace")]
        [TestCase("kek", TestName = "text not contains tags")]
        [TestCase("_", TestName = "text contains not closed tag")]
        public void ConvertToTree_ReturnsTreeWithoutTags_WhenTextHasNoTags(string text)
        {
            var tree = TreeConverter.ConvertToTree(text);

            tree.Tag.Should().BeEquivalentTo(new EmptyTag(0, text.Length));
            tree.Children.Should().BeEmpty();
        }

        [Test]
        public void ConvertToTree_AddsFirstTagToChildrenOfMainNode()
        {
            TreeConverter.ConvertToTree("_a_").Children[0].Should().BeEquivalentTo(new Node(new ItalicsTag(0, 2)));
        }

        [Test]
        public void ConvertToTree_AddsDisjointTagsToChildrenOfMainNode()
        {
            var children = TreeConverter.ConvertToTree("_a_ _b_").Children;

            children[0].Should().BeEquivalentTo(new Node(new ItalicsTag(4, 6)));
            children[1].Should().BeEquivalentTo(new Node(new ItalicsTag(0, 2)));
        }

        [Test]
        public void ConvertToTree_CorrectlyAddsIntersectingTags()
        {
            var tree = TreeConverter.ConvertToTree("#a _b_ \n");
            var child = tree.Children[0];

            child.Tag.Should().BeEquivalentTo(new TitleTag(0, 7));
            child.Children[0].Tag.Should().BeEquivalentTo(new ItalicsTag(3, 5));
        }
    }
}