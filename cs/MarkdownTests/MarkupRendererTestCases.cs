namespace MarkdownTests;

public class MarkupRendererTestCases
{
    public static IEnumerable<TestCaseData> RenderTestCases
    {
        get
        {
            yield return new TestCaseData("", "")
                .SetName("ReturnEmptyString_WhenInputIsEmpty");
            yield return new TestCaseData("a", "a")
                .SetName("ReturnEmptyString_WhenNoTagsInInput");

            yield return new TestCaseData("# a", "<h1>a</h1>\n")
                .SetName("ReturnCorrectString_WhenHeaderProvided");
            yield return new TestCaseData("# a\n# a\n# a\n", "<h1>a</h1>\n<h1>a</h1>\n<h1>a</h1>\n")
                .SetName("ReturnCorrectString_WhenManySequencedHeadersProvided");

            yield return new TestCaseData("_a_", "<em>a</em>")
                .SetName("ReturnCorrectString_WhenItalicTagProvided");
            yield return new TestCaseData("__a__", "<strong>a</strong>")
                .SetName("ReturnCorrectString_WhenBoldTagProvided");
            yield return new TestCaseData("__ a__", "__ a__")
                .SetName("ReturnCorrectString_WhenBoldTagOpensIncorrectly");
            yield return new TestCaseData("__a __", "__a __")
                .SetName("ReturnCorrectString_WhenBoldTagClosesIncorrectly");
            yield return new TestCaseData("_ a_", "_ a_")
                .SetName("ReturnCorrectString_WhenItalicTagOpensIncorrectly");
            yield return new TestCaseData("_a _", "_a _")
                .SetName("ReturnCorrectString_WhenItalicTagClosesIncorrectly");

            yield return new TestCaseData("a_a a_a", "a_a a_a")
                .SetName("ReturnCorrectString_WhenItalicTagInsideDifferentWords");
            yield return new TestCaseData("a__a a__a", "a__a a__a")
                .SetName("ReturnCorrectString_WhenBoldTagInsideDifferentWords");
            yield return new TestCaseData("1__1__1", "1__1__1")
                .SetName("ReturnCorrectString_WhenBoldTagInsideTextWithDigits");
            yield return new TestCaseData("1_1_1", "1_1_1")
                .SetName("ReturnCorrectString_WhenItalicTagInsideTextWithDigits");

            yield return new TestCaseData("_a", "_a")
                .SetName("ReturnCorrectString_WhenUnpairedItalicTagProvided");
            yield return new TestCaseData("__a", "__a")
                .SetName("ReturnCorrectString_WhenThereIsUnpairedBoldTag");
            yield return new TestCaseData("__a_a__a_", "__a_a__a_")
                .SetName("ReturnCorrectString_WhenTagsIntersect");
            yield return new TestCaseData("__a\na__a", "__a\na__a")
                .SetName("ReturnCorrectString_WhenTagsCloseOnNewline");
            yield return new TestCaseData("# a\n", "<h1>a</h1>\n")
                .SetName("ReturnCorrectString_WhenInputContainsHeaderWithNewLineChar");

            yield return new TestCaseData("_wo_rd", "<em>wo</em>rd")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");
            yield return new TestCaseData("w_o_rd", "w<em>o</em>rd")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");
            yield return new TestCaseData("wo_rd_", "wo<em>rd</em>")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");

            yield return new TestCaseData("__wo__rd", "<strong>wo</strong>rd")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");
            yield return new TestCaseData("w__o__rd", "w<strong>o</strong>rd")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");
            yield return new TestCaseData("wo__rd__", "wo<strong>rd</strong>")
                .SetName("ReturnCorrectString_WhenPartOfWordEmphasised");

            yield return new TestCaseData("![aba](caba)", "<img src=\"aba\" alt=\"caba\">")
                .SetName("ReturnLinkToken_WhenLinkToken");
            yield return new TestCaseData("![#a__b__a](c_a_ba)", "<img src=\"#a__b__a\" alt=\"c_a_ba\">")
                .SetName("ReturnLinkToken_WhenLinkTokenContainsTokens");

            yield return new TestCaseData("\\__a_", "__a_")
                .SetName("ReturnCorrectString_WhenEscapedTagsProvided");
            yield return new TestCaseData("# Text___with_different__tags\\__![and](img)",
                    "<h1>Text<strong><em>with</em>different</strong>tags__<img src=\"and\" alt=\"img\"></h1>\n")
                .SetName("ReturnCorrectString_WhenStringContainsMultipleTags");
        }
    }
}