using FluentAssertions;

namespace Tests.BlockConvertingTests;

public class HeaderConvertingTests : MarkupConverterTestBase
{
    [TestCase("# header", "<h1>header</h1>", TestName = "One hash")]
    [TestCase("## header", "<h2>header</h2>", TestName = "Two hashes")]
    [TestCase("### header", "<h3>header</h3>", TestName = "Three hashes")]
    [TestCase("#### header", "<h4>header</h4>", TestName = "Four hashes")]
    [TestCase("##### header", "<h5>header</h5>", TestName = "Five hashes")]
    [TestCase("###### header", "<h6>header</h6>", TestName = "Six hashes")]
    public void Convert_WhenOneToSixHashes_ShouldCreateHtmlHeading(string markdown, string expectedHtml)
    {
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenMoreThanSixHashes_ShouldCreateParagraph()
    {
        var markdown = "####### header";
        var expectedHtml = "<p>####### header</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenHasIsNotFollowedBySpace_ShouldCreateParagraph()
    {
        var markdown = "#header";
        var expectedHtml = "<p>#header</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [TestCase("#    header    ", TestName = "Leading and trailing spaces after hash")]
    [TestCase("    # header", TestName = "Leading spaces before hash")]
    [TestCase("    #     header   ", TestName = "Leading and trailing spaces before and after hash")]
    public void Convert_WhenHeadingHasLeadingAndTrailingSpaces_ShouldTrimSpaces(string markdown)
    {
        var expectedHtml = "<h1>header</h1>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenEmptyHeading_ShouldCreateEmptyHtmlHeading()
    {
        var markdown = "# ";
        var expectedHtml = "<h1></h1>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenHashesAfterOpenHash_ShouldNotCreateAnotherHtmlHeading()
    {
        var markdown = "# header # another header";
        var expectedHtml = "<h1>header # another header</h1>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_ManyHeadings_ShouldCreateManyHtmlHeading()
    {
        var markdown = $"# header{Environment.NewLine}## another header";
        var expectedHtml = $"<h1>header</h1>{Environment.NewLine}<h2>another header</h2>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
}