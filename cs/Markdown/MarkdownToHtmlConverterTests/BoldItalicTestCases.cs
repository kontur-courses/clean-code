using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public static class BoldItalicTestCases
    {
        public static IEnumerable<TestCaseData> BoldTests => GeneralUnderscoreTests
            .Select(x => CreateTestCase(x, "Bold", "strong"));

        public static IEnumerable<TestCaseData> ItalicTests => GeneralUnderscoreTests
            .Select(x => CreateTestCase(x, "Italic", "em"));

        private static IEnumerable<(string TestName, string Input, string Expected)> GeneralUnderscoreTests => new[]
        {
            ("Wrap {0} in <{1}>", "__{0}__", "<{1}>{0}</{1}>"),
            ("Ignore {0} inside digits", "12__3__4", "12__3__4"),
            ("Process {0} inside word", "i__nsi__de", "i<{1}>nsi</{1}>de"),
            ("Ignore {0} inside of different words", "firs__t sec__ond", "firs__t sec__ond"),
            ("Process {0} in start of word", "__s__tart", "<{1}>s</{1}>tart"),
            ("Process {0} at end of word", "en__d__", "en<{1}>d</{1}>"),
            ("Ignore opening {0} before Space char", " ignore__ before__ whitespace", " ignore__ before__ whitespace"),
            ("Ignore closing {0} after Space char", " ignore __after __whitespace", " ignore __after __whitespace"),
            ("Ignore empty {0}", "____", "____"),
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

        private static TestCaseData CreateTestCase((string, string, string) raw, string subject, string tag)
        {
            var (testName, rawInput, expected) = raw;
            return CreateTestCase(
                string.Format(testName, subject, tag),
                string.Format(rawInput, subject, tag),
                string.Format(expected, subject, tag));
        }

        private static TestCaseData CreateTestCase(string testName, string input, string expected) =>
            new TestCaseData(input, expected) {TestName = testName};
    }
}