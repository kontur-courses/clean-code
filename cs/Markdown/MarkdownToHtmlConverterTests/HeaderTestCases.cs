using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MarkdownToHtmlConverterTests
{
    public class HeaderTestCases
    {
        public static IEnumerable<TestCaseData> HeaderTests => new[]
        {
            CreateTestCase("Header can be parsed", 
                "#abc",
                "<h1>abc</h1>"),
            CreateTestCase("Header with following whitespace can be parsed", 
                "# abc",
                "<h1> abc</h1>"),
            CreateTestCase("Header not at start of line should be ignored", 
                "abc #def",
                "abc #def"),
            CreateTestCase("Header can contain Bold inside", 
                "#abc __some__ text",
                "<h1>abc <strong>some</strong> text</h1>"),
            CreateTestCase("Header can contain Italic inside", 
                "#abc _some_ text",
                "<h1>abc <em>some</em> text</h1>"),
            CreateTestCase("Header applied to containing line only", 
                $"#line1{Environment.NewLine}line2",
                $"<h1>line1</h1>{Environment.NewLine}line2"),
        };
        
        private static TestCaseData CreateTestCase(string testName, string input, string expected) =>
            new TestCaseData(input, expected) {TestName = testName};
    }
}