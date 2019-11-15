using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }


        [Test]
        public void Render_ShouldReturnTextInEmTag_OnTextWithPairUnderscores()
        {
            var text = "Текст _окруженный с двух сторон_ одинарными символами подчерка";
            var expectedResult = "Текст <em>окруженный с двух сторон</em> одинарными символами подчерка";

            var result = md.Render(text);

            result.Should().Be(expectedResult);
        }

        [Test]
        public void Render_ShouldReturnRawText_OnEscapeSymbols()
        {
            var text = @"Здесь \_подчеркивания экранированы\_";
            var expectedResult = "Здесь _подчеркивания экранированы_";

            var result = md.Render(text);

            result.Should().Be(expectedResult);
        }

        [TestCase("Подчерки внутри текста c цифрами_12_3", TestName = "numbers")]
        [TestCase("Подчерки внутри текста это_не_выделение", TestName = "text")]
        public void Render_ShouldReturnRawText_OnUnderscoresInsideText(string text)
        {
            var result = md.Render(text);

            result.Should().Be(text);
        }

        [Test]
        public void Render_ShouldReturnRawText_WhenUnderscoreNotClosed()
        {
            var text = @"Это_ не_ выделение_";
            var expectedResult = text;

            var result = md.Render(text);

            result.Should().Be(expectedResult);
        }

        [Test]
        public void Render_ShouldReturnTextInEm_OnTextWithUnderscoreBetweenSpaces()
        {
            var text = @"Это _не завершение _ выделения_";
            var expectedResult = @"Это <em>не завершение _ выделения</em>";

            var result = md.Render(text);

            result.Should().Be(expectedResult);
        }
    }
}
