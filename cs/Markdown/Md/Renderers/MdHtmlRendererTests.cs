using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Md.Renderers
{
    [TestFixture]
    public class MdHtmlRendererTests
    {
        private IRenderer htmlRenderer;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            htmlRenderer = new MdHtmlRenderer(MdSpecification.HtmlRules);
        }

        [Test]
        public void Render_WhenNullTokens_ThrowsException()
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
        public void Render_WhenMdType_ReturnsCorrectHtmlString(MdType type, string expected, string value = "")
        {
            var token = new MdToken(type, value);
            var result = htmlRenderer.Render(new[] {token});

            result
                .Should()
                .Be(expected);
        }

        [Test]
        public void Render_WhenDifferentTags_ReturnsCorrectHtmlString()
        {
            var tokens = new[]
            {
                new MdToken(MdType.OpenStrongEmphasis, ""),
                new MdToken(MdType.OpenEmphasis, ""),
                new MdToken(MdType.Text, "Hello"),
                new MdToken(MdType.CloseEmphasis, ""),
                new MdToken(MdType.CloseStrongEmphasis, ""),
            };

            var expected = "<strong><ul>Hello</ul></strong>";
            var result = htmlRenderer.Render(tokens);

            result
                .Should()
                .Be(expected);
        }
    }
}