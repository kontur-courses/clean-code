using FluentAssertions;

namespace Markdown.Tests;

[TestFixture]
public class TokenizerTests
{
    private Tokenizer tokenizer;

    [SetUp]
    public void Setup()
    {
        tokenizer = new Tokenizer();
    }

    [Test]
    public void Tokenize_ShouldReturnTextToken_WhenPlainText()
    {
        var text = "plain text";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Select(t => new { t.Type, t.Value }).Should().Contain(
            new { Type = TokenType.Text, Value = "plain text" }
        );
    }

    [Test]
    public void Tokenize_ShouldReturnHeadingToken_WhenOctothorpe()
    {
        var text = "# Heading";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.Heading && t.Value == "#");
    }

    [Test]
    public void Tokenize_ShouldReturnItalicTokens_WhenSingleUnderscores()
    {
        var text = "_italic_";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Select(t => new { t.Type, t.Value }).Should().Equal(
            new { Type = TokenType.ItalicStart, Value = "_" },
            new { Type = TokenType.Text, Value = "italic" },
            new { Type = TokenType.ItalicEnd, Value = "_" },
            new { Type = TokenType.EndOfFile, Value = "" }
        );
    }

    [Test]
    public void Tokenize_ShouldReturnBoldTokens_WhenDoubleUnderscores()
    {
        var text = "__bold__";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Select(t => new { t.Type, t.Value }).Should().Equal(
            new { Type = TokenType.BoldStart, Value = "__" },
            new { Type = TokenType.Text, Value = "bold" },
            new { Type = TokenType.BoldEnd, Value = "__" },
            new { Type = TokenType.EndOfFile, Value = "" }
        );
    }

    [Test]
    public void Tokenize_ShouldReturnListItemToken_WhenDashWithSpace()
    {
        var text = "- item";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.ListItem);
    }

    [Test]
    public void Tokenize_ShouldReturnListItemToken_WhenAsteriskWithSpace()
    {
        var text = "* item";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.ListItem);
    }

    [Test]
    public void Tokenize_ShouldReturnListItemToken_WhenPlusWithSpace()
    {
        var text = "+ item";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.ListItem);
    }

    [Test]
    public void Tokenize_ShouldNotReturnListItemToken_WhenNoSpaceAfterMarker()
    {
        var text = "-item";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().NotContain(t => t.Type == TokenType.ListItem);
        tokens.Should().Contain(t => t.Type == TokenType.Text && t.Value == "-item");
    }

    [Test]
    public void Tokenize_ShouldNotReturnListItemToken_WhenMarkerInMiddle()
    {
        var text = "text - item";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().NotContain(t => t.Type == TokenType.ListItem);
    }

    [Test]
    public void Tokenize_ShouldParseMultipleListItems()
    {
        var text = "- item1\n- item2\n- item3";
        
        var tokens = tokenizer.Tokenize(text);
        
        var listItemTokens = tokens.Where(t => t.Type == TokenType.ListItem).ToList();
        listItemTokens.Should().HaveCount(3);
    }

    [Test]
    public void Tokenize_ShouldParseListItemWithFormatting()
    {
        var text = "- _italic_ and __bold__";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Select(t => new { t.Type, t.Value }).Should().ContainInOrder(
            new { Type = TokenType.ListItem, Value = "-" },
            new { Type = TokenType.ItalicStart, Value = "_" },
            new { Type = TokenType.Text, Value = "italic" },
            new { Type = TokenType.ItalicEnd, Value = "_" },
            new { Type = TokenType.Text, Value = " and " },
            new { Type = TokenType.BoldStart, Value = "__" },
            new { Type = TokenType.Text, Value = "bold" },
            new { Type = TokenType.BoldEnd, Value = "__" }
        );
    }

    [Test]
    public void Tokenize_ShouldReturnNewLineToken_WhenLineBreak()
    {
        var text = "line1\nline2";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Select(t => new { t.Type, t.Value }).Should().Contain(
            new { Type = TokenType.NewLine, Value = "\n" }
        );
    }

    [Test]
    public void Tokenize_ShouldReturnEscapeToken_WhenBackslash()
    {
        var text = "\\_not italic\\_";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.Text && t.Value == "_");
        tokens.Should().NotContain(t => t.Type == TokenType.ItalicStart);
    }

    [Test]
    public void Tokenize_ShouldReturnEndOfFileToken_AtTheEnd()
    {
        var text = "text";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().Contain(t => t.Type == TokenType.EndOfFile);
    }

    [Test]
    public void Tokenize_ShouldConvertUnmatchedTokensToText()
    {
        var text = "__unmatched _italic";
        
        var tokens = tokenizer.Tokenize(text);
        
        tokens.Should().NotContain(t => t.Type == TokenType.BoldStart);
        tokens.Should().NotContain(t => t.Type == TokenType.ItalicStart);
        tokens.Should().Contain(t => t.Type == TokenType.Text && t.Value == "__");
        tokens.Should().Contain(t => t.Type == TokenType.Text && t.Value == "_");
    }

    [Test]
    public void Tokenize_ShouldHandleEmptyInput()
    {
        var text = "";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { Type = TokenType.EndOfFile, Value = "" });
    }

    [Test]
    public void Tokenize_ShouldHandleMixedContent()
    {
        var text = "# Heading\n- item1\n- item2\nplain text";
    
        var tokens = tokenizer.Tokenize(text);
    
        tokens.Should().Contain(t => t.Type == TokenType.Heading);
        tokens.Should().Contain(t => t.Type == TokenType.ListItem);
        tokens.Should().Contain(t => t.Type == TokenType.Text);
    }

    [Test]
    public void Tokenize_ShouldNotParseTripleUnderscore_WhenInvalidContext()
    {
        var text = "___text___";
        
        var tokens = tokenizer.Tokenize(text);
        
        var boldStarts = tokens.Count(t => t.Type == TokenType.BoldStart);
        var boldEnds = tokens.Count(t => t.Type == TokenType.BoldEnd);
        var italicStarts = tokens.Count(t => t.Type == TokenType.ItalicStart);
        var italicEnds = tokens.Count(t => t.Type == TokenType.ItalicEnd);

        boldStarts.Should().Be(boldEnds);
        italicStarts.Should().Be(italicEnds);
    }
}