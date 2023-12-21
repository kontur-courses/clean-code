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
            yield return new TestCaseData(
                    "_ a_  _a _",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 3, 1),
                        new(new EmphasisToken(), false, 6, 1),
                        new(new EmphasisToken(), true, 9, 1)
                    }).Returns(EmptyList)
                .SetName("Removes opening and closing tags if they're followed or preceded by a space respectively.");
        }
    }

    public static IEnumerable EmptyLinesTests
    {
        get
        {
            yield return new TestCaseData(
                    "____",
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new StrongToken(), true, 2, 2)
                    }).Returns(EmptyList)
                .SetName("Removes tags that surrond an empty line.");

            yield return new TestCaseData(
                "__a____a__",
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2),
                    new(new StrongToken(), false, 5, 2),
                    new(new StrongToken(), true, 8, 2)
                }).Returns(
                new List<Token>
                {
                    new(new StrongToken(), false, 0, 2),
                    new(new StrongToken(), true, 3, 2),
                    new(new StrongToken(), false, 5, 2),
                    new(new StrongToken(), true, 8, 2)
                }).SetName("Does not remove adjacent tags if they dont surrond an empty line.");
        }
    }

    public static IEnumerable BreakingNumbersTests
    {
        get
        {
            yield return new TestCaseData(
                    "_a 1_3 a_",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 4, 1),
                        new(new EmphasisToken(), false, 8, 1)
                    }).Returns(
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 8, 1)
                    })
                .SetName(
                    "Tags that break the number are not even considered as tags, so they dont break the pairing of other tags");

            yield return new TestCaseData(
                "_12_ _1_3",
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                    new(new EmphasisToken(), false, 5, 1),
                    new(new EmphasisToken(), true, 7, 1)
                }).Returns(
                new List<Token>
                {
                    new(new EmphasisToken(), false, 0, 1),
                    new(new EmphasisToken(), true, 3, 1),
                }).SetName(
                "Does not remove tags that surrond the entire number.");
        }
    }

    public static IEnumerable DifferentWordsTests
    {
        get
        {
            yield return new TestCaseData(
                    "a_a a_a",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 1, 1),
                        new(new EmphasisToken(), true, 5, 1)
                    })
                .Returns(EmptyList)
                .SetName("Removes all tags if both are fully enclosed within different words.");

            yield return new TestCaseData(
                    "_a a_a a_a a_",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 4, 1),
                        new(new EmphasisToken(), false, 8, 1),
                        new(new EmphasisToken(), true, 12, 1)
                    })
                .Returns(EmptyList)
                .SetName("Removes all tags if at least one is fully enclosed within a different word.");
        }
    }

    public static IEnumerable PartOfWordTests
    {
        get
        {
            yield return new TestCaseData(
                    "_te_xt t_ex_t te_xt_",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 3, 1),
                        new(new EmphasisToken(), false, 8, 1),
                        new(new EmphasisToken(), true, 11, 1),
                        new(new EmphasisToken(), false, 16, 1),
                        new(new EmphasisToken(), true, 19, 1)
                    })
                .Returns(
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 3, 1),
                        new(new EmphasisToken(), false, 8, 1),
                        new(new EmphasisToken(), true, 11, 1),
                        new(new EmphasisToken(), false, 16, 1),
                        new(new EmphasisToken(), true, 19, 1)
                    })
                .SetName("Does not remove tags that emphasize parts of the same word.");
        }
    }

    public static IEnumerable NestedTagsTests
    {
        get
        {
            yield return new TestCaseData(
                    "_a __a__ a_",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new StrongToken(), false, 3, 2),
                        new(new StrongToken(), true, 6, 2),
                        new(new EmphasisToken(), true, 10, 1)
                    })
                .Returns(
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 10, 1)
                    })
                .SetName("Removes strong tags nested in emphasis tags");

            yield return new TestCaseData(
                    "__a _a_ a__",
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new EmphasisToken(), false, 4, 1),
                        new(new EmphasisToken(), true, 6, 1),
                        new(new StrongToken(), true, 9, 2),
                    })
                .Returns(
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new EmphasisToken(), false, 4, 1),
                        new(new EmphasisToken(), true, 6, 1),
                        new(new StrongToken(), true, 9, 2)
                    })
                .SetName("Does not remove emphasis tags nested in strong tags.");
        }
    }

    public static IEnumerable IntersectingTagsTests
    {
        get
        {
            yield return new TestCaseData(
                    "__a_ __b_",
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new EmphasisToken(), false, 3, 1),
                        new(new StrongToken(), true, 5, 2),
                        new(new EmphasisToken(), true, 8, 1)
                    })
                .Returns(EmptyList)
                .SetName("Removes intersecting tags in a simple case");

            yield return new TestCaseData(
                    "__a__a_ __b_",
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new StrongToken(), true, 3, 2),
                        new(new EmphasisToken(), false, 6, 1),
                        new(new StrongToken(), false, 8, 2),
                        new(new EmphasisToken(), true, 11, 1),
                    })
                .Returns(
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new StrongToken(), true, 3, 2)
                    })
                .SetName("Does not remove tags that intersect but have a pair outside of the intersection");
        }
    }

    public static IEnumerable UnpairedTagsTests
    {
        get
        {
            yield return new TestCaseData(
                    "__a_",
                    new List<Token>
                    {
                        new(new StrongToken(), false, 0, 2),
                        new(new EmphasisToken(), false, 3, 1)
                    })
                .Returns(EmptyList)
                .SetName("Removes unpaired tags in a simple case with 2 different tag types");

            yield return new TestCaseData(
                    "_a_ __sd __a__",
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 2, 1),
                        new(new StrongToken(), false, 4, 2),
                        new(new StrongToken(), true, 9, 2),
                        new(new StrongToken(), false, 12, 2)
                    })
                .Returns(
                    new List<Token>
                    {
                        new(new EmphasisToken(), false, 0, 1),
                        new(new EmphasisToken(), true, 2, 1)
                    })
                .SetName("Removes unpaired tags in a complex case");
        }
    }

    public static IEnumerable EmptyInputTests
    {
        get
        {
            yield return new TestCaseData("", new List<Token>())
                .Returns(new List<Token>())
                .SetName("With empty line and empty list of tokens returns an empty list of tokens.");
        }
    }
}