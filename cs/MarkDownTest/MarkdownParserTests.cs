using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Parser.Interface;
using Markdown.Token;
using MarkDownTest.Extension;
using NUnit.Framework;

namespace MarkDownTest;

public class MarkdownParserTests
{
    private IMarkdownParser _parser;

    [SetUp]
    public void Setup()
    {
        _parser = new MarkdownParser();
    }

    [Test]
    public void Parse_PlainText_ReturnsTextToken()
    {
        var input = "Simple text";
        var expectedTokens = new[]
        {
            Token.CreateText("Simple text", 0)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_TextWithSymbols_ReturnsTextToken()
    {
        var input = "Text with symbols !@#$%^&*()";
        var expectedTokens = new[]
        {
            Token.CreateText("Text with symbols !@#$%^&*()", 0)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [TestCase("# Header", 1)]
    [TestCase("## Header", 2)]
    [TestCase("###### Header", 6)]
    public void Parse_Header_ReturnsHeaderAndTextTokens(string input, int expectedLevel)
    {
        var expectedTokens = new[]
        {
            Token.CreateHeader(expectedLevel, 0),
            Token.CreateText("Header", expectedLevel + 1)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [TestCase("#Header")]
    [TestCase("####### Header")]
    [TestCase("Text# Header")]
    public void Parse_InvalidHeader_ReturnsSingleTextToken(string input)
    {
        var expectedTokens = new[]
        {
            Token.CreateText(input, 0)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_StrongEmphasis_ReturnsCorrectTokens()
    {
        var input = "__bold__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold", 2),
            Token.CreateStrong(false, 6)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_ItalicEmphasis_ReturnsCorrectTokens()
    {
        var input = "_italic_";
        var expectedTokens = new[]
        {
            Token.CreateItalic(true, 0),
            Token.CreateText("italic", 1),
            Token.CreateItalic(false, 7)
        };
        
        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_NestedEmphasis_ReturnsCorrectTokens()
    {
        var input = "__bold _italic_ text__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),
            Token.CreateItalic(true, 7),
            Token.CreateText("italic", 8),
            Token.CreateItalic(false, 14),
            Token.CreateText(" text", 15),
            Token.CreateStrong(false, 20)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_NestedTags_ClosesInCorrectOrder()
    {
        var input = "__bold italic__ text_";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold italic", 2),
            Token.CreateStrong(false, 13), 
            Token.CreateText(" text_", 15),
        };
    
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }
    
    [Test]
    public void Parse_ComplexMarkdown_ReturnsCorrectTokenSequence()
    {
        var input = "# Header\n__bold _italic_ text__";
        var expectedTokens = new[]
        {
            Token.CreateHeader(1, 0),
            Token.CreateText("Header\n", 2),
            Token.CreateStrong(true, 9),
            Token.CreateText("bold ", 11),
            Token.CreateItalic(true, 16),
            Token.CreateText("italic", 17),
            Token.CreateItalic(false, 23),
            Token.CreateText(" text", 24),
            Token.CreateStrong(false, 29)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }
    
    [Test]
    public void Parse_NestedTags_HandlesNestedStrongAndItalic()
    {
        var input = "__bold _italic_ bold__";
        var expectedTokens = new[]
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),    
            Token.CreateItalic(true, 7),      
            Token.CreateText("italic", 8),      
            Token.CreateItalic(false, 14),     
            Token.CreateText(" bold", 15),    
            Token.CreateStrong(false, 20),   
        };

        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }
    
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\n")]
    [TestCase(null)]
    public void Parse_MinimalInput_ReturnsTextToken(string input)
    {
        var expectedTokens = new[] { Token.CreateText(input, 0) };
        if (input is null || input.Length == 0)
        {
            expectedTokens = Array.Empty<Token>();
        }
        
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_MixedContent_PreservesWhitespaceInTextTokens()
    {
        var input = "Text  __with  spaces__  here";
        var expectedTokens = new[]
        {
            Token.CreateText("Text  ", 0),
            Token.CreateStrong(true, 6),
            Token.CreateText("with  spaces", 8),
            Token.CreateStrong(false, 20),
            Token.CreateText("  here", 22)
        };
        
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }
    
    [Test]
    public void Parse_Link_ReturnsLinkToken()
    {
        var input = "[пример ссылки](https://example.com)";
        var expectedTokens = new[]
        {
            Token.CreateLink("пример ссылки", "https://example.com", 0)
        };
    
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }
    
    
    [Test]
    public void Parse_StrongTextWithLink_ReturnsCorrectTokens()
    {
        var input = "_Посетите [сайт](https://example.com)_";
        var expectedTokens = new[]
        {
            Token.CreateItalic(true, 0),
            Token.CreateText("Посетите ", 1),
            Token.CreateLink("сайт", "https://example.com", 10),
            Token.CreateItalic(false, 37)
        };
    
        var tokens = _parser.Parse(input);
        
        tokens.AssertTokensEqual(expectedTokens);
    }
    
    [Test]
    public void Parse_WithIncorrectItalicSpacing_ReturnsTextAsIs()
    {
        var input = "_подчерки _не считаются_";
        var expectedTokens = new[]
        {
            Token.CreateText("_подчерки ", 0),
            Token.CreateItalic(true, 10),
            Token.CreateText("не считаются", 11),
            Token.CreateItalic(false, 23)
        };

        var tokens = _parser.Parse(input);

        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithUnderscoresWithinWords_NoFormattingApplied()
    {
        var input = "ра_зных сл_овах";
        var expectedTokens = new[]
        {
            Token.CreateText("ра_зных сл_овах", 0)
        };
        
        var tokens = _parser.Parse(input);
        
   
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithTrailingUnderscoresAfterWords_NoFormattingApplied()
    {
        var input = "эти_ подчерки_ не должны работать";
        var expectedTokens = new[]
        {
            Token.CreateText("эти_ подчерки_ не должны работать", 0)
        };
        
        var tokens = _parser.Parse(input);
        
       
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithUnpairedUnderscore_ReturnsTextAsIs()
    {
        var input = "Непарные_ символы";
        var expectedTokens = new[]
        {
            Token.CreateText("Непарные_ символы", 0)
        };
        
        var tokens = _parser.Parse(input);
        
       
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithUnderscoresAroundNumbers_NoFormattingApplied()
    {
        var input = "цифрами_12_3";
        var expectedTokens = new[]
        {
            Token.CreateText("цифрами_12_3", 0)
        };
        
        var tokens = _parser.Parse(input);
        
       
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithMultipleConsecutiveUnderscores_ReturnsTextAsIs()
    {
        var input = "____";
        var expectedTokens = new[]
        {
            Token.CreateText("____", 0)
        };
        
        var tokens = _parser.Parse(input);
        

        tokens.AssertTokensEqual(expectedTokens);
    }
    
    
    [Test]
    public void Parse_WithEscapedCharacters_ReturnsTextWithEscapedSymbols()
    {
        var input = @"\_не_подчеркивается\_";
        var expectedTokens = new[]
        {
            Token.CreateText("_", 0),
            Token.CreateText("не_подчеркивается", 2),
            Token.CreateText("_", 19),
        };
    
        var tokens = _parser.Parse(input);
    
        
        tokens.AssertTokensEqual(expectedTokens);
    }

    [Test]
    public void Parse_WithLineBreaksBreakingControlCharacters_ReturnsTextAsIs()
    {
        var input = "Это пример с разрывом _подчеркивания\nна новой строке_";
        var expectedTokens = new[]
        {
            Token.CreateText("Это пример с разрывом _подчеркивания\nна новой строке_", 0)
        };
    
        var tokens = _parser.Parse(input);
    
 
        tokens.AssertTokensEqual(expectedTokens);
    }
}