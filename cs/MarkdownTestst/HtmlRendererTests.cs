using System.Collections.Generic;
using FluentAssertions;
using Markdown.Renderer;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlRendererTests
    {
        private readonly IReadOnlyDictionary<string, HtmlTag> htmlTagsBySeparator = new Dictionary<string, HtmlTag>
        {
            { BoldToken.Separator, new HtmlTag("<strong>", "</strong>", true) },
            { ItalicToken.Separator, new HtmlTag("<em>", "</em>", true) },
            { HeaderToken.Separator, new HtmlTag("<h1>", "</h1>", true) },
            { ScreeningToken.Separator, new HtmlTag(string.Empty, string.Empty, false) },
            { ImageToken.Separator, new HtmlTag("<img >", string.Empty, false) }
        };

        private HtmlRenderer sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sut = new HtmlRenderer(htmlTagsBySeparator);
        }

        [TestCaseSource(typeof(HtmlRendererTestCases), nameof(HtmlRendererTestCases.RenderTestCases))]
        public void ParseTokenTest(IEnumerable<Token> tokens, string text, string expectedString)
        {
            var renderedString = sut.Render(tokens, text);

            renderedString.Should().Be(expectedString);
        }
    }
}