using FluentAssertions;
using NUnit.Framework;
namespace Markdown.Tests
{
    [TestFixture]
    internal class MarkdownTests
    {
        [TestCase("_Привет_, _два_ выделения.", @"<em>Привет<\em>, <em>два<\em> выделения.")]

        [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка",
              @"Текст, <em>окруженный с двух сторон<\em> одинарными символами подчерка")]
        public void Render_ShouldItalicize_WhenMarkupIsCorrect(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }

        [TestCase("__Два__ выделенных __слова__",
            @"<strong>Два<\strong> выделенных <strong>слова<\strong>")]
        [TestCase("__Выделенный двумя символами текст__ должен становиться полужирным",
              @"<strong>Выделенный двумя символами текст<\strong> должен становиться полужирным")]
        public void Render_ShouldHighlightBold_WhenMarkupIsCorrect(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }

        [TestCase(@"\_\_Вот это\_\_, не должно выделиться тегом", "__Вот это__, не должно выделиться тегом")]
        [TestCase(@"\_Вот это\_, не должно выделиться тегом", "_Вот это_, не должно выделиться тегом")]
        public void Render_ShouldEscapeCharacters_WhenCharactersCanBeEscaped(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }

        [TestCase(@"\F\", @"\F\")]
        [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\",
            @"Здесь сим\волы экранирования\ \должны остаться.\")]
        public void Render_EscapeCharactersMustRemain_WhenCharactersCanNotBeEscaped(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }

        [TestCase(@"\\__вот это будет выделено тегом\\__", @"<strong>вот это будет выделено тегом<\strong>")]
        [TestCase(@"\\_вот это будет выделено тегом_", @"<em>вот это будет выделено тегом<\em>")]
        public void Render_ShouldEscape_EscapeCharacter(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }

        [Test]
        public void Render_SinglesHighlightShouldWork_InsideADoubleHighlight()
        {
            var text = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(@"Внутри <strong>двойного выделения <em>одинарное<\em> тоже<\strong> работает.");
        }

        [Test]
        public void Render_DoubleHighlightShouldNotWork_InsideASingleHighlight()
        {
            var text = "Внутри _одинарного __двойное__ не_ работает.";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(@"Внутри <em>одинарного __двойное__ не<\em> работает.");
        }

        [Test]
        public void Render_DoNotHighlight_NextToNumbers()
        {
            var text = "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.");
        }

        [Test]
        public void Render_ShouldHighlightPartsOfWord_WhenHighlightingIsNotInDifferentWords()
        {
            var text = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(@"Однако выделять часть слова они могут: и в <em>нач<\em>але, и в сер<em>еди<\em>не, и в кон<em>це.<\em>");
        }

        [Test]
        public void Render_ShouldNotHighlightPartsOfWord_WhenHighlightingIsInDifferentWords()
        {
            var text = "В то же время выделение в ра_зных сл_овах не работает.";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be("В то же время выделение в ра_зных сл_овах не работает.");
        }

        [Test]
        public void Render_UnpairedСharactersShouldNotBeHighlighted()
        {
            var text = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be("__Непарные_ символы в рамках одного абзаца не считаются выделением.");
        }


        [TestCase("Эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.")]
        [TestCase("Эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.")]
        public void Render_ShouldHandleHighlightCasesWithWhitespace_WhenSpacesAreNextToHighlights(string text)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(text);
        }

        [Test]
        public void Render_ShouldNotHighlightEmptyString()
        {
            var text = "____";
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(text);
        }

        [TestCase("# Заголовок", @"<h1>Заголовок<\h1>")]
        [TestCase("# Заголовок __с _разными_ символами__",
            @"<h1>Заголовок <strong>с <em>разными<\em> символами<\strong><\h1>")]
        public void Render_ShouldHighlightHeaders(string text, string expected)
        {
            var result = Markdown.MarkdownСomponents.Markdown.Render(text);
            result.Should().Be(expected);
        }
    }
}
