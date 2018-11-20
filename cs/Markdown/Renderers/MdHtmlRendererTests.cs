using System;
using FluentAssertions;
using Markdown.Md;
using NUnit.Framework;

namespace Markdown.Renderers
{
    [TestFixture]
    public class MdHtmlRendererTests
    {
        private IRenderer htmlRenderer;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            htmlRenderer = new HtmlRenderer(MdSpecification.HtmlRules);
        }

        [Test]
        public void Render_WhenNullTokens_ThrowsException()
        {
            Action action = () => htmlRenderer.Render(null);
            action
                .Should()
                .Throw<ArgumentException>();
        }

        [TestCase("text", TokenPairType.NotPair, "", "", TestName = "empty text")]
        [TestCase("text", TokenPairType.NotPair, "hello world", "hello world", TestName = "plain text")]
        [TestCase("emphasis", TokenPairType.Open, "<ul>", "", TestName = "open emphasis")]
        [TestCase("emphasis", TokenPairType.Close, "</ul>", "", TestName = "close emphasis")]
        [TestCase("strong", TokenPairType.Open, "<strong>", "", TestName = "open strong emphasis")]
        [TestCase("strong", TokenPairType.Close, "</strong>", "", TestName = "close strong emphasis")]
        public void Render_WhenMdType_ReturnsCorrectHtmlString(string type, TokenPairType pairType, string expected,
            string value = "")
        {
            var root = new TokenNode("root", "");
            var token = new TokenNode(type, value, pairType);
            root.Children.Add(token);
            var result = htmlRenderer.Render(root);

            result
                .Should()
                .Be(expected);
        }

        [Test]
        public void Render_WhenDifferentTags_ReturnsCorrectHtmlString()
        {
            var root = new TokenNode("root", "");
            var strong = new TokenNode(MdSpecification.StrongEmphasis, "", TokenPairType.Open);
            var ul = new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Open);
            var hello = new TokenNode(MdSpecification.Text, "Hello", TokenPairType.NotPair);
            var closeUl = new TokenNode(MdSpecification.Emphasis, "", TokenPairType.Close);
            var closeStrong = new TokenNode(MdSpecification.StrongEmphasis, "", TokenPairType.Close);

            ul.Children.Add(hello);
            strong.Children.Add(ul);
            strong.Children.Add(closeUl);
            root.Children.Add(strong);
            root.Children.Add(closeStrong);

            var expected = "<strong><ul>Hello</ul></strong>";
            var result = htmlRenderer.Render(root);

            result
                .Should()
                .Be(expected);
        }

        [Test]
        public void Render_WhenRealParserParses_ReturnsCorrectHtmlString()
        {
            var parser = new Parser(MdSpecification.GetTagHandlerChain());
            var tokeNode = parser.Parse("___Hello___");
            var expected = "<strong><ul>Hello</ul></strong>";

            var result = htmlRenderer.Render(tokeNode);

            result
                .Should()
                .Be(expected);
        }
    }
}