// MarkdownExtraTests.cs
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace Markdown.tests
{
    [TestFixture]
    public class MarkdownExtraTests
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md();
        }

        [Test]
        public void MarkdownSpec_ShouldReturnNull_WhenInputIsNull()
        {
            md.Render(null).Should().Be(null);
        }

        [TestCase("#НеЗаголовок", "#НеЗаголовок", TestName = "Без пробела # не заголовок")]              
        [TestCase("_пример число12 текста_", "_пример число12 текста_", TestName = "Числа не выделяются em")]                           
        [TestCase("__пример число12 текста__", "__пример число12 текста__", TestName = "Числа не выделяются strong")]
        [TestCase("текст_нет выделения_конец", "текст_нет выделения_конец", TestName = "Раздельные части не выделяются")]
        [TestCase("____", "____", TestName = "Пустая строка не выделяется strong")]
        [TestCase("__", "__", TestName = "Пустая строка не выделяется em")]
        public void MarkdownSpec_ShouldReturnExpectedValue_WhenNoChangesExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
        [TestCase("_выделение_конец", "<em>выделение</em>конец", TestName = "Выделение в начале em")]
        [TestCase("текст_выделение_конец", "текст<em>выделение</em>конец", TestName = "Выделение в середине em")]
        [TestCase("текст_выделение_", "текст<em>выделение</em>", TestName = "Выделение в конце em")]
        [TestCase("_а__б__с_", "<em>а__б__с</em>", TestName = "<strong> не работает в em")]
        [TestCase("_а_ _б_", "<em>а</em> <em>б</em>", TestName = "Два тега em")]
        [TestCase("_пример _текста_ пример_", "_пример <em>текста</em> пример_", TestName = "Нет вложенных em")]
        [TestCase("a_b c_d_", "a_b c<em>d</em>", TestName = "Пробел блокирует выделение em")]
        public void MarkdownSpec_ShouldReturnExpectedValue_WhenEmExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
        [TestCase("__выделение__конец", "<strong>выделение</strong>конец", TestName = "Выделение в начале strong")]
        [TestCase("текст__выделение__конец", "текст<strong>выделение</strong>конец", TestName = "Выделение в середине strong")]
        [TestCase("текст__выделение__", "текст<strong>выделение</strong>", TestName = "Выделение в конце strong")]
        [TestCase("__а__ __б__", "<strong>а</strong> <strong>б</strong>", TestName = "Два тега strong")]
        [TestCase("__пример __текста__ пример__", "__пример <strong>текста</strong> пример__", TestName = "Нет вложенных strong")]
        [TestCase("a__b c__d__", "a__b c<strong>d</strong>", TestName = "Пробел блокирует выделение strong")]
        public void MarkdownSpec_ShouldReturnExpectedValue_WhenStrongExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
        [TestCase("__а_b_в__", "<strong>а<em>b</em>в</strong>",TestName = "em разрешено в strong")]
        [TestCase("__a__ _b_", "<strong>a</strong> <em>b</em>")]
        public void MarkdownSpec_ShouldReturnExpectedValue_WhenStrongAndEmExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase(@"\\_а_", @"\<em>а</em>", TestName = @"Экранироание \")]
        [TestCase(@"\# Заголовок", "# Заголовок", TestName = "Экранирование #")]
        [TestCase(@"\_пример 1 текста_", "_пример 1 текста_", TestName = "Экранирование без пары")]
        [TestCase(@"\_x_", "_x_", TestName = "Экранирование em")]
        [TestCase(@"\__x__", "__x__", TestName = "Экранирование strong")]
        public void MarkdownSpec_ShouldReturnExpectedValue_WhenEscapingExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("# Заголовок", "<h1>Заголовок</h1>", TestName = "Базовый заголовок")]
        [TestCase("# Заголовок __bold__", "<h1>Заголовок <strong>bold</strong></h1>", TestName = "Заголовок strong")]
        [TestCase("# Заголовок _cursive_", "<h1>Заголовок <em>cursive</em></h1>", TestName = "Заголовок em")]

        public void MarkdownSpec_ShouldReturnExpectedValue_WhenHeaderExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

    }
}
