using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownParser_Should
{
    private readonly MarkdownParser parser = new ();

    public static IEnumerable<TestCaseData> TokenParsingTestCases()
    {
        yield return new TestCaseData(
            "Это _курсив_ текст",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new (TokenType.Text, "курсив")
                }),
                new (TokenType.Text, " текст")
            }).SetName("ShouldParse_WhenItalicTag");

        yield return new TestCaseData(
            "Это __полужирный__ текст",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "полужирный")
                }),
                new (TokenType.Text, " текст")
            }).SetName("ShouldParse_WhenStrongTag");

        yield return new TestCaseData(
            "# Заголовок",
            new List<BaseToken>
            {
                new (TokenType.Header, children: new List<BaseToken>
                {
                    new (TokenType.Text, "Заголовок")
                })
            }).SetName("ShouldParse_WhenHeaderTag");

        yield return new TestCaseData(
            "Это __жирный _и курсивный_ текст__",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "жирный "),
                    new (TokenType.Emphasis, children: new List<BaseToken>
                    {
                        new (TokenType.Text, "и курсивный")
                    }),
                    new (TokenType.Text, " текст")
                })
            }).SetName("ShouldParse_WhenNestedItalicAndStrongTags");

        yield return new TestCaseData(
            "Это _курсив_,а это __жирный__ текст.",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new (TokenType.Text, "курсив")
                }),
                new (TokenType.Text, ",а это "),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "жирный")
                }),
                new (TokenType.Text, " текст.")
            }).SetName("ShouldParse_WhenMultipleTokensInLine");

        yield return new TestCaseData(
            "en_d._,mi__dd__le",
            new List<BaseToken>
            {
                new (TokenType.Text, "en"),
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new (TokenType.Text, "d.")
                }),
                new (TokenType.Text, ",mi"),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "dd")
                }),
                new (TokenType.Text, "le")
            }).SetName("ShouldParse_WhenBoundedTagsInOneWord");

        yield return new TestCaseData(
            @"Экранированный \_символ\_",
            new List<BaseToken>
            {
                new (TokenType.Text, "Экранированный _символ_")
            }).SetName("ShouldParse_WhenEscapedTags");

        yield return new TestCaseData(
            "Это __двойное _и одинарное_ выделение__",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "двойное "),
                    new (TokenType.Emphasis, children: new List<BaseToken>
                    {
                        new (TokenType.Text, "и одинарное")
                    }),
                    new (TokenType.Text, " выделение")
                })
            }).SetName("ShouldParse_WhenItalicInStrong");

        yield return new TestCaseData(
            "# Заголовок __с _разными_ символами__",
            new List<BaseToken>
            {
                new (TokenType.Header, children: new List<BaseToken>
                {
                    new (TokenType.Text, "Заголовок "),
                    new (TokenType.Strong, children: new List<BaseToken>
                    {
                        new (TokenType.Text, "с "),
                        new (TokenType.Emphasis, children: new List<BaseToken>
                        {
                            new (TokenType.Text, "разными")
                        }),
                        new (TokenType.Text, " символами")
                    })
                })
            }).SetName("ShouldParse_WhenHeaderWithTags");

        yield return new TestCaseData(
            "# Заголовок 1\n# Заголовок 2",
            new List<BaseToken>
            {
                new (TokenType.Header, children: new List<BaseToken>
                {
                    new (TokenType.Text, "Заголовок 1")
                }),
                new (TokenType.Text, "\n"),
                new (TokenType.Header, children: new List<BaseToken>
                {
                    new (TokenType.Text, "Заголовок 2")
                })
            }).SetName("ShouldParse_WhenMultipleHeaders");

        yield return new TestCaseData(
            "Это текст с [ссылкой](http://link.com)",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это текст с "),
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "ссылкой")
                }, "http://link.com")
            }).SetName("ShouldParse_WhenSimpleLinkTag");

        yield return new TestCaseData(
            "Это текст с [двумя](http://link1.com) [ссылками](http://link2.com)",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это текст с "),
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "двумя")
                }, "http://link1.com"),
                new (TokenType.Text, " "),
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "ссылками")
                }, "http://link2.com")
            }).SetName("ShouldParse_WhenSeveralLinkTags");

        yield return new TestCaseData(
            "_[Ссылка](http://link.com) внутри курсива_",
            new List<BaseToken>
            {
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new LinkToken(new List<BaseToken>
                    {
                        new (TokenType.Text, "Ссылка")
                    }, "http://link.com"),
                    new (TokenType.Text, " внутри курсива")
                })
            }).SetName("ShouldParse_WhenLinkInsideItalic");

        yield return new TestCaseData(
            "Это [ссылка с _тегом_](http://link.com)",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "ссылка с "),
                    new (TokenType.Emphasis, children: new List<BaseToken>
                    {
                        new (TokenType.Text, "тегом")
                    })
                }, "http://link.com")
            }).SetName("ShouldParse_WhenLinkWithTagInside");

        yield return new TestCaseData(
            "Пустая ссылка [](http://link.com)",
            new List<BaseToken>
            {
                new (TokenType.Text, "Пустая ссылка "),
                new LinkToken(new List<BaseToken>(), "http://link.com")
            }).SetName("ShouldParse_WhenLinkWithEmptyText");

        yield return new TestCaseData(
            "Пустая ссылка []()",
            new List<BaseToken>
            {
                new (TokenType.Text, "Пустая ссылка "),
                new LinkToken(new List<BaseToken>(), "")
            }).SetName("ShouldParse_WhenLinkWithEmptyUrl");

        yield return new TestCaseData(
            @"[Ссылка с экранированными \] символами\]](http://link.com)",
            new List<BaseToken>
            {
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "Ссылка с экранированными ] символами]")
                }, "http://link.com")
            }).SetName("ShouldParse_WhenLinkWithEscapedSymbol");

        yield return new TestCaseData(
            "Это [ссылка с [ссылка с _тегом_](http://link.com)](http://link.com)",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это "),
                new LinkToken(new List<BaseToken>
                {
                    new (TokenType.Text, "ссылка с "),
                    new LinkToken(new List<BaseToken>
                    {
                        new (TokenType.Text, "ссылка с "),
                        new (TokenType.Emphasis, children: new List<BaseToken>
                        {
                            new (TokenType.Text, "тегом")
                        })
                    }, "http://link.com")
                }, "http://link.com")
            }).SetName("ShouldParse_WhenLinkInsideLink");

        yield return new TestCaseData(
            "Если пустая _______ строка",
            new List<BaseToken>
            {
                new (TokenType.Text, "Если пустая _______ строка")
            }).SetName("ShouldNotParse_WhenEmptyEmphasis");

        yield return new TestCaseData(
            "Текст с цифрами_12_3 не должен выделяться",
            new List<BaseToken>
            {
                new (TokenType.Text, "Текст с цифрами_12_3 не должен выделяться")
            }).SetName("ShouldNotParse_WhenUnderscoresInNumbers");

        yield return new TestCaseData(
            @"Здесь сим\волы экранирования\ \должны остаться.\",
            new List<BaseToken>
            {
                new (TokenType.Text, @"Здесь сим\волы экранирования\ \должны остаться.\")
            }).SetName("ShouldNotParse_WhenEscapingSymbols");

        yield return new TestCaseData(
            @"\\_вот это будет выделено тегом_",
            new List<BaseToken>
            {
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new (TokenType.Text, "вот это будет выделено тегом")
                })
            }).SetName("ShouldNotParse_WhenEscapedYourself");

        yield return new TestCaseData(
            "и в нач_але_,и в сер__еди__не",
            new List<BaseToken>
            {
                new (TokenType.Text, "и в нач"),
                new (TokenType.Emphasis, children: new List<BaseToken>
                {
                    new (TokenType.Text, "але")
                }),
                new (TokenType.Text, ",и в сер"),
                new (TokenType.Strong, children: new List<BaseToken>
                {
                    new (TokenType.Text, "еди")
                }),
                new (TokenType.Text, "не")
            }).SetName("ShouldParse_WhenTagsInSimilarWord");

        yield return new TestCaseData(
            "Это пер_вый в_торой пример.",
            new List<BaseToken>
            {
                new (TokenType.Text, "Это пер_вый в_торой пример.")
            }).SetName("ShouldNotParse_WhenTagInDifferentWords");

        yield return new TestCaseData(
            "_e __e",
            new List<BaseToken>
            {
                new (TokenType.Text, "_e __e")
            }).SetName("ShouldNotParse_WhenUnclosedTags");

        yield return new TestCaseData(
            "_e __s e_ s__",
            new List<BaseToken>
            {
                new (TokenType.Text, "_e __s e_ s__")
            }).SetName("ShouldNotParse_WhenTagsIntersection");

        yield return new TestCaseData(
            "__s \n s__,_e \r\n e_",
            new List<BaseToken>
            {
                new (TokenType.Text, "__s \n s__,_e \r\n e_")
            }).SetName("ShouldNotParse_WhenTagsIntersectionNewLines");

        yield return new TestCaseData(
            "Текст с [незавершённой ссылкой](http://link.com",
            new List<BaseToken>
            {
                new (TokenType.Text, "Текст с "),
                new (TokenType.Text, "[незавершённой ссылкой](http://link.com")
            }).SetName("ShouldNotParse_WhenUnfinishedLinkTag");
    }

    [TestCaseSource(nameof(TokenParsingTestCases))]
    public void MarkdownParser_ShouldParseTokens(string input, List<BaseToken> expectedTokens)
    {
        var actualTokens = parser.ParseTokens(input).ToList();
        CompareTokens(expectedTokens, actualTokens);
    }

    private void CompareTokens(IReadOnlyList<BaseToken> expected, IReadOnlyList<BaseToken> actual)
    {
        actual.Should().HaveCount(expected.Count, "Количество токенов должно совпадать");
        for (int i = 0; i < expected.Count; i++)
        {
            actual[i].Type.Should().Be(expected[i].Type, $"Тип токена на позиции {i} должен совпадать");
            actual[i].Content.Should().Be(expected[i].Content, $"Содержимое токена на позиции {i} должно совпадать");

            if (expected[i].Children.Any())
            {
                CompareTokens(expected[i].Children, actual[i].Children);
            }
            else
            {
                actual[i].Children.Should().BeNullOrEmpty($"Токен на позиции {i} не должен иметь дочерних элементов");
            }
        }
    }
}