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
            ("Process {0} inside word", "i{2}nsi{2}de", "i<{1}>nsi</{1}>de"),
            ("Ignore {0} inside of different words", "firs{2}t sec{2}ond", "firs{2}t sec{2}ond"),
            ("Process {0} in start of word", "{2}s{2}tart", "<{1}>s</{1}>tart"),
            ("Process {0} at end of word", "en{2}d{2}", "en<{1}>d</{1}>"),
            ("Ignore open {0} before Space char", "ignore{2} before{2} whitespace", "ignore{2} before{2} whitespace"),
            ("Ignore closing {0} after Space char", "ignore {2}after {2}whitespace", "ignore {2}after {2}whitespace"),
            ("Ignore empty {0}", "{2}{2}", "{2}{2}"),
        };

        public static IEnumerable<TestCaseData> BoldItalicInteractionTests => new[]
        {
            CreateTestCase("When Italic inside Bold wrap both",
                "__bold _italic_ bold__",
                "<strong>bold <em>italic</em> bold</strong>"),
            CreateTestCase("When Bold inside Italic treat ignore Bold",
                "_italic __bold__ italic_",
                "<em>italic __bold__ italic</em>"),
            CreateTestCase("Ignore unpaired Bold and Italic",
                "_italic __bold",
                "_italic __bold"),
            CreateTestCase("Ignore crossing Bold and Italic",
                "__crossing _should __be _ignored",
                "__crossing _should __be _ignored"),
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