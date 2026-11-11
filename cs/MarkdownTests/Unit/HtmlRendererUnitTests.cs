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
            var action = () => renderer.Render(tokens);

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Render_ReturnsEmptyString_WhenEmptyList()
        {
            var tokens = new List<Token>();

            var result = renderer.Render(tokens);

            result.Should().NotBeNull();
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

        [Test]
        public void Render_ReturnsString_WhenTextWithSingleEmTag()
        {
            var tokens = new List<Token>() //эквивалент _text_
            {
               new Token(new TagInfo("_", "em"), new List<Token>() {new Token("text")})
            };
            var expected = "<em>text</em>";

            var result = renderer.Render(tokens);

            result.Should().Be(expected);
        }

        [Test]
        public void Render_ReturnsString_WhenTextWithSingleStrongTag()
        {
            var tokens = new List<Token>() //эквивалент __text__
            {
               new Token(new TagInfo("__", "strong"), new List<Token>() {new Token("text")})
            };
            var expected = "<strong>text</strong>";

            var result = renderer.Render(tokens);

            result.Should().Be(expected);
        }

        [Test]
        public void Render_ReturnsString_WhenNestedStructureEmInStrong()
        {
            var tokens = new List<Token>() //эквивалент ___text___
            {
                new Token(new TagInfo("__", "strong"), new List<Token>()
                {
                    new Token(new TagInfo("_", "em"), new List<Token>()
                    {
                        new Token("text")
                    })
                })
            };

            var expected = "<strong><em>text</em></strong>";

            var result = renderer.Render(tokens);

            result.Should().Be(expected);
        }

        [Test]
        public void Render_ReturnsStringWithOnlyEmTag_WhenNestedStructureStrongInEm()
        {
            var tokens = new List<Token> //эквивалент _i __am__ senya_
            {
                new Token(new TagInfo("_", "em"), new List<Token>
                {
                    new Token("text "),

                    new Token("__strong__ text")
                })
            };

            var expected = "<em>text __strong__ text</em>";

            var result = renderer.Render(tokens);

            result.Should().Be(expected);
        }

        //в целом тест может быть лишним, наверное,
        //но это unit тест именно рендерера, поэтому проверка на рендер детей
        //плюс ко всему он показывает декомпозицию задач по разным классам - рендерер не проверяет правила парсинга, а только переводит в html-разметку
        [Test]
        public void Render_ReturnsString_WithMultipleChildren()
        {

            var tokens = new List<Token> //эквивалент _i __am__ senya 
            {
                new Token(new TagInfo("_", "em"), new List<Token>
                {
                    new Token("i "),

                    new Token(new TagInfo("__", "strong"), new List<Token>
                    {
                        new Token("am")
                    }),
            
                    new Token(" senya")
                })
            };

            var expected = "<em>i <strong>am</strong> senya</em>";

            var result = renderer.Render(tokens);

            result.Should().Be(expected);
        }
    }
}