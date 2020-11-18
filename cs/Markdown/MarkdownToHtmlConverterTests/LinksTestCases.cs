using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public class LinksTestCases
    {
        public static IEnumerable<TestCaseData> LinkTests => new[]
        {
            CreateTestCase("Can parse links",
                "<https://google.com/>",
                "<a href=\"https://google.com/\">https://google.com/</a>")
        };

        private static TestCaseData CreateTestCase(string testName, string input, string expected) =>
            new TestCaseData(input, expected) {TestName = testName};
    }
}