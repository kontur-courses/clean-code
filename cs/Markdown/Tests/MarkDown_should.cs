using System;
using System.Linq;
using FluentAssertions;
using Markdown.Languages;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MarkDown_should
    {
        [Test]
        public void RenderTree_WhenNull_TrowArgumentException()
        {
            Action action = () => { new MarkDown().RenderTree(null); };
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase("abc  abc")]
        [TestCase("abc")]
        public void RenderTree_WhenOnlyText_TreeContainOneTextNodeWithThisText(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            tree.ChildNode.Should().HaveCount(1);
            var textNode = tree.ChildNode.First() as TextNode;
            textNode.Value.Should().Be(str);
        }

        [TestCase("_abc_", ExpectedResult = TagType.Em)]
        [TestCase("__abc__", ExpectedResult = TagType.Strong)]
        public TagType RenderTree_WhenOneTag_TreeContainOneTagNodeWithTypeOfNode(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            tree.ChildNode.Should().HaveCount(1);
            var tagNode = tree.ChildNode.First() as TagNode;
            return tagNode.TypeTag;
        }

        [TestCase("_abc_", ExpectedResult = "abc")]
        [TestCase("_ab c_", ExpectedResult = "ab c")]
        [TestCase("__abc__", ExpectedResult = "abc")]
        [TestCase("__ab c__", ExpectedResult = "ab c")]
        public string RenderTree_WhenOneTag_TagNodeContainThisText(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            var tagNode = tree.ChildNode.First() as TagNode;
            var textNode = tagNode.ChildNode.First() as TextNode;
            return textNode.Value;
        }
        
        [TestCase("а _abc_ а")]
        public void RenderTree_WhenOneTagWithText_TagNodeContain3Node(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            tree.ChildNode.Should().HaveCount(3);
        }
        
        [TestCase("_abc_ _abc_")]
        public void RenderTree_WhenTwoTag_TagNodeContain3Node(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            tree.ChildNode.Should().HaveCount(3);
        }
        
        [TestCase("_abc _")]
        [TestCase("_ abc_")]
        [TestCase("_abc")]
        [TestCase("abc_")]
        [TestCase("__abc __")]
        [TestCase("__ abc__")]
        [TestCase("_abc__")]
        [TestCase("__abc_")]
        [TestCase("__abc")]
        [TestCase("abc__")]
        [TestCase("___abc___")]
        public void RenderTree_WhenIncorrectTag_TreeContainsText(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            var textNode = tree.ChildNode.First() as TextNode;
            textNode.Value.Should().Be(str);
        }
        
        [TestCase("_2")]
        [TestCase("2_")]
        [TestCase("_2_")]
        [TestCase("2_2_2")]
        [TestCase("__2__")]
        [TestCase("2__2__2")]
        public void RenderTree_WhenTagWithNumber_TreeContainsText(string str)
        {
            var tree = new MarkDown().RenderTree(str);
            var textNode = tree.ChildNode.First() as TextNode;
            textNode.Value.Should().Be(str);
        }
    }
    
    
}