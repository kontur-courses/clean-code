using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MdTestCases
    {
        public static IEnumerable<TestCaseData> RenderTestCases
        {
            get
            {
                yield return new TestCaseData("", "")
                    .SetName("ReturnEmptyString_WhenInputIsEmpty");
                yield return new TestCaseData("a", "a")
                    .SetName("ReturnEmptyString_WhenThereAreNoMarkdownsInInput");
                yield return new TestCaseData("# a", "<h1>a</h1>")
                    .SetName("ReturnCorrectString_WhenInputContainsHeaderWithEOF");
                yield return new TestCaseData("# a\n", "<h1>a</h1>\n")
                    .SetName("ReturnCorrectString_WhenInputContainsHeaderWithLf");
                yield return new TestCaseData("# a\n# a\n# a\n", "<h1>a</h1>\n<h1>a</h1>\n<h1>a</h1>\n")
                    .SetName("ReturnCorrectString_WhenThereAreManyHeadersAndManyStringsInInput");
                yield return new TestCaseData("_a_", "<em>a</em>")
                    .SetName("ReturnCorrectString_WhenInputContainsItalic");
                yield return new TestCaseData("__a__", "<strong>a</strong>")
                    .SetName("ReturnCorrectString_WhenInputContainsBold");
                yield return new TestCaseData("__ a__", "__ a__")
                    .SetName("ReturnCorrectString_WhenThereIsIncorrectOpenBoldTag");
                yield return new TestCaseData("__a __", "__a __")
                    .SetName("ReturnCorrectString_WhenThereIsIncorrectCloseBoldTag");
                yield return new TestCaseData("_ a_", "_ a_")
                    .SetName("ReturnCorrectString_WhenThereIsIncorrectOpenItalicTag");
                yield return new TestCaseData("_a _", "_a _")
                    .SetName("ReturnCorrectString_WhenThereIsIncorrectCloseItalicTag");
                yield return new TestCaseData("a_a a_a", "a_a a_a")
                    .SetName("ReturnCorrectString_WhenItalicTagInsideDifferentWords");
                yield return new TestCaseData("_a", "_a")
                    .SetName("ReturnCorrectString_WhenThereIsUnpairedItalicTag");
                yield return new TestCaseData("_a\na_a", "_a\na_a")
                    .SetName("ReturnCorrectString_WhenThereIsUnpairedItalicTagInParagraph");
                yield return new TestCaseData("_a__a_a__", "_a__a_a__")
                    .SetName("ReturnCorrectString_WhenThereIsItalicBoldIntersection");
                yield return new TestCaseData("__a_a__a_", "__a_a__a_")
                    .SetName("ReturnCorrectString_WhenThereIsBoldItalicIntersection");
                yield return new TestCaseData("a__a a__a", "a__a a__a")
                    .SetName("ReturnCorrectString_WhenBoldTagInsideDifferentWords");
                yield return new TestCaseData("__a", "__a")
                    .SetName("ReturnCorrectString_WhenThereIsUnpairedBoldTag");
                yield return new TestCaseData("__a\na__a", "__a\na__a")
                    .SetName("ReturnCorrectString_WhenThereIsUnpairedBoldTagInParagraph");
                yield return new TestCaseData("1__1__1", "1__1__1")
                    .SetName("ReturnCorrectString_WhenBoldTagInsideTextWithDigits");
                yield return new TestCaseData("1_1_1", "1_1_1")
                    .SetName("ReturnCorrectString_WhenItalicTagInsideTextWithDigits");
                yield return new TestCaseData("![abc](abc)", "<img src=\"abc\" alt=\"abc\">")
                    .SetName("ReturnCorrectString_WhenInputContainsImage");
                yield return new TestCaseData(
                        "# ![abc](abc) __a__ _a_ __a_a_a__ \\_a\\_ _a__a__a_ __a_a__a_ _a__a_a__ \n",
                        "<h1><img src=\"abc\" alt=\"abc\"> <strong>a</strong> <em>a</em> <strong>a<em>a</em>a</strong> _a_ <em>a__a__a</em> __a_a__a_ _a__a_a__ </h1>\n")
                    .SetName("ReturnCorrectString_WhenThereAreManyTags");
            }
        }
    }
}