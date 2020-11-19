using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public static class BoldItalicTestCases
    {
        public static IEnumerable<TestCaseData> BoldTests => GeneralUnderscoreTests
            .Select(x => CreateTestCase(x, "Bold", "strong", "__"));

        public static IEnumerable<TestCaseData> ItalicTests => GeneralUnderscoreTests
            .Select(x => CreateTestCase(x, "Italic", "em", "_"));

        private static IEnumerable<(string TestName, string Input, string Expected)> GeneralUnderscoreTests => new[]
        {
            ("Wrap {0} in <{1}>", "{2}{0}{2}", "<{1}>{0}</{1}>"),
            ("Ignore {0} inside digits", "12{2}3{2}4", "12{2}3{2}4"),
            ("Process {0} inside word", "a{2}bc{2}d", "a<{1}>bc</{1}>d"),
            ("Ignore {0} inside of different words", "a{2}b c{2}d", "a{2}b c{2}d"),
            ("Process {0} at borders of different words", "{2}ab cd{2}", "<{1}>ab cd</{1}>"),
            ("Process {0} in start of word", "{2}ab{2}cd", "<{1}>ab</{1}>cd"),
            ("Process {0} at end of word", "ab{2}c{2}", "ab<{1}>c</{1}>"),
            ("Ignore open {0} before Space char", "{2} a{2}b", "{2} a{2}b"),
            ("Ignore closing {0} after Space char", "{2}ab {2}", "{2}ab {2}"),
            ("Ignore empty {0}", "{2}{2}", "{2}{2}"),
            ("Opening {0} can be screened", @"\{2}abc{2}", "{2}abc{2}"),
            ("Closing {0} can be screened", @"{2}abc\{2}", "{2}abc{2}"),
            ("Screening char in middle of {0} content doesn't affect", @"{2}ab\c{2}", @"<{1}>ab\c</{1}>"),
            ("Ignore {0} in different lines", "{2}a\r\nc{2}", "{2}a\r\nc{2}"),
        };

        public static IEnumerable<TestCaseData> ScreeningTests => new[]
        {
            CreateTestCase("Two screening chars at row doesnt screen following",
                @"\\__abc__",
                @"\<strong>abc</strong>"),
            CreateTestCase("Screening before word char should be printed",
                @"\abc",
                @"\abc"),
            CreateTestCase("Screening before digit should be printed",
                @"\123",
                @"\123")
        };

        public static IEnumerable<TestCaseData> BoldItalicInteractionTests => new[]
        {
            CreateTestCase("When Italic inside Bold wrap both",
                "__ab _cd_ ef__",
                "<strong>ab <em>cd</em> ef</strong>"),
            CreateTestCase("When Bold inside Italic treat ignore Bold",
                "_ab __cd__ ef_",
                "<em>ab __cd__ ef</em>"),
            CreateTestCase("Ignore unpaired Bold and Italic",
                "_ab __cd",
                "_ab __cd"),
            CreateTestCase("Ignore crossing Bold and Italic (bold first)",
                "__ab _cd ef__ gh",
                "__ab _cd ef__ gh"),
            CreateTestCase("Ignore crossing Bold and Italic (italic first)",
                "_ab __cd ef_ gh__",
                "_ab __cd ef_ gh__"),
            CreateTestCase("Single closing italic inside bold printed as text",
                "__abc d_ efg__",
                "<strong>abc d_ efg</strong>"),
            CreateTestCase("Single closing bold inside italic printed as text",
                "_abc d__ efg_",
                "<em>abc d__ efg</em>"),
            CreateTestCase("Single opening italic inside bold printed as text",
                "__abc _d efg__",
                "<strong>abc _d efg</strong>"),
            CreateTestCase("Single opening bold inside italic printed as text",
                "_abc __d efg_",
                "<em>abc __d efg</em>"),
            CreateTestCase("Screened opening Bold and closing Italic printed as Italic with underscore before",
                @"\__abc_",
                @"_<em>abc</em>"),
        };

        private static TestCaseData CreateTestCase((string, string, string) raw, string subject, string tag,
            string underscore)
        {
            var (testName, rawInput, expected) = raw;
            return CreateTestCase(
                string.Format(testName, subject, tag, underscore),
                string.Format(rawInput, subject, tag, underscore),
                string.Format(expected, subject, tag, underscore));
        }

        private static TestCaseData CreateTestCase(string testName, string input, string expected) =>
            new TestCaseData(input, expected) {TestName = testName};
    }
}