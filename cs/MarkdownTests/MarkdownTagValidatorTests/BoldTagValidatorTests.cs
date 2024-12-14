using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

[TestFixture]
public class BoldTagValidatorTests
{
    private readonly MarkdownTagValidator markdownTagValidator = new();
    
    [TestCaseSource(nameof(TestCases))]
    public bool IsValid_ShouldValidateBoldTag(TestCase testCase)
    {
        return markdownTagValidator.IsValidPairedTag(
            testCase.Tokens,
            testCase.PairedMarkdownTags,
            testCase.ExternalTagType);
    }
    
    private static readonly IEnumerable<TestCaseData> TestCases = new[]
    {
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new BoldTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello!"),
                        new BoldTagToken(3),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(3))
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
                        new BoldTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello!"),
                        new BoldTagToken(3),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(3))
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
                        new BoldTagToken(1),
                        new TextToken("lo,"),
                        new SpaceToken(),
                        new TextToken("wo"),
                        new BoldTagToken(5),
                        new TextToken("rld!"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(5))
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
                        new BoldTagToken(0),
                        new TextToken("Hello,"),
                        new SpaceToken(),
                        new TextToken("wo"),
                        new BoldTagToken(4),
                        new TextToken("rld!"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(5))
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
                        new BoldTagToken(1),
                        new TextToken("lo,"),
                        new SpaceToken(),
                        new TextToken("world!"),
                        new BoldTagToken(5),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(5))
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
                        new BoldTagToken(2),
                        new DigitToken("3"),
                        new BoldTagToken(4),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(2), 
                        new BoldTagToken(4))
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
                        new BoldTagToken(0),
                        new DigitToken("1"),
                        new DigitToken("2"),
                        new DigitToken("3"),
                        new BoldTagToken(4),
                        new DigitToken("4"),
                        new DigitToken("5"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(4))
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
                        new BoldTagToken(1),
                        new DigitToken("2"),
                        new TextToken("."),
                        new DigitToken("3"),
                        new BoldTagToken(5),
                        new DigitToken("4"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(5))
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
                        new BoldTagToken(1),
                        new DigitToken("2"),
                        new DigitToken("3"),
                        new BoldTagToken(4),
                        new DigitToken("4"),
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(4))
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
                        new BoldTagToken(1),
                        new TextToken("Hello"),
                        new BoldTagToken(3)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(3))
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
                        new BoldTagToken(0),
                        new TextToken("Hello"),
                        new EscapeToken(),
                        new BoldTagToken(3)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(3))
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
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new BoldTagToken(2),
                        new BoldTagToken(3),
                        new SpaceToken(),
                        new TextToken("world!")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(2), 
                        new BoldTagToken(3))
                }
            )
            .Returns(false)
            .SetName("12. Невалидный тег. Между тегами пустая строка. Строка: 'Hello, ____ world!'"),
        
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello,"),
                        new SpaceToken(),
                        new BoldTagToken(3),
                        new TextToken("big"),
                        new BoldTagToken(5),
                        new SpaceToken(),
                        new TextToken("world"),
                        new ItalicsTagToken(8)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                            new BoldTagToken(3), 
                            new BoldTagToken(5)),
                    ExternalTagType = MarkdownTagType.Italics
                }
            )
            .Returns(false)
            .SetName("13. Невалидный тег. Теги внутри тегов курсива. Строка: '_Hello, __big__ world_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("Hello,"),
                        new SpaceToken(),
                        new TextToken("world!"),
                        new BoldTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(4))
                }
            )
            .Returns(true)
            .SetName("14. Валидный тег. Текст между тегами. Строка: '_Hello, world!_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("He"),
                        new BoldTagToken(2),
                        new TextToken("llo,"),
                        new SpaceToken(),
                        new TextToken("world!")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(2))
                }
            )
            .Returns(true)
            .SetName("15. Валидный тег. Теги в одном слове. Строка: '_He_llo, world!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("Ma"),
                        new BoldTagToken(1),
                        new TextToken("rkd"),
                        new BoldTagToken(3),
                        new TextToken("own")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(3))
                }
            )
            .Returns(true)
            .SetName("16. Валидный тег. Теги в одном слове. Строка: 'Ma_rkd_own'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new TextToken("He"),
                        new BoldTagToken(1),
                        new TextToken("llo,"),
                        new BoldTagToken(3),
                        new SpaceToken(),
                        new TextToken("world!")
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(1), 
                        new BoldTagToken(3))
                }
            )
            .Returns(true)
            .SetName("17. Валидный тег. Теги в одном слове. Строка: 'He_llo,_ world!'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new EscapeToken(),
                        new EscapeToken(),
                        new BoldTagToken(2),
                        new TextToken("Hello"),
                        new BoldTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(2), 
                        new BoldTagToken(4))
                }
            )
            .Returns(true)
            .SetName(@"18. Валидный тег. Символ экранирования экранирован. Строка: '\\_Hello_'"),
        
        new TestCaseData
            (
                new TestCase
                {
                    Tokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("Hello"),
                        new EscapeToken(),
                        new EscapeToken(),
                        new BoldTagToken(4)
                    ],
                    PairedMarkdownTags = new PairedMarkdownTagTokens(
                        MarkdownTagType.Bold,
                        new BoldTagToken(0), 
                        new BoldTagToken(4))
                }
            )
            .Returns(true)
            .SetName(@"19. Валидный тег. Символ экранирования экранирован. Строка: '_Hello\\_'"),
    };

    public record TestCase
    {
        public required List<IToken> Tokens { get; init; }
        
        public required IPairedMarkdownTagTokens PairedMarkdownTags { get; init; }

        public MarkdownTagType? ExternalTagType { get; init; }
    }
}