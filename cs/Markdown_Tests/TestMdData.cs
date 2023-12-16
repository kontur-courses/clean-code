using NUnit.Framework;

namespace Markdown_Tests
{
    public static class TestMdData
    {
        public static IEnumerable<TestCaseData> ItalicText()
        {
            yield return new TestCaseData("_a_", "<em>a</em>")
                .SetName("Starts_And_Ends_With_Tag");
            yield return new TestCaseData(
                "_some text to make italic_",
                "<em>some text to make italic</em>"
            ).SetName("Starts_And_Ends_With_Tag_Long");
            yield return new TestCaseData(
                "_aboba walks_ and _biba runs_",
                "<em>aboba walks</em> and <em>biba runs</em>"
            ).SetName("Two_Italic_Parts");
        }


        public static IEnumerable<TestCaseData> BoldText()
        {
            yield return new TestCaseData("__b__", "<strong>b</strong>")
                .SetName("Starts_And_Ends_With_Tag");
            yield return new TestCaseData(
                "__some text to make bold__",
                "<strong>some text to make bold</strong>"
            ).SetName("Starts_And_Ends_With_Tag_Long");
            yield return new TestCaseData(
                "__aboba walks__ and __biba runs__",
                "<strong>aboba walks</strong> and <strong>biba runs</strong>"
            ).SetName("Two_Bold_Parts");
        }
    }
}
