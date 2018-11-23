using FluentAssertions;
using Markdown.Md;
using NUnit.Framework;

namespace Markdown.Renderers
{
    [TestFixture]
    public class MdHtmlRendererTests
    {
        private IConverter converter;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            converter = new Md.Md(new Parser(MdSpecification.GetTagHandlerChain(), new TagToTextTagConverter()),
                new HtmlRenderer(MdSpecification.GetHtmlTagHandlerChain()));
        }

        [TestCase("__Hello__", "<strong>Hello</strong>", TestName = "Bold")]
        [TestCase("___Hello___", "<strong><ul>Hello</ul></strong>", TestName = "Italic")]
        [TestCase("__Hello, _world___", "<strong>Hello, <ul>world</ul></strong>", TestName = "italic in bold")]
        [TestCase("_Hello, __world__ !_", "<ul>Hello, __world__ !</ul>", TestName = "bold in italic")]
        public void Render_WhenDifferentTags_ReturnsCorrectHtmlString(string input, string expected)
        {
            var result = converter.Convert(input);

            result
                .Should()
                .Be(expected);
        }
    }
}