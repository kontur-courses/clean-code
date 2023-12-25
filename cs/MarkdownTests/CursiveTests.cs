namespace MarkdownTests;

[TestFixture]
public class CursiveTests
{
    [Test]
    public void Render_ShouldCursive_WhenPairIsFull()
    {
        Markdown.Md.Render("I _go_ to _home")
            .Should().Be(@"I <em>go</em> to _home");
    }

    [Test]
    public void Render_ShouldCursive_WhenUnderlined()
    {
        Markdown.Md.Render("_home_")
            .Should().Be(@"<em>home</em>");
    }

    [Test]
    public void Render_ShouldNotCursive_WhenThereAreDigits()
    {
        Markdown.Md.Render("I _write 1 digit_ in text")
            .Should().Be(@"I _write 1 digit_ in text");
    }


    [Test]
    public void Render_ShouldNotCursive_WhenInDifferentWords()
    {
        Markdown.Md.Render("Hom_e h_ot")
            .Should().Be(@"Hom_e h_ot");
    }

    [Test]
    public void Render_ShouldCursive_WhenInDifferentPartsOfSameWord()
    {
        Markdown.Md.Render("No _so_lid")
            .Should().Be(@"No <em>so</em>lid");
    }
}