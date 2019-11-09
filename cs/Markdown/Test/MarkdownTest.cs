using System.Collections.Generic;
using NUnit.Framework;

namespace Markdown.Test
{
    public class MarkdownTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TestCaseSource(nameof(FeatureNameSource))]
        public void Should_ParseParagraph_With__Feature(string paragraph, string result)
        {
        }

        private static IEnumerable<TestCaseData> FeatureNameSource = new List<TestCaseData>
        {
            new TestCaseData("", "")
        };
    }
}