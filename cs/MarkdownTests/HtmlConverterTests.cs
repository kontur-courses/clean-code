using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.MarkdownTags;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class HtmlConverterTests
{
    private IHtmlConverter htmlConverter;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        htmlConverter = new HtmlConverter();
    }

    [TestCaseSource(nameof(_testCases))]
    public void Test(TestCase testCase, Expected expected)
    {
        var htmlSting = htmlConverter.Convert(testCase.MarkdownParagraphs);

        htmlSting.Should().Be(expected.HtmlString);
    }

    private static IEnumerable<TestCaseData> _testCases = new[]
    {
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs =
                    [
                        new MarkdownParagraph
                        {
                            Tokens =
                            [
                                new TextToken("Здесь"),
                                new SpaceToken(),
                                new TextToken("нет"),
                                new SpaceToken(),
                                new TextToken("тегов")
                            ],
                            Tags = []
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "Здесь нет тегов"
                })
            .SetName("01. Начальная строка: 'Здесь нет тегов'. В строке отсутствуют теги."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs =
                    [
                        new MarkdownParagraph
                        {
                            Tokens =
                            [
                                new HeadingTagToken(0),
                                new SpaceToken(),
                                new TextToken("Текст"),
                                new SpaceToken(),
                                new TextToken("с"),
                                new SpaceToken(),
                                new TextToken("заголовком")
                            ],
                            Tags = [new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading)]
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "<h1>Текст с заголовком</h1>"
                })
            .SetName("02. Начальная строка: '# Текст с заголовком'. В строке единственный тег-заголовок."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs =
                    [
                        new MarkdownParagraph
                        {
                            Tokens =
                            [
                                new TextToken("Текст"),
                                new SpaceToken(),
                                new TextToken("с"),
                                new SpaceToken(),
                                new BoldTagToken(4),
                                new TextToken("полужирным"),
                                new BoldTagToken(6),
                                new SpaceToken(),
                                new TextToken("тегом")
                            ],
                            Tags = 
                            [
                                new MarkdownTag(new BoldTagToken(4), MarkdownTagType.Bold),
                                new MarkdownTag(new BoldTagToken(6), MarkdownTagType.Bold, true),
                            ]
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "Текст с <strong>полужирным</strong> тегом"
                })
            .SetName("03. Начальная строка: 'Текст с __полужирным__ тегом'. В строке единственный тег-полужирный."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs =
                    [
                        new MarkdownParagraph
                        {
                            Tokens =
                            [
                                new TextToken("Текст"),
                                new SpaceToken(),
                                new TextToken("с"),
                                new SpaceToken(),
                                new ItalicsTagToken(4),
                                new TextToken("курсивом"),
                                new ItalicsTagToken(6),
                            ],
                            Tags = 
                            [
                                new MarkdownTag(new ItalicsTagToken(4), MarkdownTagType.Italics),
                                new MarkdownTag(new ItalicsTagToken(6), MarkdownTagType.Italics, true),
                            ]
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "Текст с <em>курсивом</em>"
                })
            .SetName("04. Начальная строка: 'Текст с _курсивом_'. В строке единственный тег-курсив."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs = 
                    [
                        new MarkdownParagraph
                        {
                            Tokens = 
                            [
                                new HeadingTagToken(0),
                                new SpaceToken(),
                                new TextToken("Заголовок"),
                                new SpaceToken(),
                                new BoldTagToken(4),
                                new TextToken("с"),
                                new SpaceToken(),
                                new ItalicsTagToken(7),
                                new TextToken("разными"),
                                new ItalicsTagToken(9),
                                new SpaceToken(),
                                new TextToken("символами"),
                                new BoldTagToken(12),
                            ],
                            Tags =
                            [
                                new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading),
                                new MarkdownTag(new BoldTagToken(4), MarkdownTagType.Bold),
                                new MarkdownTag(new BoldTagToken(12), MarkdownTagType.Bold, true),
                                new MarkdownTag(new ItalicsTagToken(7), MarkdownTagType.Italics),
                                new MarkdownTag(new ItalicsTagToken(9), MarkdownTagType.Italics, true),
                            ]
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
                })
            .SetName("05. Начальная строка: '# Заголовок __с _разными_ символами__'. В строке присутствуют типы тегов: заголовок, курсив, полужирный."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs = 
                    [
                        new MarkdownParagraph
                        {
                            Tokens = 
                            [
                                new MarkedListTagToken(0),
                                new SpaceToken(),
                                new TextToken("Один"),
                                new SpaceToken(),
                                new TextToken("элемент"),
                                new SpaceToken(),
                                new TextToken("списка"),
                                new SpaceToken(),
                                new BoldTagToken(8),
                                new TextToken("с"),
                                new SpaceToken(),
                                new ItalicsTagToken(11),
                                new TextToken("разными"),
                                new ItalicsTagToken(13),
                                new SpaceToken(),
                                new TextToken("символами"),
                                new BoldTagToken(16),
                            ],
                            Tags =
                            [
                                new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList),
                                new MarkdownTag(new BoldTagToken(8), MarkdownTagType.Bold),
                                new MarkdownTag(new BoldTagToken(16), MarkdownTagType.Bold, true),
                                new MarkdownTag(new ItalicsTagToken(11), MarkdownTagType.Italics),
                                new MarkdownTag(new ItalicsTagToken(13), MarkdownTagType.Italics, true),
                            ]
                        }
                    ]
                },
                new Expected
                {
                    HtmlString = "<ul>" + 
                                 Environment.NewLine +
                                 "<li>Один элемент списка <strong>с <em>разными</em> символами</strong></li>" +
                                 Environment.NewLine + 
                                 "</ul>"
                })
            .SetName("06. Начальная строка: '* Один элемент списка __с _разными_ символами__'. В строке присутствуют типы тегов: маркированный список, курсив, полужирный."),
        
        new TestCaseData
            (
                new TestCase
                {
                    MarkdownParagraphs = 
                    [
                        new MarkdownParagraph
                        {
                            Tokens = [new HeadingTagToken(0), new SpaceToken(), new TextToken("Заголовок")],
                            Tags = [new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading)]
                        },
                        new MarkdownParagraph
                        {
                            Tokens = 
                            [
                                new MarkedListTagToken(0), 
                                new SpaceToken(), 
                                new TextToken("Тут"),
                                new SpaceToken(),
                                new ItalicsTagToken(4),
                                new TextToken("курсив"),
                                new ItalicsTagToken(6)
                            ],
                            Tags = 
                            [
                                new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList),
                                new MarkdownTag(new ItalicsTagToken(4), MarkdownTagType.Italics),
                                new MarkdownTag(new ItalicsTagToken(6), MarkdownTagType.Italics, true)
                            ]
                        },
                        new MarkdownParagraph
                        {
                            Tokens = 
                            [
                                new MarkedListTagToken(0), 
                                new SpaceToken(), 
                                new TextToken("Тут"),
                                new SpaceToken(),
                                new BoldTagToken(4),
                                new TextToken("полужирный"),
                                new ItalicsTagToken(6)
                            ],
                            Tags = 
                            [
                                new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList),
                                new MarkdownTag(new BoldTagToken(4), MarkdownTagType.Bold),
                                new MarkdownTag(new BoldTagToken(6), MarkdownTagType.Bold, true)
                            ]
                        },
                        new MarkdownParagraph
                        {
                            Tokens = 
                            [
                                new MarkedListTagToken(0), 
                                new SpaceToken(), 
                                new TextToken("Тут"),
                                new SpaceToken(),
                                new TextToken("просто"),
                                new SpaceToken(),
                                new TextToken("текст"),
                            ],
                            Tags = 
                            [
                                new MarkdownTag(new MarkedListTagToken(0), MarkdownTagType.MarkedList),
                            ]
                        },
                    ]
                },
                new Expected
                {
                    HtmlString = "<h1>Заголовок</h1>" + 
                                 Environment.NewLine + 
                                 "<ul>" + 
                                 Environment.NewLine + 
                                 "<li>Тут <em>курсив</em></li>" + 
                                 Environment.NewLine + 
                                 "<li>Тут <strong>полужирный</strong></li>" + 
                                 Environment.NewLine + 
                                 "<li>Тут просто текст</li>" + 
                                 Environment.NewLine + 
                                 "</ul>"
                })
            .SetName("07. Текст имеет несколько абзацев и содержит в себе все теги."),
    };

    public record TestCase
    {
        public required List<MarkdownParagraph> MarkdownParagraphs { get; init; }
    }

    public record Expected
    {
        public required string HtmlString { get; init; }
    }
}