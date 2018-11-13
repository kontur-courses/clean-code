using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown
{

    [TestFixture]
    public class HtmlRendererTests
    {
        private HtmlRenderer htmlRenderer;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            htmlRenderer = new HtmlRenderer();
        }

        [Test]
        public void Render_ThrowsException_OnNullTokens()
        {
            Action action = () => htmlRenderer.Render(null);
            action
                .Should()
                .Throw<ArgumentException>();
        }

        [TestCase(MdType.Text, "", "", TestName = "empty text")]
        [TestCase(MdType.Text, "hello world", "hello world", TestName = "plain text")]
        [TestCase(MdType.OpenEmphasis, "<ul>", "", TestName = "open emphasis")]
        [TestCase(MdType.CloseEmphasis, "</ul>", "", TestName = "close emphasis")]
        [TestCase(MdType.OpenStrongEmphasis, "<strong>", "", TestName = "open strong emphasis")]
        [TestCase(MdType.CloseStrongEmphasis, "</strong>", "", TestName = "close strong emphasis")]
        public void Render_ReturnsCorrectHtmlString_WithMdType(MdType type, string expected, string value = "")
        {
            var token = new Token(type, value);
            var result =  htmlRenderer.Render(new[] {token});

            result
                .Should()
                .Be(expected);
        }

        [Test]
        public void Render_ReturnsCorrectHtmlString_WithDifferentTags()
        {
            var tokens = new[]
            {
                new Token(MdType.OpenStrongEmphasis, ""), 
                new Token(MdType.OpenEmphasis, ""), 
                new Token(MdType.Text, "Hello"), 
                new Token(MdType.CloseEmphasis, ""), 
                new Token(MdType.CloseStrongEmphasis, ""), 
            };

            var expected = "<strong><ul>Hello</ul></strong>";
            var result = htmlRenderer.Render(tokens);

            result
                .Should()
                .Be(expected);
        }
    }
}