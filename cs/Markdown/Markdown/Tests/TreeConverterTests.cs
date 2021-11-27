using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converters;
using Markdown.MdTags;
using Markdown.SyntaxTree;
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
            tree.Root.Should().BeEquivalentTo(new Tag(0, text.Length));
            tree.Children.Should().BeEmpty();
        }

        [Test]
        public void ConvertToTree_AddsFirstTagToChildrenOfMainNode()
        {
            TreeConverter.ConvertToTree("_a_").Children[0].Should().BeEquivalentTo(new Tree(new ItalicsTag(0, 2)));
        }

        [Test]
        public void ConvertToTree_AddsDisjointTagsToChildrenOfMainNode()
        {
            var expectedChildren = new List<Tree> {new Tree(new ItalicsTag(4, 6)), new Tree(new ItalicsTag(0, 2))};
            TreeConverter.ConvertToTree("_a_ _b_").Children.Should()
                .BeEquivalentTo(expectedChildren, options => options.WithoutStrictOrdering());
        }

        [Test]
        public void ConvertToTree_CorrectlyAddsIntersectingTags()
        {
            var tree = TreeConverter.ConvertToTree("#a _b_");
            var child = tree.Children[0];
            child.Root.Should().BeEquivalentTo(new TitleTag(0, 6));
            child.Children[0].Root.Should().BeEquivalentTo(new ItalicsTag(3, 5));
        }

        [TestCase("_x__a__x_", TestName = "strong text tag is children of italic tag")]
        [TestCase("*_a_*", TestName = "italic tag is children of unnumbered list")]
        [TestCase("#*a*", TestName = "unnumbered list is children of title tag")]
        [TestCase("#+a+", TestName = "list element is children of title tag")]
        public void ConvertToTree_NotTagToChildrenOfAnother(string text)
        {
            var tree = TreeConverter.ConvertToTree(text);
            var child = tree.Children[0];
            child.Children.Should().BeEmpty();
        }

        [Test]
        public void ConvertToTree_NotAddsTagToChildrenOfListElement_WhenItIsChildrenOfUnnumberedList()
        {
            var tree = TreeConverter.ConvertToTree("*+_a_+*");
            var child = tree.Children[0].Children[0];
            child.Children.Should().BeEmpty();
        }

        [Test]
        public void ConvertToTree_NotAddsListElement_WhenItIsNotInsideUnnumberedList()
        {
            var tree = TreeConverter.ConvertToTree("+a+");
            var child = tree.Children;
            child.Should().BeEmpty();
        }
    }
}