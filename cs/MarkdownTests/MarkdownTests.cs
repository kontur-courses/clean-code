using NUnit.Framework.Internal;

namespace MarkdownTests;

[TestFixture]
public class MarkdownTests
{
    [TestCase("Nikita", "Nikita", TestName = "Text no have tags")]
    [TestCase("_Nikita_", @"<em>Nikita</em>", TestName = "Cursive")]
    [TestCase("_Nikita_ __go__ in _home_", @"<em>Nikita</em> <strong>go</strong> in <em>home</em>", TestName = "Cursive and Bold")]
    [TestCase("#Report _my live_ in __Ekaterinburg__", @"<h1>Report <em>my live</em> in <strong>Ekaterinburg</strong></h1>", TestName = "Headline and Cursive and Bold")]
    public void Render_ShouldHeadlineCursiveBold_WhenCorrectWrite(string makrdown, string html)
    {
        Markdown.Md.Render(makrdown)
            .Should().Be(html);

    }
}