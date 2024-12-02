using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser;
using Markdown.TokenParser.Nodes;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTokenParserTests
{
    private static IEnumerable<TestCaseData> MdTokenParseCases()
    {
        var tokenizer = new MdTokenizer();
        yield return new TestCaseData("Test",
                new Node(TypeOfNode.Body , 3, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 3,new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    })
                }))
            .SetName("OnlyWord");
        
        yield return new TestCaseData("123",
                new Node(TypeOfNode.Body, 3,new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "123")
                    })
                }))
            .SetName("OnlyNumber");
        
        yield return new TestCaseData("_Test_",
                new Node(TypeOfNode.Body, 5, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, 5, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 1, Value: "Test")
                        })
                    })
                }))
            .SetName("EmphasisWord");
        
        yield return new TestCaseData("__Test__",
                new Node(TypeOfNode.Body, 7, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 7, new List<Node>
                    {
                        new Node(TypeOfNode.Strong,7, new List<Node>
                            {
                                new Node(TypeOfNode.Text, 1, Value: "Test")
                            })
                    })
                }))
            .SetName("StrongWord");
        
        yield return new TestCaseData("# Test",
                new Node(TypeOfNode.Body, 5, new List<Node>
                {
                    new Node(TypeOfNode.Heading, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    })
                }))
            .SetName("HashWord");
        
        yield return new TestCaseData("_785_",
                new Node(TypeOfNode.Body, 5, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, 5, new List<Node>
                            {
                                new Node(TypeOfNode.Text, 1, Value: "785")
                            })
                    })
                }))
            .SetName("EmphasisNumber");
        
        yield return new TestCaseData("__543__",
                new Node(TypeOfNode.Body, 7, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 7, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 7, new List<Node>
                            {
                                new Node(TypeOfNode.Text, 1, Value: "543")
                            })
                    })
                }))
            .SetName("StrongNumber");
        
        yield return new TestCaseData("Test Test",
                new Node(TypeOfNode.Body, 5, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "Test"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 2, Value: "Test")
                    })
                }))
            .SetName("TextWithSeveralWords");
        
        yield return new TestCaseData("Test\nTest",
                new Node(TypeOfNode.Body, 7, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    }),
                    new Node(TypeOfNode.Newline, 1, Value: "\n"),
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    })
                }))
            .SetName("TextWithSeveralParagraphs");
        
        yield return new TestCaseData("Test\r\nTest",
                new Node(TypeOfNode.Body, 7, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    }),
                    new Node(TypeOfNode.Newline, 1, Value: "\r\n"),
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    })
                }))
            .SetName("TextWithSeveralParagraphsWithCarriageReturn");
        
        yield return new TestCaseData("# Test\nTest",
                new Node(TypeOfNode.Body, 9, new List<Node>
                {
                    new Node(TypeOfNode.Heading, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    }),
                    new Node(TypeOfNode.Newline, 1, Value: "\n"),
                    new Node(TypeOfNode.Paragraph, 3, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 3, Value: "Test")
                    })
                }))
            .SetName("HeadingAndParagraph");
        
        yield return new TestCaseData("__Strong _Emphasis_ Strong__",
                new Node(TypeOfNode.Body, 13, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 13, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 13, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 1, Value: "Strong"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Emphasis, 4, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, 1, Value: "Emphasis"), 
                                }),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "Strong")
                        })
                    })
                }))
            .SetName("EmphasisInsideStrong");
        
        yield return new TestCaseData("_Emphasis __Strong__ Emphasis_",
                new Node(TypeOfNode.Body, 13, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 13, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, 13, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 1, Value: "Emphasis"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "_"),
                            new Node(TypeOfNode.Text, 1, Value: "_"),
                            new Node(TypeOfNode.Text, 1, Value: "Strong"),
                            new Node(TypeOfNode.Text, 1, Value: "_"),
                            new Node(TypeOfNode.Text, 1, Value: "_"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "Emphasis"),
                        })
                    })
                }))
            .SetName("StrongInsideEmphasis");
        
        yield return new TestCaseData("__Test",
                new Node(TypeOfNode.Body, 5, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 5, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "_"), 
                        new Node(TypeOfNode.Text, 1, Value: "_"), 
                        new Node(TypeOfNode.Text, 2, Value: "Test")
                    })
                }))
            .SetName("UnpairedStrong");
        
        yield return new TestCaseData("_Test",
                new Node(TypeOfNode.Body, 4, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 4, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "_"), 
                        new Node(TypeOfNode.Text, 2, Value: "Test")
                    })
                }))
            .SetName("UnpairedEmphasis");
        
        yield return new TestCaseData("# Test # Hash",
                new Node(TypeOfNode.Body, 9, new List<Node>
                {
                    new Node(TypeOfNode.Heading, 9, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "Test"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "#"), 
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 2, Value: "Hash")
                    })
                }))
            .SetName("HashInsideHeading");
        
        yield return new TestCaseData("Te_st Te_st",
                new Node(TypeOfNode.Body, 9, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 9, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "Te"),
                        new Node(TypeOfNode.Text, 1, Value: "_"),
                        new Node(TypeOfNode.Text, 1, Value: "st"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "Te"),
                        new Node(TypeOfNode.Text, 1, Value: "_"),
                        new Node(TypeOfNode.Text, 2, Value: "st")
                    })
                }))
            .SetName("EmphasisInDifferentWords");
        
        yield return new TestCaseData("_ Test_",
                new Node(TypeOfNode.Body, 6, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 6, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "_"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "Test"),
                        new Node(TypeOfNode.Text, 2, Value: "_")
                    })
                }))
            .SetName("OpeningSingleUnderscoreFollowedByWhitespace");
        
        yield return new TestCaseData("__ Test__",
                new Node(TypeOfNode.Body, 8, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, 8, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 2, Value: "_"),
                        new Node(TypeOfNode.Text, 1, Value: "_"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "Test"),
                        new Node(TypeOfNode.Text, 1, Value: "_"),
                        new Node(TypeOfNode.Text, 2, Value: "_")
                    })
                }))
            .SetName("OpeningDoubleUnderscoreFollowedByWhitespace");
        
        yield return new TestCaseData("_Test _",
            new Node(TypeOfNode.Body, 6, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 6, new List<Node>
                {
                    new Node(TypeOfNode.Text, 2, Value: "_"),
                    new Node(TypeOfNode.Text, 1, Value: "Test"),
                    new Node(TypeOfNode.Text, 1, Value: " "),
                    new Node(TypeOfNode.Text, 2, Value: "_")
                })
            }))
            .SetName("ClosingSingleUnderscorePrecededByWhitespace");

        yield return new TestCaseData("__Test __",
            new Node(TypeOfNode.Body, 8, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 8, new List<Node>
                {
                    new Node(TypeOfNode.Text, 2, Value: "_"),
                    new Node(TypeOfNode.Text, 1, Value: "_"),
                    new Node(TypeOfNode.Text, 1, Value: "Test"),
                    new Node(TypeOfNode.Text, 1, Value: " "),
                    new Node(TypeOfNode.Text, 1, Value: "_"),
                    new Node(TypeOfNode.Text, 2, Value: "_")
                })
            }))
            .SetName("ClosingDoubleUnderscorePrecededByWhitespace");

        yield return new TestCaseData("Test_25_87_Test",
            new Node(TypeOfNode.Body, 9, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 9, new List<Node>
                {
                    new Node(TypeOfNode.Text, 2, Value: "Test"),
                    new Node(TypeOfNode.Text, 1, Value: "_"),
                    new Node(TypeOfNode.Text, 1, Value: "25"),
                    new Node(TypeOfNode.Text, 1, Value: "_"),
                    new Node(TypeOfNode.Text, 1, Value: "87"),
                    new Node(TypeOfNode.Text, 1, Value: "_"),
                    new Node(TypeOfNode.Text, 2, Value: "Test")
                })
            }))
            .SetName("UnderscoreInsideTheTextWithNumbers");

        yield return new TestCaseData("_Te_st",
            new Node(TypeOfNode.Body, 6, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 6, new List<Node>
                {
                    new Node(TypeOfNode.Emphasis, 4, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "Te")
                    }),
                    new Node(TypeOfNode.Text, 2, Value: "st")
                })
            }))
            .SetName("EmphasisPartOfWord");

        yield return new TestCaseData("__Te__st",
            new Node(TypeOfNode.Body, 8, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 8, new List<Node>
                {
                    new Node(TypeOfNode.Strong, 6, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "Te")
                    }),
                    new Node(TypeOfNode.Text, 2, Value: "st")
                })
            }))
            .SetName("StrongPartOfWord");

        yield return new TestCaseData("_Test_ __Test__",
            new Node(TypeOfNode.Body, 11, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 11, new List<Node>
                {
                    new Node(TypeOfNode.Emphasis, 4, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "Test")
                    }),
                    new Node(TypeOfNode.Text, 1, Value: " "),
                    new Node(TypeOfNode.Strong, 7, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "Test")
                    })
                })
            }))
            .SetName("WhitespaceSeparatedSelections");

        yield return new TestCaseData("# __Strong _Emphasis_ Strong__",
            new Node(TypeOfNode.Body, 15, new List<Node>
            {
                new Node(TypeOfNode.Heading, 15, new List<Node>
                {
                    new Node(TypeOfNode.Strong, 13, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "Strong"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Emphasis, 4, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 1, Value: "Emphasis")
                        }),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "Strong")
                    })
                })
            }))
            .SetName("HeadingWithSelections");
        
        yield return new TestCaseData("-+*",
            new Node(TypeOfNode.Body, 5, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 5, new List<Node>
                {
                    new Node(TypeOfNode.Text, 2, Value: "-"),
                    new Node(TypeOfNode.Text, 1, Value: "+"),
                    new Node(TypeOfNode.Text, 2, Value: "*")
                })
            }))
            .SetName("Bullets");

        yield return new TestCaseData("- Пункт первый",
            new Node(TypeOfNode.Body, 7, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 7, new List<Node>
                {
                    new Node(TypeOfNode.UnorderedList, 7, new List<Node>
                    {
                        new Node(TypeOfNode.ListItem, 7, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Пункт"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 2, Value: "первый")
                        })
                    })
                })
            }))
            .SetName("UnorderedListWithOneListItem");

        yield return new TestCaseData("- Пункт первый\n- Пункт второй\n- Пункт третий",
            new Node(TypeOfNode.Body, 23, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 23, new List<Node>
                {
                    new Node(TypeOfNode.UnorderedList, 23, new List<Node>
                    {
                        new Node(TypeOfNode.ListItem, 7, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Пункт"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 2, Value: "первый")
                        }),
                        new Node(TypeOfNode.ListItem, 8, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Пункт"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 2, Value: "второй")
                        }),
                        new Node(TypeOfNode.ListItem, 8, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Пункт"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 2, Value: "третий")
                        })
                    })
                })
            }))
            .SetName("UnorderedListWithSeveralListItem");

        yield return new TestCaseData("- Уровень 1.1\n    - Уровень 2.1\n- Уровень 1.2\n    - Уровень 2.1\n    - Уровень 2.2",
            new Node(TypeOfNode.Body, 61, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 61, new List<Node>
                {
                    new Node(TypeOfNode.UnorderedList, 61, new List<Node>
                    {
                        new Node(TypeOfNode.ListItem, 23, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Уровень"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "1"),
                            new Node(TypeOfNode.Text, 1, Value: "."),
                            new Node(TypeOfNode.Text, 2, Value: "1"),
                            new Node(TypeOfNode.UnorderedList, 14, new List<Node>
                            {
                                new Node(TypeOfNode.ListItem, 14, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, 2, Value: "Уровень"),
                                    new Node(TypeOfNode.Text, 1, Value: " "),
                                    new Node(TypeOfNode.Text, 1, Value: "2"),
                                    new Node(TypeOfNode.Text, 1, Value: "."),
                                    new Node(TypeOfNode.Text, 2, Value: "1")
                                })
                            })
                        }),
                        new Node(TypeOfNode.ListItem, 38, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 2, Value: "Уровень"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "1"),
                            new Node(TypeOfNode.Text, 1, Value: "."),
                            new Node(TypeOfNode.Text, 2, Value: "2"),
                            new Node(TypeOfNode.UnorderedList, 28, new List<Node>
                            {
                                new Node(TypeOfNode.ListItem, 14, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, 2, Value: "Уровень"),
                                    new Node(TypeOfNode.Text, 1, Value: " "),
                                    new Node(TypeOfNode.Text, 1, Value: "2"),
                                    new Node(TypeOfNode.Text, 1, Value: "."),
                                    new Node(TypeOfNode.Text, 2, Value: "1")
                                }),
                                new Node(TypeOfNode.ListItem, 14, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, 2, Value: "Уровень"),
                                    new Node(TypeOfNode.Text, 1, Value: " "),
                                    new Node(TypeOfNode.Text, 1, Value: "2"),
                                    new Node(TypeOfNode.Text, 1, Value: "."),
                                    new Node(TypeOfNode.Text, 2, Value: "2")
                                })
                            })
                        })
                    })
                })
            }))
            .SetName("NestedUnorderedLists");

        yield return new TestCaseData("__s _E _e_ E_ s__",
            new Node(TypeOfNode.Body, 19, new List<Node>
            {
                new Node(TypeOfNode.Paragraph, 19, new List<Node>
                {
                    new Node(TypeOfNode.Strong, 19, new List<Node>
                    {
                        new Node(TypeOfNode.Text, 1, Value: "s"),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Emphasis, 10, new List<Node>
                        {
                            new Node(TypeOfNode.Text, 1, Value: "E"),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Emphasis, 4, new List<Node>
                            {
                                new Node(TypeOfNode.Text, 1, Value: "e")
                            }),
                            new Node(TypeOfNode.Text, 1, Value: " "),
                            new Node(TypeOfNode.Text, 1, Value: "E")
                        }),
                        new Node(TypeOfNode.Text, 1, Value: " "),
                        new Node(TypeOfNode.Text, 1, Value: "s")
                    })
                })
            }))
            .SetName("EmphasisInsideEmphasis");
    }
    
    [TestCaseSource(nameof(MdTokenParseCases))]
    public void Parse_BuildCorrectAST_When(string markdown, Node expectedSyntaxTree)
    {
        var mdTokenizer = new MdTokenizer();
        var mdTokenParser = new MdTokenParser();

        var tokens = mdTokenizer.Tokenize(markdown);
        var syntaxTree = mdTokenParser.Parse(tokens);

        syntaxTree.Should().BeEquivalentTo(expectedSyntaxTree, option => option.AllowingInfiniteRecursion());
    }
}