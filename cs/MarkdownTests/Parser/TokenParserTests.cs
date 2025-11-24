using FluentAssertions;
using Markdown.Domains;
using Markdown.Domains.Nodes;
using Markdown.Parser;

// ReSharper disable UseCollectionExpression

namespace MarkdownTest.Parser;

[TestFixture]
public class TokenParserTests
{
    private static IEnumerable<TestCaseData> TokenParserCases_Default()
    {
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Grid, "#"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "Test")
            },
            new RootNode(new List<Node>
            {
                new HeaderNode(1, new List<Node>
                {
                    new TextNode("Test")
                })
            })
        ).SetName("HeaderNode_Level1");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "Test")
            },
            new RootNode(new List<Node>
            {
                new HeaderNode(2, new List<Node>
                {
                    new TextNode("Test")
                })
            })
        ).SetName("HeaderNode_Level2");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "Test")
            },
            new RootNode(new List<Node>
            {
                new HeaderNode(6, new List<Node>
                {
                    new TextNode("Test")
                })
            })
        ).SetName("HeaderNode_Level6");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Test"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new ItalicNode(new List<Node>
                {
                    new TextNode("Test")
                })
            })
        ).SetName("ItalicUnderscoreNode");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "text"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new BoldNode(new List<Node>
                {
                    new TextNode("text")
                })
            })
        ).SetName("BoldUnderscoreNode");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Test"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new ItalicNode(new List<Node>
                {
                    new TextNode("Test")
                })
            })
        ).SetName("ItalicUnderscoreNode_Duplicate");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Number, "123"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new BoldNode(new List<Node>
                {
                    new TextNode("123")
                })
            })
        ).SetName("BoldUnderscoreNode_Number");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Escape, "\n"),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "Header"),
                new(TokenType.Space, " "),
                new(TokenType.Grid, "#"),
                new(TokenType.Grid, "#"),
                new(TokenType.Escape, "\n"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "word"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Space, " "),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "word"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new ItalicNode(new List<Node>
                {
                    new TextNode("Test")
                }),
                new NewLineNode(),
                new HeaderNode(2, new List<Node>
                {
                    new TextNode("Header"),
                    new TextNode(" "),
                    new TextNode("##"),
                }),
                new NewLineNode(),
                new BoldNode(new List<Node>
                {
                    new TextNode("word")
                }),
                new TextNode(" "),
                new ItalicNode(new List<Node>
                {
                    new TextNode("word")
                })
            })
        ).SetName("WithDifferentTags");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Word, "text"),
                new(TokenType.RightSquareBracket, "]"),
                new(TokenType.LeftParenthesis, "("),
                new(TokenType.Word, "example.com"),
                new(TokenType.RightParenthesis, ")")
            },
            new RootNode(new List<Node>
            {
                new LinkNode(LinkNodeType.LinkRoot,
                    new List<Node>
                    {
                        new LinkNode(LinkNodeType.MeaningText,
                            new List<Node>
                            {
                                new TextNode("text")
                            }
                        ),
                        new LinkNode(LinkNodeType.LinkText,
                            new List<Node>
                            {
                                new TextNode("example.com")
                            }
                        )
                    }
                )
            })
        ).SetName("WithLink");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "text"),
                new(TokenType.Underscore, "_"),
                new(TokenType.RightSquareBracket, "]"),
                new(TokenType.LeftParenthesis, "("),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "example.com"),
                new(TokenType.Underscore, "_"),
                new(TokenType.RightParenthesis, ")")
            },
            new RootNode(new List<Node>
            {
                new LinkNode(LinkNodeType.LinkRoot,
                    new List<Node>
                    {
                        new LinkNode(LinkNodeType.MeaningText,
                            new List<Node>
                            {
                                new ItalicNode(new List<Node>
                                {
                                    new TextNode("text")
                                })
                            }
                        ),
                        new LinkNode(LinkNodeType.LinkText,
                            new List<Node>
                            {
                                new ItalicNode(new List<Node>
                                {
                                    new TextNode("example.com")
                                })
                            }
                        )
                    }
                )
            })
        ).SetName("WithLinkItalicText");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "text"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.RightSquareBracket, "]"),
                new(TokenType.LeftParenthesis, "("),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "example.com"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.RightParenthesis, ")")
            },
            new RootNode(new List<Node>
            {
                new LinkNode(LinkNodeType.LinkRoot,
                    new List<Node>
                    {
                        new LinkNode(LinkNodeType.MeaningText,
                            new List<Node>
                            {
                                new BoldNode(new List<Node>
                                {
                                    new TextNode("text")
                                })
                            }
                        ),
                        new LinkNode(LinkNodeType.LinkText,
                            new List<Node>
                            {
                                new BoldNode(new List<Node>
                                {
                                    new TextNode("example.com")
                                })
                            }
                        )
                    }
                )
            })
        ).SetName("WithLinkBoldText");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Word, "text"),
                new(TokenType.LeftParenthesis, "("),
                new(TokenType.Word, "example.com"),
                new(TokenType.RightParenthesis, ")")
            },
            new RootNode(new List<Node>
            {
                new TextNode("["),
                new TextNode("text"),
                new TextNode("("),
                new TextNode("example.com"),
                new TextNode(")")
            })
        ).SetName("WithLinkUnclosedSquareBracket");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Word, "text"),
                new(TokenType.LeftParenthesis, "]"),
                new(TokenType.Word, "example.com"),
                new(TokenType.RightParenthesis, ")")
            },
            new RootNode(new List<Node>
            {
                new TextNode("["),
                new TextNode("text"),
                new TextNode("]"),
                new TextNode("example.com"),
                new TextNode(")")
            })
        ).SetName("WithLinkUnclosedParenthesisBracket");
    }


        [Test]
        [TestCaseSource(nameof(TokenParserCases_Default))]
        [TestCaseSource(nameof(TokenParserCases_Extreme))]
        public void Parse_ShouldParse_Correctly(List<MdToken> tokens, Node expectedNode)
        {
            var parser = new Markdown.Parser.TokenParser(tokens);

            var result = parser.Parse();

            result.Should().BeEquivalentTo(expectedNode);
        }
        
        [Test]
        public void FindClosing_SquareBrackets_ShouldReturnCorrectIndex()
        {
            var tokens = new List<MdToken>
            {
                new(TokenType.LeftSquareBracket, "["),
                new(TokenType.Word, "example"),
                new(TokenType.Number, "123"),
                new(TokenType.RightSquareBracket, "]"),
                new(TokenType.Word, "example")
            };
            const int expectedCloseIndex = 3;

            var closeIndex =  TokenParser.FindClosing(tokens, 1, 1, TokenType.RightSquareBracket);

            closeIndex.Should().Be(expectedCloseIndex);
        }
        
        private static IEnumerable<TestCaseData> TokenParserCases_Extreme()
    {
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "UnclosedItalic")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode("UnclosedItalic")
            })
        ).SetName("UnclosedItalic");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "__"),
                new(TokenType.Word, "UnclosedBold")
            },
            new RootNode(new List<Node>
            {
                new TextNode("__"),
                new TextNode("UnclosedBold")
            })
        ).SetName("UnclosedBold");

        // Любой символ можно экранировать, чтобы он не считался частью разметки.
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Slash, "\\"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "SlashUnderscore"),
                new(TokenType.Slash, "\\"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("\\_"),
                new TextNode("SlashUnderscore"),
                new TextNode("\\_")
            })
        ).SetName("SlashUnderscoreBold_BothSides");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Slash, "\\"),
                new(TokenType.Grid, "#"),
                new(TokenType.Space, " "),
                new(TokenType.Underscore, "HEADER")
            },
            new RootNode(new List<Node>
            {
                new TextNode("\\# "),
                new TextNode(" "),
                new TextNode("HEADER")
            })
        ).SetName("SlashBeforeGrid");

        // Если внутри подчерков пустая строка ____, то они остаются символами подчерка.
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode("_"),
                new TextNode("_"),
                new TextNode("_")
            })
        ).SetName("FourSlash");

        // __Непарные_ символы в рамках одного абзаца не считаются выделением.
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "Непарные"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode("_"),
                new TextNode("Непарные"),
                new TextNode("_")
            })
        ).SetName("Unpaired");

        // Внутри __двойного выделения _одинарное_ тоже__ работает
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new BoldNode(new List<Node>
                {
                    new TextNode("test"),
                    new ItalicNode(new List<Node>
                    {
                        new TextNode("test")
                    }),
                    new TextNode("test")
                })
            })
        ).SetName("WithinDoubleSelectionSingle");

        // Но не наоборот — внутри _одинарного __двойное__ не_ работает.
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "test"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new ItalicNode(new List<Node>
                {
                    new TextNode("test"),
                    new TextNode("_"),
                    new TextNode("_"),
                    new TextNode("test"),
                    new TextNode("_"),
                    new TextNode("_"),
                    new TextNode("test")
                })
            })
        ).SetName("WithinSingleSelectionDouble");

        // Подчерки внутри текста с цифрами_12_3 не считаются выделением и должны оставаться символами подчерка
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Word, "цифрами"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Number, "12"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Number, "3")
            },
            new RootNode(new List<Node>
            {
                new TextNode("цифрами"),
                new TextNode("_"),
                new TextNode("12"),
                new TextNode("_"),
                new TextNode("3")
            })
        ).SetName("UnderscoreInNumbers");

        // Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "нач"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "але")
            },
            new RootNode(new List<Node>
            {
                new ItalicNode(new List<Node>
                {
                    new TextNode("нач")
                }),
                new TextNode("але")
            })
        ).SetName("UnderscoreInWord_Start");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Word, "сер"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "eди"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "не")
            },
            new RootNode(new List<Node>
            {
                new TextNode("сер"),
                new ItalicNode(new List<Node>
                {
                    new TextNode("eди")
                }),
                new TextNode("не")
            })
        ).SetName("UnderscoreInWord_Middle");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Word, "кон"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "це"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("кон"),
                new ItalicNode(new List<Node>
                {
                    new TextNode("це")
                })
            })
        ).SetName("UnderscoreInWord_Finish");

        // В то же время выделение в ра_зных сл_овах не работает
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Word, "ра"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "зных"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "сло"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "вах")
            },
            new RootNode(new List<Node>
            {
                new TextNode("ра"),
                new TextNode("_"),
                new TextNode("зных"),
                new TextNode(" "),
                new TextNode("сло"),
                new TextNode("_"),
                new TextNode("вах")
            })
        ).SetName("UnderscoreDifferentWord");

        // эти_ подчерки_ и эти _подчерки _ не считаются выделением, 
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "подчерки"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode(" "),
                new TextNode("подчерки"),
                new TextNode("_")
            })
        ).SetName("SpaceAfterOpenUnderscore");

        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "подчерки"),
                new(TokenType.Space, " "),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode("подчерки"),
                new TextNode(" "),
                new TextNode("_")
            })
        ).SetName("SpaceBeforeClosedUnderscore");

        // В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.
        yield return new TestCaseData(
            new List<MdToken>
            {
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "пересечения"),
                new(TokenType.Space, " "),
                new(TokenType.Underscore, "_"),
                new(TokenType.Word, "двойных"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Underscore, "_"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "и"),
                new(TokenType.Space, " "),
                new(TokenType.Word, "одинарных"),
                new(TokenType.Underscore, "_")
            },
            new RootNode(new List<Node>
            {
                new TextNode("_"),
                new TextNode("_"),
                new TextNode("пересечения"),
                new TextNode(" "),
                new TextNode("_"),
                new TextNode("двойных"),
                new TextNode("_"),
                new TextNode("_"),
                new TextNode(" "),
                new TextNode("и"),
                new TextNode(" "),
                new TextNode("одинарных"),
                new TextNode("_")
            })
        ).SetName("UnionDoubleAndOneUnderscore");
    }
        
    [Test]
    public void FindClosing_ParenthesisBrackets_ShouldReturnCorrectIndex()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Word, "example"),
            new(TokenType.LeftParenthesis, "("),
            new(TokenType.Word, "example"),
            new(TokenType.Number, "123"),
            new(TokenType.RightParenthesis, ")"),
            new(TokenType.Word, "example")
        };
        const int expectedCloseIndex = 4;

        var closeIndex = TokenParser.FindClosing(tokens, 2, 1, TokenType.RightParenthesis);

        closeIndex.Should().Be(expectedCloseIndex);
    }
}