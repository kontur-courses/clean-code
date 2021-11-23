using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Renderer;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlRendererTests
    {
        private HtmlRenderer sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sut = new HtmlRenderer(DefaultTagSets.HtmlTagsBySeparator);
        }

        [TestCaseSource(typeof(HtmlRendererTestCases), nameof(HtmlRendererTestCases.RenderTestCases))]
        public void ParseTokenTest(IEnumerable<Token> tokens, string text, string expectedString)
        {
            var renderedString = sut.Render(tokens, text);

            renderedString.Should().Be(expectedString);
        }
    }
}