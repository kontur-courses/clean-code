using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public static class GeneralTestCases
    {
        public static IEnumerable<TestCaseData> PrimitiveCases => new[]
        {
            CreateTestCase("Don't modify text",
                "just text here",
                "just text here"),
            CreateTestCase("Return empty string on empty input",
                string.Empty,
                string.Empty),
        };

        private static TestCaseData CreateTestCase(string testName, string expected, string actual) =>
            new TestCaseData(actual, expected) {TestName = testName};
    }
}