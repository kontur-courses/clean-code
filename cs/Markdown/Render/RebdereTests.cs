using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class RendererTests
    {
        private Renderer renderer;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            renderer = new Renderer();
        }

        [Test]
        public void RenderTreeToHTML_TextNode_ReturnsText()
        {
            var textNode = new TextNode("текст");
            
            var result = renderer.RenderTreeToHTML(textNode);
            
            result.Should().Be("текст");
        }

        [Test]
        public void RenderTreeToHTML_EmphasisNode_ReturnsEmphasisTags()
        {
            var emphasisNode = new EmphasisNode();
            emphasisNode.Children.Add(new TextNode("курсив"));
            
            var result = renderer.RenderTreeToHTML(emphasisNode);
            
            result.Should().Be("<em>курсив</em>");
        }

        [Test]
        public void RenderTreeToHTML_StrongNode_ReturnsStrongTags()
        {
            var strongNode = new StrongNode();
            strongNode.Children.Add(new TextNode("жирный"));
            
            var result = renderer.RenderTreeToHTML(strongNode);
            
            result.Should().Be("<strong>жирный</strong>");
        }

        [Test]
        public void RenderTreeToHTML_HeaderNode_ReturnsHeaderTags()
        {
            var headerNode = new HeaderNode();
            headerNode.Children.Add(new TextNode("Заголовок"));
            
            var result = renderer.RenderTreeToHTML(headerNode);
            
            result.Should().Be("<h1>Заголовок</h1>");
        }

        [Test]
        public void RenderTreeToHTML_ListNode_ReturnsListTags()
        {
            var listNode = new ListNode();
            var listItem = new ListItemNode();
            listItem.Children.Add(new TextNode("пункт"));
            listNode.Children.Add(listItem);
            
            var result = renderer.RenderTreeToHTML(listNode);
            
            result.Should().Be("<ul>\n<li>пункт</li>\n</ul>");
        }



        [Test]
        public void RenderTreeToHTML_NextLineNode_ReturnsEmptyString()
        {
            var nextLineNode = new NextLineNode();
            
            var result = renderer.RenderTreeToHTML(nextLineNode);
            
            result.Should().Be("\n");
        }



        [Test]
        public void RenderTreeToHTML_NestedEmphasisInStrong_RendersNestedTags()
        {
            var strongNode = new StrongNode();
            strongNode.Children.Add(new TextNode("жирный "));
            
            var emphasisNode = new EmphasisNode();
            emphasisNode.Children.Add(new TextNode("курсив"));
            strongNode.Children.Add(emphasisNode);
            
            var result = renderer.RenderTreeToHTML(strongNode);
            
            result.Should().Be("<strong>жирный <em>курсив</em></strong>");
        }




    }
}