using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests.TagTests;

[TestFixture]
public class MarkedListTagTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag>
            { new HeaderTag(), new ItalicTag(), new StrongTag(), new MarkedListTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenListAtStartOfLine_ConvertsCorrectly()
    {
        var text = "* элемент списка";
        var expected = "<ul><li>элемент списка</li></ul>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenListInMiddleOfText_NotConverted()
    {
        var text = "Текст * элемент списка не должен преобразовываться";
        var expected = "Текст * элемент списка не должен преобразовываться";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenMultipleListItems_ConvertsCorrectly()
    {
        var text = "* первый элемент\n* второй элемент";
        var expected = "<ul><li>первый элемент</li>\n<li>второй элемент</li></ul>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenListAfterNewline_ConvertsCorrectly()
    {
        var text = "Предыдущий абзац\n* элемент после перевода строки";
        var expected = "Предыдущий абзац\n<ul><li>элемент после перевода строки</li></ul>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenOnlyAsteriskWithoutSpace_NotConverted()
    {
        var text = "*";
        var expected = "*";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenListWithOnlyAsteriskAndSpace_ConvertsToEmptyListItem()
    {
        var text = "* ";
        var expected = "<ul><li></li></ul>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenListWithoutSpaceAfterAsterisk_NotConverted()
    {
        var text = "*элемент без пробела";
        var expected = "*элемент без пробела";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenListWithMultipleSpacesAfterAsterisk_ConvertsCorrectly()
    {
        var text = "*   элемент с несколькими пробелами";
        var expected = "<ul><li>  элемент с несколькими пробелами</li></ul>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
    
    [Test]
    public void WhenListFollowedByText_ConvertsCorrectly()
    {
        var text = "* элемент списка\nобычный текст";
        var expected = "<ul><li>элемент списка</li></ul>\nобычный текст";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}