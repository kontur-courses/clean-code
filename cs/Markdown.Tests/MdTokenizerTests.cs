using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown.Tests;
[TestFixture]
public class MdTokenizerTests
{
    private static IEnumerable<TestCaseData> MdTokenizeCases()
    {
        yield return new TestCaseData(null, new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("StringIsNull");
        
        yield return new TestCaseData(string.Empty, new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("EmptyString");
        
        yield return new TestCaseData("SimpleText", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.Word, "SimpleText"), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyText");
        
        yield return new TestCaseData("52", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.Number, "52"), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyNumber");
        
        yield return new TestCaseData(" ", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.Whitespace, " "), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyWhitespace");
        
        yield return new TestCaseData("_", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph), 
                new Token(TypeOfToken.Underscore, "_"), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyUnderscore");
        
        yield return new TestCaseData("#", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Hash, "#"), 
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyHash");
        
        yield return new TestCaseData("\n", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.EndOfParagraph),
                new Token(TypeOfToken.Newline, "\n"),
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("OnlyNewline");
        
        yield return new TestCaseData("\r\n", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.EndOfParagraph),
                new Token(TypeOfToken.Newline, "\r\n"),
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("NewlineAndCarriageReturn");
        
        yield return new TestCaseData("$%^&", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "$%^&"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("DifferentCharacters");
        
        yield return new TestCaseData("#World 52 _Hello_", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Hash, "#"),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Number, "52"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("MixedTokens");
        
        yield return new TestCaseData("Hello   World", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("MultipleWhitespaces");
        
        yield return new TestCaseData("Hello\nWorld", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.EndOfParagraph),
                new Token(TypeOfToken.Newline, "\n"),
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("TextWithSeveralParagraphs");
        
        yield return new TestCaseData("# Test\nTest", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Hash, "#"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "Test"),
                new Token(TypeOfToken.EndOfParagraph),
                new Token(TypeOfToken.Newline, "\n"),
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "Test"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("HeadingAndParagraph");
        
        yield return new TestCaseData(@"\_Escaped\_", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Word, "Escaped"),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("EscapedEmphasis");
        
        yield return new TestCaseData(@"\__Strong\__", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.Word, "Strong"),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("EscapedStrong");

        yield return new TestCaseData(@"\# Heading", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "#", 2),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "Heading"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("EscapedHash");

        yield return new TestCaseData(@"Hello\ World", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, @"Hello\"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("EscapedWhitespace");

        yield return new TestCaseData(@"\\\\", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Word, "\\", 2),
                new Token(TypeOfToken.Word, "\\", 2),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("DoubleEscape");
        
        yield return new TestCaseData("-+*", new List<Token>
            {
                new Token(TypeOfToken.StartOfParagraph),
                new Token(TypeOfToken.Bullet, "-", 1),
                new Token(TypeOfToken.Bullet, "+", 1),
                new Token(TypeOfToken.Bullet, "*", 1),
                new Token(TypeOfToken.EndOfParagraph)
            })
            .SetName("Bullets");
    }
    
    [TestCaseSource(nameof(MdTokenizeCases))]
    public void Tokenize_CorrectlySplitsTestIntoTokens_When(string markdown, List<Token> expectedTokens)
    {
        var mdTokenizer = new MdTokenizer();
        
        var tokens = mdTokenizer.Tokenize(markdown);

        tokens.Should().BeEquivalentTo(expectedTokens, option => option.AllowingInfiniteRecursion());
    }
    
    [Test]
    public void Tokenize_CorrectlySplitsLongText()
    {
        var mdTokenizer = new MdTokenizer();
        var longText = string.Concat(Enumerable.Repeat("_eee_ ", 10000));
    
        var tokens = mdTokenizer.Tokenize(longText);
    
        tokens.Count().Should().Be(40002);
    }
}