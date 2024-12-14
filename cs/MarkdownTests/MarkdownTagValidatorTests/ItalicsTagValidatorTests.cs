using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

public class ItalicsTagValidatorTests
{
    private readonly MarkdownTagValidator markdownTagValidator = new();
    
    [TestCaseSource(nameof(TestCases))]
    public bool IsValid_ShouldValidateItalicsTag(TestCase testCase)
    {
        return markdownTagValidator.IsValidPairedTag(
            testCase.Tokens, 
            testCase.PairedMarkdownTags);
    }
    
    private static readonly IEnumerable<TestCaseData> TestCases = new[]
    {
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello!"),
                        new ItalicsTagToken(3),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(false)
            .SetName("01. Невалидный тег. Пробел перед открывающим тегом. Строка: '_ Hello!_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello!"),
                        new ItalicsTagToken(3),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(false)
            .SetName("02. Невалидный тег. Пробел перед закрывающим тегом. Строка: '_Hello! _'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("Hel"),
                        new ItalicsTagToken(1),
                        new TextToken("lo,"),
                        new SpaceToken(),
                        new TextToken("wo"),
                        new ItalicsTagToken(5),
                        new TextToken("rld!"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(5))
                }
            )
            .Returns(false)
            .SetName("03. Невалидный тег. Теги в разных словах. Строка: 'Hel_lo, wo_rld!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello,"),
                        new SpaceToken(),
                        new TextToken("wo"),
                        new ItalicsTagToken(4),
                        new TextToken("rld!"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(false)
            .SetName("04. Невалидный тег. Теги в разных словах. Строка: '_Hello, wo_rld!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("Hel"),
                        new ItalicsTagToken(1),
                        new TextToken("lo,"),
                        new SpaceToken(),
                        new TextToken("world!"),
                        new ItalicsTagToken(5),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(5))
                }
            )
            .Returns(false)
            .SetName("05. Невалидный тег. Теги в разных словах. Строка: 'He_llo, world!_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new DigitToken("1"),
                        new DigitToken("2"),
                        new ItalicsTagToken(2),
                        new DigitToken("3"),
                        new ItalicsTagToken(4),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(2),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(false)
            .SetName("06. Невалидный тег. Теги внутри текста c цифрами. Строка: '12_3_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new DigitToken("1"),
                        new DigitToken("2"),
                        new DigitToken("3"),
                        new ItalicsTagToken(4),
                        new DigitToken("4"),
                        new DigitToken("5"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(false)
            .SetName("07. Невалидный тег. Теги внутри текста c цифрами. Строка: '_123_45'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new DigitToken("1"),
                        new ItalicsTagToken(1),
                        new DigitToken("2"),
                        new TextToken("."),
                        new DigitToken("3"),
                        new ItalicsTagToken(5),
                        new DigitToken("4"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(5))
                }
            )
            .Returns(false)
            .SetName("08. Невалидный тег. Теги внутри текста c цифрами. Строка: '1_2.3_4'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("Hello"),
                        new ItalicsTagToken(1),
                        new DigitToken("2"),
                        new DigitToken("3"),
                        new ItalicsTagToken(4),
                        new DigitToken("4"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(false)
            .SetName("09. Невалидный тег. Теги внутри текста c цифрами. Строка: 'Hello_23_4'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new EscapeToken(),
                        new ItalicsTagToken(1),
                        new TextToken("Hello"),
                        new ItalicsTagToken(3)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(false)
            .SetName(@"10. Невалидный тег. Открывающий тег экранирован. Строка: '\_Hello_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello"),
                        new EscapeToken(),
                        new ItalicsTagToken(3)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(false)
            .SetName(@"11. Невалидный тег. Закрывающий тег экранирован. Строка: '_Hello\_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello,"),
                        new SpaceToken(),
                        new TextToken("world!"),
                        new ItalicsTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(true)
            .SetName("12. Валидный тег. Текст между тегами. Строка: '_Hello, world!_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("He"),
                        new ItalicsTagToken(2),
                        new TextToken("llo,"),
                        new SpaceToken(),
                        new TextToken("world!")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(2))
                }
            )
            .Returns(true)
            .SetName("13. Валидный тег. Теги в одном слове. Строка: '_He_llo, world!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("Ma"),
                        new ItalicsTagToken(1),
                        new TextToken("rkd"),
                        new ItalicsTagToken(3),
                        new TextToken("own")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(true)
            .SetName("14. Валидный тег. Теги в одном слове. Строка: 'Ma_rkd_own'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("He"),
                        new ItalicsTagToken(1),
                        new TextToken("llo,"),
                        new ItalicsTagToken(3),
                        new SpaceToken(),
                        new TextToken("world!")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(1),
                        new ItalicsTagToken(3))
                }
            )
            .Returns(true)
            .SetName("15. Валидный тег. Теги в одном слове. Строка: 'He_llo,_ world!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new EscapeToken(),
                        new EscapeToken(),
                        new ItalicsTagToken(2),
                        new TextToken("Hello"),
                        new ItalicsTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(2),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(true)
            .SetName(@"16. Валидный тег. Символ экранирования экранирован. Строка: '\\_Hello_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello"),
                        new EscapeToken(),
                        new EscapeToken(),
                        new ItalicsTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Italics,
                        new ItalicsTagToken(0),
                        new ItalicsTagToken(4))
                }
            )
            .Returns(true)
            .SetName(@"17. Валидный тег. Символ экранирования экранирован. Строка: '_Hello\\_'"),
    };

    public record TestCase
    {
        public required List<IToken> Tokens { get; init; }
        
        public required IPairedMarkdownTagTokens PairedMarkdownTags { get; init; }
    }
}