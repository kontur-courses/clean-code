using NUnit.Framework;
using Markdown;
using FluentAssertions;

namespace MarkdownTests.Unit
{
    [TestFixture]
    internal class HtmlRendererUnitTests
    {
        private IHtmlRenderer renderer;

        [SetUp]
        public void Setup()
        {
            renderer = new HtmlRenderer(); // недоступ изза уровня защиты internal СПРОСИТЬ
        }

        [TestCase(null)]
        public void Render_ThrowsException_WhenInvalidData(List<Token> tokens)
        {
            var result = renderer.Render(tokens);

            result.Should().Throw();
        }

        [Test]
        public void Render_ReturnsEmptyString_WhenEmptyList()
        {
            var tokens = new List<Token>();

            var result = renderer.Render(tokens);

            result.Should().BeEmpty();
        }

        [TestCase("text without tags йоу")]
        [TestCase("x")]
        [TestCase("       x       ")]
        [TestCase("0123456789")]
        public void Render_ReturnsString_WhenSingleText(string tokenText)
        {
            var tokens = new List<Token>
            {
                new Token(tokenText)
            };

            var result = renderer.Render(tokens);

            result.Should().Be(tokenText);
        }
    }
}