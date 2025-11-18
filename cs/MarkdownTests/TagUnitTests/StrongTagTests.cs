using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests.TagTests;

[TestFixture]
public class StrongTagTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag> { new StrongTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenSimpleStrong_ConvertsCorrectly()
    {
        var text = "__Выделенный двумя символами текст__";
        var expected = "<strong>Выделенный двумя символами текст</strong>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongAtWordStart_ConvertsCorrectly()
    {
        var text = "и в __нач__але слова";
        var expected = "и в <strong>нач</strong>але слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongAtWordEnd_ConvertsCorrectly()
    {
        var text = "и в кон__це__ слова";
        var expected = "и в кон<strong>це</strong> слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongInWordMiddle_ConvertsCorrectly()
    {
        var text = "и в сер__еди__не слова";
        var expected = "и в сер<strong>еди</strong>не слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongWithWhitespaceAfter_NotConverted()
    {
        var text = "эти__ подчерки__ не считаются выделением";
        var expected = "эти__ подчерки__ не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongWithWhitespaceBefore_NotConverted()
    {
        var text = "эти __подчерки __не считаются выделением";
        var expected = "эти __подчерки __не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenUnderscoreInNumbers_NotConverted()
    {
        var text =
            "Подчерки внутри текста c цифрами__12__3 не считаются выделением";
        var expected =
            "Подчерки внутри текста c цифрами__12__3 не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenStrongAcrossDifferentWords_NotConverted()
    {
        var text = "В то же время выделение в ра__зных сл__овах не работает";
        var expected =
            "В то же время выделение в ра__зных сл__овах не работает";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenEmptyStrong_NotConverted()
    {
        var text =
            "Если внутри подчерков пустая строка ________, то они остаются";
        var expected =
            "Если внутри подчерков пустая строка ________, то они остаются";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenUnpairedStrong_NotConverted()
    {
        var text = "__Непарные символы не считаются выделением";
        var expected = "__Непарные символы не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenOnlyOpeningStrong_NotConverted()
    {
        var text = "__Текст без закрывающих тегов";
        var expected = "__Текст без закрывающих тегов";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenOnlyClosingStrong_NotConverted()
    {
        var text = "Текст без открывающих тегов__";
        var expected = "Текст без открывающих тегов__";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}