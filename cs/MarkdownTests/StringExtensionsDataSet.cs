using Markdown.Tags;

namespace MarkdownTests;

public class StringExtensionsDataSet
{
    public static IEnumerable<TestCaseData> IsTagCases
    {
        get
        {
            yield return new TestCaseData("_a_", new EmTag(), 0, true, true)
                .SetName("True_When_IsOpenTag");
            yield return new TestCaseData("_a_", new EmTag(), 2, false, true)
                .SetName("True_When_IsCloseTag");
            yield return new TestCaseData("_a_", new EmTag(), 1, true, false)
                .SetName("False_When_NotTag");
            yield return new TestCaseData("_a_", new EmTag(), 2, true, false)
                .SetName("False_When_NotOpenTag");
            yield return new TestCaseData("__a__", new EmTag(), 0, true, false)
                .SetName("False_When_StrongInsteadOfEm");
            yield return new TestCaseData("a_a_", new EmTag(), 1, true, false)
                .SetName("False_When_BeforeOpenTagLetterOrDigit");
        }
    }
}