using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests.TagTests;

[TestFixture]
public class ItalicTagTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag> { new ItalicTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenSimpleItalic_ConvertsCorrectly()
    {
        var text = "Текст _окруженный с двух сторон_ одинарными символами";
        var expected =
            "Текст <em>окруженный с двух сторон</em> одинарными символами";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicAtWordStart_ConvertsCorrectly()
    {
        var text = "и в _нач_але слова";
        var expected = "и в <em>нач</em>але слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicAtWordEnd_ConvertsCorrectly()
    {
        var text = "и в кон_це_ слова";
        var expected = "и в кон<em>це</em> слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicInWordMiddle_ConvertsCorrectly()
    {
        var text = "и в сер_еди_не слова";
        var expected = "и в сер<em>еди</em>не слова";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicWithWhitespaceAfter_NotConverted()
    {
        var text = "эти_ подчерки_ не считаются выделением";
        var expected = "эти_ подчерки_ не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicWithWhitespaceBefore_NotConverted()
    {
        var text = "эти _подчерки _не считаются выделением";
        var expected = "эти _подчерки _не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenUnderscoreInNumbers_NotConverted()
    {
        var text =
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением";
        var expected =
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenItalicAcrossDifferentWords_NotConverted()
    {
        var text = "В то же время выделение в ра_зных сл_овах не работает";
        var expected = "В то же время выделение в ра_зных сл_овах не работает";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenEmptyItalic_NotConverted()
    {
        var text = "Если внутри подчерков пустая строка __, то они остаются";
        var expected =
            "Если внутри подчерков пустая строка __, то они остаются";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenUnpairedItalic_NotConverted()
    {
        var text = "_Непарные символы не считаются выделением";
        var expected = "_Непарные символы не считаются выделением";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenOnlyOpeningItalic_NotConverted()
    {
        var text = "_Текст без закрывающих тегов";
        var expected = "_Текст без закрывающих тегов";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenOnlyClosingItalic_NotConverted()
    {
        var text = "Текст без открывающих тегов_";
        var expected = "Текст без открывающих тегов_";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}
