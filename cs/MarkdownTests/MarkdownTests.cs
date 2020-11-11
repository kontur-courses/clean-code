using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        [TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>",
            TestName = "Render_WhenOneItalicField")]
        public void Render_WhenItalicMarkdown(string markdownText, string expected)
        {
            var actual = Markdown.Markdown.Render(markdownText);
            actual.Should().Be(expected);
        }

        [TestCase("__Выделенный двумя символами текст__", "<strong>Выделенный двумя символами текст</strong>",
            TestName = "Render_WhenOneBoldField")]
        public void Render_WhenBoldMarkdown(string markdownText, string expected)
        {
            var actual = Markdown.Markdown.Render(markdownText);
            actual.Should().Be(expected);
        }

        [TestCase("\\_Вот это\\_", "_Вот это_", TestName = "Render_WhenEscapedMarkdowns")]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.",
            "Здесь сим\\волы экранирования\\ \\должны остаться.", TestName = "Render_WhenEscapingSymbolsEscapeNothing")]
        public void Render_WhenThereAreEscapingSymbols(string markdownText, string expected)
        {
            var actual = Markdown.Markdown.Render(markdownText);
            actual.Should().Be(expected);
        }

        [TestCase(@"\\_вот это будет выделено тегом_", @"\<em>вот это будет выделено тегом</em>",
            TestName = "Render_WhenEscapingSymbolIsEscaped")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает",
            "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает",
            TestName = "Render_WhenItalicIsInsideBold")]
        [TestCase("внутри _одинарного __двойное__ не_ работает", "внутри <em>одинарного __двойное__ не</em> работает",
            TestName = "Render_WhenBoldIsInsideItalic")]
        [TestCase("Подчерки внутри текста c цифрами_12_3 не считаются",
            "Подчерки внутри текста c цифрами_12_3 не считаются", TestName = "Render_WhenDigitsInMarkdowns")]
        [TestCase("в _нач_але, и в сер_еди_не, и в кон_це._",
            "в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>",
            TestName = "Render_WhenMarkdownsAreInsideWord")]
        [TestCase("в ра_зных сл_овах не работает", "в ра_зных сл_овах не работает",
            TestName = "Render_WhenMarkdownsAreInsideDifferentWord")]
        [TestCase("__Непарные_ символы в рамках одного абзаца", "__Непарные_ символы в рамках одного абзаца",
            TestName = "Render_WhenUnpairedMarkdownsInOneParagraph")]
        [TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением",
            TestName = "Render_WhenWhitespaceAfterOpeningMarkdown")]
        [TestCase(" эти _подчерки _не считаются_", " эти _подчерки _не считаются_",
            TestName = "Render_WhenWhitespaceBeforeClosingMarkdown")]
        [TestCase("__пересечения _двойных__ и одинарных_ подчерков", "__пересечения _двойных__ и одинарных_ подчерков",
            TestName = "Render_WhenItalicAndBoldAreIntersected")]
        [TestCase("внутри подчерков пустая строка ____", "внутри подчерков пустая строка ____",
            TestName = "Render_WhenEmptyLineInMarkdowns")]
        public void Render_WhenInteractionOfMarkdowns(string markdownText, string expected)
        {
            var actual = Markdown.Markdown.Render(markdownText);
            actual.Should().Be(expected);
        }

        [TestCase("# Заголовки", "<h1> Заголовки</h1>", TestName = "Render_WhenTitle")]
        [TestCase("# Заголовок __с _разными_ символами__",
            "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Render_WhenTitleWithAnotherMarkdowns")]
        public void Render_WhenTitleMarkdown(string markdownText, string expected)
        {
            var actual = Markdown.Markdown.Render(markdownText);
            actual.Should().Be(expected);
        }
    }
}