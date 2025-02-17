using FluentAssertions;
using Markdown.Interfaces;

namespace Markdown.Tests;

//todo: текст header, если на второй строчки

public class MdParserTests
{
    private ILexer lexer;

    [SetUp]
    public void Setup()
    {
        lexer = new MdParser();
    }

    [Test]
    public void MdParser_ParseTag_SimpleHeader() // Тест на простой заголовок
    {
        var tokens = lexer.Tokenize("# hello");

        tokens.Should().BeEquivalentTo([
            new Token("# ", TokenType.Header) { StartIndex = 0, IsTag = true },
            new Token("hello", TokenType.Word, 2)
        ]);
    }


    [Test]
    public void MdParser_ParseTag_HeaderWithTextWithNewLine() // Тест на заголовок с текстом и переносом строки
    {
        var tokens = lexer.Tokenize("# hello\n");

        tokens.Should().BeEquivalentTo([
            new Token("# ", TokenType.Header) { IsTag = true },
            new Token("hello", TokenType.Word, 2),
            new Token("\n", TokenType.NewLine, 7) { IsTag = true }
        ]);
    }


    [Test]
    public void
        MdParser_ParseTag_HeaderWithTextWithNewLineAndText() // Тест на заголовок с текстом, переносом строки и последующим текстом
    {
        var tokens = lexer.Tokenize("# hello\n hi");

        tokens.Should().BeEquivalentTo([
            new Token("# ", TokenType.Header, 0) { IsTag = true },
            new Token("hello", TokenType.Word, 2),
            new Token("\n", TokenType.NewLine, 7) { IsTag = true },
            new Token(" ", TokenType.WhiteSpace, 8),
            new Token("hi", TokenType.Word, 9) { IsTag = false }
        ]);
    }


    [Test]
    public void MdParser_ParseTag_BoldText()
    {
        var text = "__bold text__";

        var tokens = lexer.Tokenize(text);


        tokens.Should().BeEquivalentTo([
            new Token("__", TokenType.Bold) { StartIndex = 0, IsTag = true },
            new Token("bold", TokenType.Word, 2),
            new Token(" ", TokenType.WhiteSpace, 6),
            new Token("text", TokenType.Word, 7),
            new Token("__", TokenType.Bold) { StartIndex = 11, IsTag = true }
        ]);
    }

    [Test]
    public void MdParser_ParseTag_DigitWithItalic() // 3 условие спецификаций
    {
        var text = "_1234_";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("_", TokenType.Italic) { StartIndex = 0, IsTag = true },
            new Token("1234", TokenType.Digit, 1),
            new Token("_", TokenType.Italic) { StartIndex = 5, IsTag = true }
        ]);
    }

    [Test]
    public void MdParser_ParseTag_ItalicInWord()
    {
        var text = "ou_ter_wild";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("ou", TokenType.Word, 0),
            new Token("_", TokenType.Italic) { StartIndex = 2, IsTag = true },
            new Token("ter", TokenType.Word, 3),
            new Token("_", TokenType.Italic) { StartIndex = 6, IsTag = true },
            new Token("wild", TokenType.Word, 7)
        ]);
    }


    [Test]
    public void MdParser_SingleItalicTag_ShouldBe_Text()
    {
        var text = "he_llo";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("he", TokenType.Word, 0),
            new Token("_", TokenType.Italic) { StartIndex = 2, IsTag = false },
            new Token("llo", TokenType.Word, 3)
        ]);
    }

    [Test]
    public void MdParser_ParseTag_ItalicText()
    {
        var text = "_italic text_";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("_", TokenType.Italic) { StartIndex = 0, IsTag = true },
            new Token("italic", TokenType.Word, 1),
            new Token(" ", TokenType.WhiteSpace, 7),
            new Token("text", TokenType.Word, 8),
            new Token("_", TokenType.Italic) { StartIndex = 12, IsTag = true }
        ]);
    }

    [Test]
    public void MdParser_ParseTag_Marker()
    {
        var text = "* apple\n* pineApple";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token(" ", TokenType.MarkerRange) { IsTag = true },
            new Token("* ", TokenType.Marker) { IsTag = true },
            new Token("apple", TokenType.Word, 2),
            new Token("\n", TokenType.NewLine, 7) { IsTag = true },
            new Token("* ", TokenType.Marker, 8) { IsTag = true },
            new Token("pineApple", TokenType.Word, 10)
        ]);
    }

    [Test]
    public void MdParser_ParseTag_MarkerRangeCLose()
    {
        var text = "* apple\n* pineApple\n ";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token(" ", TokenType.MarkerRange) { IsTag = true },
            new Token("* ", TokenType.Marker) { IsTag = true },
            new Token("apple", TokenType.Word, 2),
            new Token("\n", TokenType.NewLine, 7) { IsTag = true },
            new Token("* ", TokenType.Marker, 8) { IsTag = true },
            new Token("pineApple", TokenType.Word, 10),
            new Token("\n", TokenType.NewLine, 19) { IsTag = true },
            new Token(" ", TokenType.MarkerRange, 19) {IsTag = true},
            new Token(" ", TokenType.WhiteSpace, 20)
        ]);
    }


    [Test]
    public void MdParser_ParseTags_ItalicInsideBold()
    {
        var text = "__bold _italic_ text__";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("__", TokenType.Bold) { StartIndex = 0, IsTag = true },
            new Token("bold", TokenType.Word, 2),
            new Token(" ", TokenType.WhiteSpace, 6),
            new Token("_", TokenType.Italic) { StartIndex = 7, IsTag = true },
            new Token("italic", TokenType.Word, 8),
            new Token("_", TokenType.Italic) { StartIndex = 14, IsTag = true },
            new Token(" ", TokenType.WhiteSpace) { StartIndex = 15 },
            new Token("text", TokenType.Word) { StartIndex = 16 },
            new Token("__", TokenType.Bold) { StartIndex = 20, IsTag = true }
        ]);
    }


    [Test]
    public void MdParser_ParseTags_ItalicInBold_WithComplexText()
    {
        var text = "_hi __bold t_ k__";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("_", TokenType.Italic, 0),
            new Token("hi", TokenType.Word, 1),
            new Token(" ", TokenType.WhiteSpace, 3),
            new Token("__", TokenType.Bold, 4),
            new Token("bold", TokenType.Word, 6),
            new Token(" ", TokenType.WhiteSpace) { StartIndex = 10 },
            new Token("t", TokenType.Word) { StartIndex = 11 },
            new Token("_", TokenType.Italic) { StartIndex = 12 },
            new Token(" ", TokenType.WhiteSpace) { StartIndex = 13 },
            new Token("k", TokenType.Word) { StartIndex = 14 },
            new Token("__", TokenType.Bold) { StartIndex = 15 }
        ]);
    }

    [Test]
    public void MdParser_ParseTags_WhiteSpaces()
    {
        var text = "   ";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("   ", TokenType.WhiteSpace, 0),
        ]);
    }

    [Test]
    public void MdParser_ParseTags_BoldWithStartItalic()
    {
        var text = "__bold_text__";

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("__", TokenType.Bold, 0) { IsTag = true },
            new Token("bold", TokenType.Word, 2),
            new Token("_", TokenType.Italic, 6),
            new Token("text", TokenType.Word, 7),
            new Token("__", TokenType.Bold, 11) { IsTag = true }
        ]);
    }


    [Test]
    public void MdParser_ParseTags_BoldInItalic()
    {
        var text = "_italic __bold__ text_"; // тест проверяет вложенные теги

        var tokens = lexer.Tokenize(text);

        tokens.Should().BeEquivalentTo([
            new Token("_", TokenType.Italic, 0) { IsTag = true },
            new Token("italic", TokenType.Word, 1),
            new Token(" ", TokenType.WhiteSpace, 7),
            new Token("__", TokenType.Bold, 8),
            new Token("bold", TokenType.Word, 10),
            new Token("__", TokenType.Bold, 14),
            new Token(" ", TokenType.WhiteSpace, 16),
            new Token("text", TokenType.Word, 17),
            new Token("_", TokenType.Italic, 21) { IsTag = true }
        ]);
    }
}