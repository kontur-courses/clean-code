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
                new Renderer(MdSpecification.GetHtmlTagHandlerChain()));
        }

        [TestCase("__Hello__", "<html><strong>Hello</strong></html>", TestName = "Bold")]
        [TestCase("___Hello___", "<html><strong><ul>Hello</ul></strong></html>", TestName = "Italic")]
        [TestCase("__Hello, _world___", "<html><strong>Hello, <ul>world</ul></strong></html>", TestName = "italic in bold")]
        [TestCase("_Hello, __world__ !_", "<html><ul>Hello, __world__ !</ul></html>", TestName = "bold in italic")]
        public void Render_WhenDifferentTags_ReturnsCorrectHtmlString(string input, string expected)
        {
            var result = converter.Convert(input);

            result
                .Should()
                .Be(expected);
        }
    }
}