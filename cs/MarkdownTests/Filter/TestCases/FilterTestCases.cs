using System.Collections;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace MarkdownTests.Filter.TestCases;

public class FilterTestCases
{
    private static readonly List<Token> EmptyList = new();

    public static IEnumerable SpaceInterruptionTests
    {
        get
        {
            yield return new FilterTestData(
                "_ a_  _a _",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                    new(new EmphasisToken(), false, 6, 1),
                    new(new EmphasisToken(), true, 9, 1)
                },
                EmptyList);

            yield return new FilterTestData(
                "a _ a_",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 2, 1),
                    new(new EmphasisToken(), true, 5, 1)
                },
                EmptyList);
        }
    }

    public static IEnumerable EmptyLinesTests
    {
        get
        {
            yield return new FilterTestData(
                "____",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 2, 2)
                },
                EmptyList);

            yield return new FilterTestData(
                "__a____a__",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2),
                    new(new StrongToken(), false, 5, 2),
                    new(new StrongToken(), true, 8, 2)
                },
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2),
                    new(new StrongToken(), false, 5, 2),
                    new(new StrongToken(), true, 8, 2)
                });
        }
    }

    public static IEnumerable BreakingNumbersTests
    {
        get
        {
            yield return new FilterTestData(
                "_a 1_3 a_",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 4, 1),
                    new(new EmphasisToken(), false, 8, 1)
                },
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 8, 1)
                });

            yield return new FilterTestData(
                "_12_ _1_3",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                    new(new EmphasisToken(), false, 5, 1),
                    new(new EmphasisToken(), true, 7, 1)
                },
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                });
        }
    }

    public static IEnumerable DifferentWordsTests
    {
        get
        {
            yield return new FilterTestData(
                "_a aa_a ",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 5, 1)
                },
                EmptyList);

            yield return new FilterTestData(
                "_a a_a a_a a_",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 4, 1),
                    new(new EmphasisToken(), false, 8, 1),
                    new(new EmphasisToken(), true, 12, 1)
                },
                EmptyList);
        }
    }

    public static IEnumerable PartOfWordTests
    {
        get
        {
            yield return new FilterTestData(
                "_te_xt t_ex_t te_xt_",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                    new(new EmphasisToken(), false, 8, 1),
                    new(new EmphasisToken(), true, 11, 1),
                    new(new EmphasisToken(), false, 16, 1),
                    new(new EmphasisToken(), true, 19, 1)
                },
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                    new(new EmphasisToken(), false, 8, 1),
                    new(new EmphasisToken(), true, 11, 1),
                    new(new EmphasisToken(), false, 16, 1),
                    new(new EmphasisToken(), true, 19, 1)
                });
        }
    }

    public static IEnumerable NestedTagsTests
    {
        get
        {
            yield return new FilterTestData(
                "_a __a__ a_",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new StrongToken(), false, 3, 2),
                    new(new StrongToken(), true, 6, 2),
                    new(new EmphasisToken(), true, 10, 1)
                },
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 10, 1)
                });

            yield return new FilterTestData(
                "__a _a_ a__",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new EmphasisToken(), false, 4, 1),
                    new(new EmphasisToken(), true, 6, 1),
                    new(new StrongToken(), true, 9, 2),
                },
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new EmphasisToken(), false, 4, 1),
                    new(new EmphasisToken(), true, 6, 1),
                    new(new StrongToken(), true, 9, 2)
                });
        }
    }

    public static IEnumerable IntersectingTagsTests
    {
        get
        {
            yield return new FilterTestData(
                "__a_ __b_",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new EmphasisToken(), false, 3, 1),
                    new(new StrongToken(), true, 5, 2),
                    new(new EmphasisToken(), true, 8, 1)
                },
                EmptyList);

            yield return new FilterTestData(
                "__a__a_ __b_",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2),
                    new(new EmphasisToken(), false, 6, 1),
                    new(new StrongToken(), false, 8, 2),
                    new(new EmphasisToken(), true, 11, 1),
                },
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2)
                });
        }
    }

    public static IEnumerable UnpairedTagsTests
    {
        get
        {
            yield return new FilterTestData(
                "__a_",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new EmphasisToken(), false, 3, 1)
                },
                EmptyList
            );

            yield return new FilterTestData(
                "_a_ __sd __a__",
                new List<Token>
                {
                    new( new EmphasisToken(), false, 0, 1),
                    new( new EmphasisToken(), true, 2, 1),
                    new( new StrongToken(), false, 4, 2),
                    new( new StrongToken(), true, 9, 2),
                    new( new StrongToken(), false, 12, 2)
                },
                new List<Token>
                {
                    new( new EmphasisToken(), false, 0, 1),
                    new( new EmphasisToken(), true, 2, 1)
                }
            );
        }
    }

    public static IEnumerable EmptyInputTests
    {
        get
        {
            yield return new FilterTestData(
                "",
                new List<Token>(),
                new List<Token>()
            );
        }
    }
}