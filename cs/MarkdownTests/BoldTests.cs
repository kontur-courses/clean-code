namespace MarkdownTests;

[TestFixture]
public class BoldTests
{
    
    [Test]
    public void Render_ShouldBold_WhenDoubleUnderlined()
    {
        Markdown.Md.Render("Hello __Peter__ ?")
            .Should().Be(@"Hello <strong>Peter</strong> ?");
    }

    [Test]
    public void Render_ShouldBold_WhenPairIsFull()
    {
        Markdown.Md.Render("I __go to home__ when __cold")
            .Should().Be(@"I <strong>go to home</strong> when __cold");
    }

    

    [Test]
    public void Render_ShouldNotBold_WhenThereAreDigits()
    {
        Markdown.Md.Render("I __have 1 bread__")
            .Should().Be(@"I __have 1 bread__");
    }

    [Test]
    public void Render_ShouldNotBold_WhenInDifferentWords()
    {
        Markdown.Md.Render("Me__to ha__ve")
            .Should().Be(@"Me__to ha__ve");
    }

    [Test]
    public void Render_ShouldBold_WhenInDifferentPartsOfSameWord()
    {
        Markdown.Md.Render("I __ha__ve")
            .Should().Be(@"I <strong>ha</strong>ve");
    }
}