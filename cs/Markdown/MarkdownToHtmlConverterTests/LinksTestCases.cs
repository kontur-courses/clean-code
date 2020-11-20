using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public class LinksTestCases
    {
        public static IEnumerable<TestCaseData> LinkTests => new[]
        {
            CreateTestCase("Can parse simple links",
                "(https://google.com/)",
                "<a href=\"https://google.com/\">https://google.com/</a>"),
            CreateTestCase("Can parse links with replacement text",
                "[google](https://google.com/)",
                "<a href=\"https://google.com/\">google</a>"),
            CreateTestCase("Can parse links with text around",
                "abc [google](https://google.com/) def",
                "abc <a href=\"https://google.com/\">google</a> def"),
            CreateTestCase("Bold tag inside replacement text ignored",
                "[__google__](https://google.com/)",
                "<a href=\"https://google.com/\">__google__</a>"),
            CreateTestCase("Replacement text can contain whitespaces",
                "[a bc](https://google.com/)",
                "<a href=\"https://google.com/\">a bc</a>"),
        };

        private static TestCaseData CreateTestCase(string testName, string input, string expected) =>
            new TestCaseData(input, expected) {TestName = testName};
    }
}