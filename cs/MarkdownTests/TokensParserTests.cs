using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Parsers.TokensParsers;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;
using Moq;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class TokensParserTests
{
    private readonly Mock<IMarkdownTagValidator> validatorMock = new();
    private TokensParser parser;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        parser = new TokensParser(validatorMock.Object);
    }
    
    [TestCaseSource(nameof(TestCases))]
    public void TokensParserTest(TestCase testCase, Expected expected)
    {
        CreateValidatorMockForSingleTagToken(testCase.ParagraphOfTokens, testCase.ValidSingleMarkdownTags, true);
        CreateValidatorMockForSingleTagToken(testCase.ParagraphOfTokens, testCase.InvalidSingleMarkdownTags, false);
        CreateValidatorMockForPairedTagTokens(testCase.ParagraphOfTokens, testCase.ValidPairedMarkdownTags, true);
        CreateValidatorMockForPairedTagTokens(testCase.ParagraphOfTokens, testCase.InValidPairedMarkdownTags, false);
    
        var parsedTags = parser.ParserMarkdownTags(testCase.ParagraphOfTokens).ToList();
    
        parsedTags.Should().BeEquivalentTo(expected.Tags);
    }

    private void CreateValidatorMockForSingleTagToken(
        List<IToken> paragraphOfTokens, 
        IEnumerable<ISingleMarkdownTagToken>? tags,
        bool isValidTags)
    {
        if (tags == null)
        {
            return;
        }
        
        foreach (var tag in tags)
        {
            validatorMock
                .Setup(validator => 
                    validator.IsValidSingleTag(paragraphOfTokens, It.Is<ISingleMarkdownTagToken>(
                        singleMarkdownTag => 
                            singleMarkdownTag.Token.PositionInTokens == tag.Token.PositionInTokens && 
                            singleMarkdownTag.Token.Content == tag.Token.Content)))
                .Returns(isValidTags);
        }
    }
    
    private void CreateValidatorMockForPairedTagTokens(
        List<IToken> paragraphOfTokens, 
        IEnumerable<IPairedMarkdownTagTokens>? tags,
        bool isValidTags)
    {
        if (tags == null)
        {
            return;
        }
        
        foreach (var tag in tags)
        {
            validatorMock
                .Setup(validator => 
                    validator.IsValidPairedTag(paragraphOfTokens, It.Is<IPairedMarkdownTagTokens>(
                        pairedMarkdownTagTokens => 
                            pairedMarkdownTagTokens.Opening.PositionInTokens == tag.Opening.PositionInTokens && 
                            pairedMarkdownTagTokens.Opening.Content == tag.Opening.Content && 
                            pairedMarkdownTagTokens.Closing.PositionInTokens == tag.Closing.PositionInTokens && 
                            pairedMarkdownTagTokens.Closing.Content == tag.Closing.Content),
                        It.IsAny<MarkdownTagType?>()))
                .Returns(isValidTags);
        }
    }
    
    public record TestCase
    {
        public required List<IToken> ParagraphOfTokens { get; init; }
    
        public IEnumerable<ISingleMarkdownTagToken>? ValidSingleMarkdownTags { get; init; }
        
        public IEnumerable<ISingleMarkdownTagToken>? InvalidSingleMarkdownTags { get; init; }
        
        public IEnumerable<IPairedMarkdownTagTokens>? ValidPairedMarkdownTags { get; init; }
        
        public IEnumerable<IPairedMarkdownTagTokens>? InValidPairedMarkdownTags { get; init; }
    }

    public record Expected
    {
        public required List<MarkdownTag> Tags { get; init; }
    }

    private static readonly IEnumerable<TestCaseData> TestCases = new[]
    {
        new TestCaseData(
            new TestCase
            {
                ParagraphOfTokens =
                [
                    new HeadingTagToken(0),
                    new SpaceToken(),
                    new TextToken("Hello")
                ],
                ValidSingleMarkdownTags = [new SingleMarkdownTagToken(MarkdownTagType.Heading, new HeadingTagToken(0))],
            },
            new Expected
            {
                Tags = [new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading)]
            })
            .SetName("01. Строка: '# Hello'. В строке единственный валидный тег заголовок."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new HeadingTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new HeadingTagToken(4),
                        new SpaceToken(),
                        new TextToken("world")
                    ],
                    ValidSingleMarkdownTags = [new SingleMarkdownTagToken(MarkdownTagType.Heading, new HeadingTagToken(0))],
                    InvalidSingleMarkdownTags = [new SingleMarkdownTagToken(MarkdownTagType.Heading, new HeadingTagToken(4))]
                },
                new Expected
                {
                    Tags = [new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading)]
                })
            .SetName("02. Строка: '# Hello # world'. В строке два тега заголовка: валидный и невалидный."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello"),
                        new ItalicsTagToken(2),
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Italics, new ItalicsTagToken(0), new ItalicsTagToken(2))
                    ]
                },
                new Expected
                {
                    Tags = 
                    [
                        new MarkdownTag(new ItalicsTagToken(0), MarkdownTagType.Italics),
                        new MarkdownTag(new ItalicsTagToken(2), MarkdownTagType.Italics, true)
                    ]
                })
            .SetName("03. Строка: '_Hello_'. В строке единственная пара валидных тегов курсивов."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new ItalicsTagToken(0),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new ItalicsTagToken(3),
                        new SpaceToken(),
                        new TextToken("world"),
                        new ItalicsTagToken(6)
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Italics, new ItalicsTagToken(0), new ItalicsTagToken(6))
                    ],
                    InValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Italics, new ItalicsTagToken(0), new ItalicsTagToken(3))
                    ]
                },
                new Expected
                {
                    Tags = 
                    [
                        new MarkdownTag(new ItalicsTagToken(0), MarkdownTagType.Italics),
                        new MarkdownTag(new ItalicsTagToken(6), MarkdownTagType.Italics, true)
                    ]
                })
            .SetName("04. Строка: '_Hello _ world_'. В строке три тег-токена курсива, одна пара валидных тегов курсивов."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("Hello"),
                        new BoldTagToken(2),
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Bold, new BoldTagToken(0), new BoldTagToken(2))
                    ]
                },
                new Expected
                {
                    Tags = 
                    [
                        new MarkdownTag(new BoldTagToken(0), MarkdownTagType.Bold),
                        new MarkdownTag(new BoldTagToken(2), MarkdownTagType.Bold, true)
                    ]
                })
            .SetName("05. Строка: '__Hello__'. В строке единственная пара валидных тегов полужирных."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new BoldTagToken(3),
                        new SpaceToken(),
                        new TextToken("world"),
                        new BoldTagToken(6)
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Bold, new BoldTagToken(0), new BoldTagToken(6))
                    ],
                    InValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Bold, new BoldTagToken(0), new BoldTagToken(3))
                    ]
                },
                new Expected
                {
                    Tags = 
                    [
                        new MarkdownTag(new BoldTagToken(0), MarkdownTagType.Bold),
                        new MarkdownTag(new BoldTagToken(6), MarkdownTagType.Bold, true)
                    ]
                })
            .SetName("06. Строка: '__Hello __ world__'. В строке три тег-токена полужирный, одна пара валидных тегов полужирных."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new BoldTagToken(0),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new ItalicsTagToken(3),
                        new TextToken("my"),
                        new BoldTagToken(5),
                        new SpaceToken(),
                        new TextToken("world"),
                        new ItalicsTagToken(8)
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Bold, new BoldTagToken(0), new BoldTagToken(5)),
                        new PairedMarkdownTagTokens(MarkdownTagType.Italics, new ItalicsTagToken(3), new ItalicsTagToken(8))
                    ],
                },
                new Expected
                {
                    Tags = []
                })
            .SetName("07. Строка: '__Hello _my__ world_'. В строке пересекаются валидные пары тегов курсив и полужирный."),
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new HeadingTagToken(0),
                        new SpaceToken(),
                        new BoldTagToken(2),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new ItalicsTagToken(5),
                        new TextToken("my"),
                        new ItalicsTagToken(7),
                        new SpaceToken(),
                        new TextToken("world"),
                        new BoldTagToken(10)
                    ],
                    ValidSingleMarkdownTags = 
                    [
                        new SingleMarkdownTagToken(MarkdownTagType.Heading, new HeadingTagToken(0))
                    ],
                    ValidPairedMarkdownTags = 
                    [
                        new PairedMarkdownTagTokens(MarkdownTagType.Bold, new BoldTagToken(2), new BoldTagToken(10)),
                        new PairedMarkdownTagTokens(MarkdownTagType.Italics, new ItalicsTagToken(5), new ItalicsTagToken(7))
                    ],
                },
                new Expected
                {
                    Tags = 
                    [
                        new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading),
                        new MarkdownTag(new BoldTagToken(2), MarkdownTagType.Bold),
                        new MarkdownTag(new BoldTagToken(10), MarkdownTagType.Bold, true),
                        new MarkdownTag(new ItalicsTagToken(5), MarkdownTagType.Italics),
                        new MarkdownTag(new ItalicsTagToken(7), MarkdownTagType.Italics, true)
                    ]
                })
            .SetName("08. Строка: '# __Hello _my_ world__'. В строке три типа тег-токенов, все они валидные."),
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new MarkedListTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello")
                    ],
                    ValidSingleMarkdownTags = 
                    [
                        new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(0))
                    ]
                },
                new Expected
                {
                    Tags = [new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList)]
                })
            .SetName("09. Строка: '* Hello'. В строке единственный валидный тег - маркированный список."), 
        
        new TestCaseData(
                new TestCase
                {
                    ParagraphOfTokens =
                    [
                        new MarkedListTagToken(0),
                        new SpaceToken(),
                        new TextToken("Hello"),
                        new SpaceToken(),
                        new MarkedListTagToken(4),
                        new SpaceToken(),
                        new TextToken("world")
                    ],
                    ValidSingleMarkdownTags = 
                    [
                        new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(0))
                    ],
                    InvalidSingleMarkdownTags = 
                    [
                        new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(4))
                    ]
                },
                new Expected
                {
                    Tags = [new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList)]
                })
            .SetName("10. Строка: '* Hello * world'. В строке два тега - маркированный список: валидный и невалидный."), 
    };
}