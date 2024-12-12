using System.Text;
using FluentAssertions;
using FluentAssertions.Extensions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests;

public class MarkdownTests
{
    [Test]
    public void Md_ShouldReturnCorrectString_WithHeadingTag()
    {
        var result = Md.Render("# Thank God It's Christmas \r\n # Queen");
        var expected = "<h1> Thank God It's Christmas </h1>\n <h1> Queen</h1>";
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Md_ShouldReturnCorrectString_WithMixTags()
    {
        var result = Md.Render("# __Princes _of the_ Universe__ [littleLink](Yes)");
        var expected = "<h1> <strong>Princes <em>of the</em> Universe</strong> <a href=\"Yes\">littleLink</a></h1>";
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Md_ShouldReturnCorrectString_WithItalicInsideBoldTag()
    {
        var result = Md.Render("__Too Much _Love Will_ Kill You__");
        var expected = "<strong>Too Much <em>Love Will</em> Kill You</strong>";
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Md_ShouldReturnCorrectString_WithItaBoldInsideItalicTag()
    {
        var result = Md.Render("_We __Will Rock__ You_");
        var expected = "<em>We __Will Rock__ You</em>";
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Md_ShouldReturnCorrectString_WithUnpairTagsInSameParagraph()
    {
        var result = Md.Render("__Another One_ Bites the Dust \n __Another _One Bites the Dust");
        var expected = "__Another One_ Bites the Dust \n __Another _One Bites the Dust";
        result.Should().BeEquivalentTo(expected);
    }

    [TestCase("п_24_54", ExpectedResult = "п_24_54")]
    [TestCase("п__24__54", ExpectedResult = "п__24__54")]
    public string Md_ShouldReturnCorrectString_WithNumbers(string text)
    {
        return Md.Render(text);
    }

    [TestCase("____", ExpectedResult = "____")]
    [TestCase("__", ExpectedResult = "__")]
    public string Md_ShouldReturnCorrectString_WithEmptyTags(string text)
    {
        return Md.Render(text);
    }

    [TestCase("\\", ExpectedResult = "\\")]
    [TestCase("\\__", ExpectedResult = "__")]
    [TestCase("\\_", ExpectedResult = "_")]
    [TestCase("\\#", ExpectedResult = "#")]
    [TestCase("\\w", ExpectedResult = "\\w")]
    [TestCase("\\# I want to break free", ExpectedResult = "# I want to break free")]
    [TestCase("\\# I \\__wan\\__t \\_to\\_ break free", ExpectedResult = "# I __wan__t _to_ break free")]

    public string Md_ShouldReturnCorrectString_WithSlashes(string text)
    {
        return Md.Render(text);
    }

    [TestCase("don't_ stop_ me_ now_", ExpectedResult = "don't_ stop_ me_ now_")]
    [TestCase("don't__ stop__ me__ now__", ExpectedResult = "don't__ stop__ me__ now__")]
    public string Md_ShouldReturnCorrectString_WithSpacesAfterOpenTags(string text)
    {
        return Md.Render(text);
    }

    [TestCase("_Bohemian _Rhapsody by Queen_", ExpectedResult = "_Bohemian <em>Rhapsody by Queen</em>")]
    [TestCase("__Bohemian __Rhapsody by Queen__", ExpectedResult = "__Bohemian <strong>Rhapsody by Queen</strong>")]
    public string Md_ShouldReturnCorrectString_WithSpacesBeforeCloserTags(string text)
    {
        return Md.Render(text);
    }


    [TestCase("__The _Show__ Must_ Go On", ExpectedResult = "__The _Show__ Must_ Go On")]
    [TestCase("_The __Show_ Must__ Go On", ExpectedResult = "_The __Show_ Must__ Go On")]
    public string Md_ShouldReturnCorrectString_WithCrossTags(string text)
    {
        return Md.Render(text);
    }

    [TestCase("I Wa_nt to Br_eak Free", ExpectedResult = "I Wa_nt to Br_eak Free")]
    [TestCase("I Wa__nt to Br__eak Free", ExpectedResult = "I Wa__nt to Br__eak Free")]
    public string Md_ShouldReturnCorrectString_WithTagsInDifferentWords(string text)
    {
        return Md.Render(text);
    }

    [TestCase("A K_in_d of Magic", ExpectedResult = "A K<em>in</em>d of Magic")]
    [TestCase("A _Kin_d of Magic", ExpectedResult = "A <em>Kin</em>d of Magic")]
    [TestCase("A Kin_d_ of Magic", ExpectedResult = "A Kin<em>d</em> of Magic")]
    [TestCase("A K__in__d of Magic", ExpectedResult = "A K<strong>in</strong>d of Magic")]
    [TestCase("A __Kin__d of Magic", ExpectedResult = "A <strong>Kin</strong>d of Magic")]
    [TestCase("A Kin__d__ of Magic", ExpectedResult = "A Kin<strong>d</strong> of Magic")]
    public string Md_ShouldReturnCorrectString_WithTagsInSameWords(string text)
    {
        return Md.Render(text);
    }

    [TestCase("[Let's listen it](https://www.youtube.com/watch?v=-tJYN-eG1zk)",
        ExpectedResult = "<a href=\"https://www.youtube.com/watch?v=-tJYN-eG1zk\">Let's listen it</a>")]
    [TestCase("[Queen](https://en.wikipedia.org/wiki/Queen_(band))",
        ExpectedResult = "<a href=\"https://en.wikipedia.org/wiki/Queen_(band)\">Queen</a>")]
    public string Md_ShouldReturnCorrectString_WithLink(string text)
    {
        return Md.Render(text);
    }

    [Test]
    public void Md_ShouldEnd_WithLinearTime()
    {
        const string markdown = "# __Princes _of the_ Universe__";
        var markdownSb = new StringBuilder();
        for (int i = 0; i < 100000; i++)
        {
            markdownSb.Append(markdown);
        }

        var renderMd = () => { Md.Render(markdownSb.ToString()); };

        renderMd.ExecutionTime().Should().BeLessThan(10.Seconds());
    }
}