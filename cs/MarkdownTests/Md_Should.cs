using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class Md_Should
{
    private ParsersHandler _handler;
    private Md _md;
    [SetUp]
    public void Setup()
    {
        _handler = new ParsersHandler();
        _md = new Md(_handler);
    }

    [Test]
    public void WhenNoSpecialSymbols_ReturnsText()
    {
        _md.Render("ABC").Should().Be("ABC");
    }

    [TestCase("_ABC_")]
    [TestCase("_AB_C")]
    [TestCase("A_B_C")]
    [TestCase("AB_C_")]
    public void WhenHasUnderscoreSymbols_ReturnsConverted(string input)
    {
        _md.Render(input).Should().Be(ReplaceUnderscoresWithTag(input));
    }

    private string ReplaceUnderscoresWithTag(string input)
    {
        var opened = false;
        var result = new StringBuilder();
        foreach (var symb in input)
        {
            if (symb.Equals('_'))
            {
                if (!opened)
                {
                    result.Append("<em>");
                    opened = true;
                }
                else
                {
                    result.Append("</em>");
                    opened = false;
                }
            }
            else
                result.Append(symb);
        }

        return result.ToString();
    }
}