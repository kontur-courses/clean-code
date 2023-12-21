using NUnit.Framework;

namespace Markdown_Tests
{
    public static class TestMdData
    {
        public static IEnumerable<TestCaseData> TestTextWithTags()
        {
            yield return new TestCaseData("_a_", "<em>a</em>")
                .SetName("Starts_And_Ends_With_Italic_Tag");
            yield return new TestCaseData(
                "_some text to make italic_",
                "<em>some text to make italic</em>"
            ).SetName("Starts_And_Ends_With_Italic_Tag_Long");
            yield return new TestCaseData(
                "_aboba walks_ and _biba runs_",
                "<em>aboba walks</em> and <em>biba runs</em>"
            ).SetName("Two_Italic_Parts");
            yield return new TestCaseData("__b__", "<strong>b</strong>")
                .SetName("Starts_And_Ends_With_Bold_Tag");
            yield return new TestCaseData(
                "__some text to make bold__",
                "<strong>some text to make bold</strong>"
            ).SetName("Starts_And_Ends_With_Bold_Tag_Long");
            yield return new TestCaseData(
                "__aboba walks__ and __biba runs__",
                "<strong>aboba walks</strong> and <strong>biba runs</strong>"
            ).SetName("Two_Bold_Parts");
            yield return new TestCaseData("# aboba", "<h1>aboba</h1>").SetName("One_Header_Tag");
            yield return new TestCaseData(
                "# aboba# aboba# aboba# aboba# aboba# aboba",
                "<h1>aboba# aboba# aboba# aboba# aboba# aboba</h1>"
            ).SetName("Several_HeaderTags_One_Parsed");
            yield return new TestCaseData("\\_aboba\\_", "_aboba_").SetName("Escaped_Tags");
            yield return new TestCaseData("__aboba_", "__aboba_").SetName("Escaped_Not_Paired_Tags");
            yield return new TestCaseData("_aboba_4_", "<em>aboba_4</em>")
                .SetName("Escaped_Underlining_Word_With_Number");
            
            yield return new TestCaseData("_abo_ba", "<em>abo</em>ba")
                .SetName("Italic_Tag_In_Beginning_And_Inside_Same_Word");
            yield return new TestCaseData("ab_o_ba", "ab<em>o</em>ba")
                .SetName("Italic_Tag_Inside_Same_Word");
            yield return new TestCaseData("abo_ba_", "abo<em>ba</em>")
                .SetName("Italic_Tag_In_End_And_Inside_Same_Word");
            yield return new TestCaseData("ab_o b_a", "ab_o b_a")
                .SetName("Italic_Tag_Inside_Different_Words_Escaped");
            yield return new TestCaseData("a__ b_ _o __ba", "a__ b_ _o __ba")
                .SetName("Highlight_Starts_After_Non_Space_Ends_Before_Non_Space_Escaped");
            yield return new TestCaseData("_a __bob_ a__", "_a __bob_ a__")
                .SetName("Escaped_Bold_Tag_Intersects_Italic");
            yield return new TestCaseData("_a __bob__ a_", "_a __bob__ a_")
                .SetName("Escaped_Bold_Tag_Inside_Italic");
            yield return new TestCaseData(
                "# Заголовок __с _разными_ символами__",
                "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
            ).SetName("Complex_Situation");
        }
    }
}
