using FluentAssertions;
using Markdown.Parsers.MarkdownLineParsers;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownLineParserTests
{
    private readonly MarkdownLineParser markdownLineParser = new MarkdownLineParser();

    [Test]
    public void ParseParagraphForTokens_ShouldDefineHeadingTagToken()
    {
        const string paragraph = "#";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new HeadingTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineBoldTagToken()
    {
        const string paragraph = "__";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new BoldTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineItalicsTagToken()
    {
        const string paragraph = "_";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new ItalicsTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineMarkedListTagToken()
    {
        const string paragraph = "*";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new MarkedListTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineDigitToken()
    {
        const string paragraph = "1";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new DigitToken("1")]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineSpaceToken()
    {
        const string paragraph = " ";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new SpaceToken()]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineEscapeToken()
    {
        const string paragraph = "\\";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new EscapeToken()]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineTextToken()
    {
        const string paragraph = "Abc!";
        var expectedTokens = new List<IToken>
        {
            new TextToken("Abc!")
        };
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo(expectedTokens);
    }

    [Test]
    public void ParseParagraphForTokens_ShouldDefineAllTokens()
    {
        const string paragraph = @"# _Hello_ __world1\__";
        var expectedTokens = new List<IToken>
        {
            new HeadingTagToken(0),
            new SpaceToken(),
            new ItalicsTagToken(2),
            new TextToken("Hello"),
            new ItalicsTagToken(4),
            new SpaceToken(),
            new BoldTagToken(6),
            new TextToken("world"),
            new DigitToken("1"),
            new EscapeToken(),
            new BoldTagToken(10)
        };
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo(expectedTokens);
    }
}