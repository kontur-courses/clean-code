using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerTests;

[TestFixture]
public class TokenizeUnderscoresTests
{
    [TestCase("_текст_")]
    public void Tokenize_SimpleItalic_ReturnsCorrectTokens(string markdownText)
    {
        var expectedToken = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedToken);
    }

    [TestCase("__текст__")]
    public void Tokenize_SimpleBold_ReturnsCorrectTokens(string markdownText)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expected);
    }

    [TestCase("__начало _внутри_ конец__")]
    public void Tokenize_BoldWithItalicInside(string markdownText)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("начало", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("внутри", TokenType.Text),
            new("_", TokenType.Underscore),
            new(" ", TokenType.Space),
            new("конец", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expected);
    }

    [TestCase("_те\nкст_")]
    public void Tokenize_ItalicWithNewLineInside(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("те", TokenType.Text),
            new("\n", TokenType.NewLine),
            new("кст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст__")]
    public void Tokenize_SimpleBold(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_те_кст")]
    public void Tokenize_ItalicAtStartWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("т_екс_т")]
    public void Tokenize_ItalicInMiddleWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("т", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("екс", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("т", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("текс_т._")]
    public void Tokenize_ItalicAtEndWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("текс", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("т.", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст_")]
    public void Tokenize_UnpairedUnderscores(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_ текст_")]
    public void Tokenize_LeadingSpacePreventsItalic(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("1_2_3")]
    public void Tokenize_DigitsPreventItalic(string markdown)
    {
        var expected = new List<Token>
        {
            new("1", TokenType.Text),
            new("_", TokenType.Text),
            new("2", TokenType.Text),
            new("_", TokenType.Text),
            new("3", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("те_кст те_кст")]
    public void Tokenize_SeparateWordsUnderscores(string markdown)
    {
        var expected = new List<Token>
        {
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new(" ", TokenType.Space),
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_текст _текст")]
    public void Tokenize_UnclosedItalicMultipleWords(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__")]
    public void Tokenize_OnlyDoubleUnderscore(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("____")]
    public void Tokenize_OnlyFourUnderscores(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст _текст__ текст_")]
    public void Tokenize_BoldWithNestedItalic(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_текст __текст__ текст_")]
    public void Tokenize_ItalicWithBoldInside(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_текст т__екс__т текст_")]
    public void Tokenize_ItalicWithBoldInsideWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("т", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("екс", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("т", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст т_екст текст___")]
    public void Tokenize_BoldWithTrailingUnderscore(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("т", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("екст", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("т__е_к_с__т")]
    public void Tokenize_BoldInWordWithItalicInside(string markdown)
    {
        var expected = new List<Token>
        {
            new("т", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("е", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("к", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("с", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("т", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__те_к_ст__")]
    public void Tokenize_BoldWithItalicInsideInWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("к", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("ст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_те__к__ст_")]
    public void Tokenize_ItalicWithBoldInsideInWord(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("те", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("к", TokenType.Text),
            new("__", TokenType.WordDoubleUnderscore),
            new("ст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("_123_")]
    public void Tokenize_ItalicWithNumbers(string markdown)
    {
        var expected = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("123", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__123__")]
    public void Tokenize_BoldWithNumbers(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("123", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__++++__")]
    public void Tokenize_BoldWithSymbols(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("++++", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст___текст_")]
    public void Tokenize_BoldThenItalic(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__ текст __")]
    public void Tokenize_BoldWithSpacesPrevents(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("__", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("__текст __")]
    public void Tokenize_BoldAfterSpacesPrevents(string markdown)
    {
        var expected = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("__", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }
}