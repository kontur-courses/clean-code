using FluentAssertions;

namespace Tests.BlockConvertingTests;

public class ParagraphConvertingTests : MarkupConverterTestBase
{
    [Test]
    public void Convert_WhenSingleLineParagraph_ShouldCreateSingleParagraph()
    {
        var markdown = "aaa";
        var expectedHtml = "<p>aaa</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenTwoParagraphsSeparatedByBlankLine_ShouldCreateTwoParagraphs()
    {
        var markdown = $"aaa{Environment.NewLine}{Environment.NewLine}bbb";
        var expectedHtml = $"<p>aaa</p>{Environment.NewLine}<p>bbb</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenMultiLineParagraphWithoutBlankLines_ShouldCreateSingleParagraph()
    {
        var markdown = $"aaa{Environment.NewLine}bbb{Environment.NewLine}ccc{Environment.NewLine}ddd";
        var expectedHtml = $"<p>aaa bbb ccc ddd</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenMultipleBlankLinesBetweenParagraphs_ShouldCreateTwoParagraphs()
    {
        var markdown = $"aaa{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}bbb";
        var expectedHtml = $"<p>aaa</p>{Environment.NewLine}<p>bbb</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenBlankLinesAtStartAndEnd_ShouldIgnoreBlankLines()
    {
        var markdown = $" {Environment.NewLine}{Environment.NewLine}aaa" +
                       $"{Environment.NewLine}{Environment.NewLine}" +
                       $"bbb{Environment.NewLine}{Environment.NewLine}";
        
        var expectedHtml = $"<p>aaa</p>{Environment.NewLine}<p>bbb</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Convert_WhenParagraphHasTrailingSpacesAndLeadingSpaces_ShouldTrimSpaces()
    {
        var markdown = "   aaa   ";
        var expectedHtml = "<p>aaa</p>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }

    [Test]
    public void Convert_WhenHeaderAfterParagraph_ShouldCloseParagraph()
    {
        var markdown = $"aaa{Environment.NewLine}# bbb";
        var expectedHtml = $"<p>aaa</p>{Environment.NewLine}<h1>bbb</h1>";
        
        var actualHtml = Converter.Convert(markdown);
        
        actualHtml.Should().Be(expectedHtml);
    }
}