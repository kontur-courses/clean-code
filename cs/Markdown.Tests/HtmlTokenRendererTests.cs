using System;
using FluentAssertions;
using Markdown.Extensions;
using Markdown.TagRenderer;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class HtmlTokenRendererTests
    {
        private HtmlTagRenderer sut;

        [SetUp]
        public void SetUp()
        {
            sut = new HtmlTagRenderer();
        }

        [Test]
        public void Render_ShouldThrowException_WhenTokensIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Render(null));
        }

        [Test]
        public void Render_ShouldReturnEmptyString_WhenTokensEmpty()
        {
            var html = sut.Render(Array.Empty<TagNode>());

            html.Should().Be(string.Empty);
        }

        [Test]
        public void Render_ShouldReturnText_WhenTextNode()
        {
            var node = Tag.Text("Text").ToNode();

            var html = sut.Render(new[] { node });

            html.Should().Be("Text");
        }

        [Test]
        public void Render_ShouldReturnFormattedText_WhenSurroundsByCursive()
        {
            var node = new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("Text").ToNode());

            var html = sut.Render(new[] { node });

            html.Should().Be("<em>Text</em>");
        }

        [Test]
        public void Render_ShouldReturnFormattedText_WhenSurroundsByBold()
        {
            var node = new TagNode(Tag.Bold(Token.Bold.Value), Tag.Text("Text").ToNode());

            var html = sut.Render(new[] { node });

            html.Should().Be("<strong>Text</strong>");
        }

        [Test]
        public void Render_ShouldReturnFormattedText_WhenStartsWithHeader1()
        {
            var node = new TagNode(Tag.Header1(Token.Header1.Value), Tag.Text("Text").ToNode());

            var html = sut.Render(new[] { node });

            html.Should().Be("<h1>Text</h1>");
        }

        [Test]
        public void Render_ShouldReturnFormattedText_WhenHasInnerFormatting()
        {
            var node = new TagNode(Tag.Header1(Token.Header1.Value), new[]
            {
                Tag.Text("A").ToNode(),
                new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("B").ToNode()),
                Tag.Text("C").ToNode()
            });

            var html = sut.Render(new[] { node });

            html.Should().Be("<h1>A<em>B</em>C</h1>");
        }
    }
}