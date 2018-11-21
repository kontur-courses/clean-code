using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputIsEmpty()
        {
            var mdParser = new Parser("");
            var mdDocument = mdParser.Parse();
            mdDocument.RootNode.Child.Count.Should().Be(0);
        }
        
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputHasSimpleText()
        {
            var mdParser = new Parser("simple text");
            var mdDocument = mdParser.Parse();
            mdDocument.RootNode.Child.Count.Should().Be(1);
            mdDocument.RootNode.Child.First().Should().BeEquivalentTo(new TextNode("simple text"));
        }
        
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputHasOneTag()
        {
            var mdParser = new Parser("__simple text__");
            var mdDocument = mdParser.Parse();
            mdDocument.RootNode.Child.Count.Should().Be(1);
            var tagNode = mdDocument.RootNode.Child.First();
            tagNode.Type.Should().Be(NodeType.DoubleUnderlineTag);
            tagNode.Child.Count.Should().Be(1);
            tagNode.Child.First().Should().BeEquivalentTo(new TextNode("simple text"));
        }
        
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputHasTextAndTag()
        {
            var mdParser = new Parser("some text _simple text_");
            var mdDocument = mdParser.Parse();
            
            mdDocument.RootNode.Child.Count.Should().Be(2);
            mdDocument.RootNode.Child[0].Should().BeEquivalentTo(new TextNode("some text "));

            var tagNode = mdDocument.RootNode.Child[1];
            tagNode.Type.Should().Be(NodeType.SingleUnderlineTag);
            tagNode.Child.Count.Should().Be(1);
            tagNode.Child.First().Should().BeEquivalentTo(new TextNode("simple text"));
        }
        
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputHasTextAndTagAndText()
        {
            var mdParser = new Parser("some text __simple text__ some text 2");
            var mdDocument = mdParser.Parse();
            
            mdDocument.RootNode.Child.Count.Should().Be(3);
            mdDocument.RootNode.Child[0].Should().BeEquivalentTo(new TextNode("some text "));

            var tagNode = mdDocument.RootNode.Child[1];
            tagNode.Type.Should().Be(NodeType.DoubleUnderlineTag);
            tagNode.Child.Count.Should().Be(1);
            tagNode.Child.First().Should().BeEquivalentTo(new TextNode("simple text"));
            
            mdDocument.RootNode.Child[2].Should().BeEquivalentTo(new TextNode(" some text 2"));
        }
        
        [Test]
        public void Parser_ShouldReturnCorrectDocument_WhenInputHasNestedTags()
        {
            var mdParser = new Parser("__very _simple_ _simple_ text__");
            var mdDocument = mdParser.Parse();
            
            mdDocument.RootNode.Child.Count.Should().Be(1);

            var parentTag = mdDocument.RootNode.Child[0];

            parentTag.Type.Should().Be(NodeType.DoubleUnderlineTag);
            parentTag.Child.Count.Should().Be(5);
           
            parentTag.Child[0].Should().BeEquivalentTo(new TextNode("very "));
            parentTag.Child[2].Should().BeEquivalentTo(new TextNode(" "));
            parentTag.Child[4].Should().BeEquivalentTo(new TextNode(" text"));

            var childTag1 = parentTag.Child[1];
            childTag1.Type.Should().Be(NodeType.SingleUnderlineTag);
            childTag1.Child.Count.Should().Be(1);
            childTag1.Child[0].Should().BeEquivalentTo(new TextNode("simple"));

            var childTag2 = parentTag.Child[3];
            childTag2.Type.Should().Be(NodeType.SingleUnderlineTag);
            childTag2.Child.Count.Should().Be(1);
            childTag2.Child[0].Should().BeEquivalentTo(new TextNode("simple"));
        }
    }
}