using System.Collections.Generic;
using System.IO;
using System.Linq;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        public Md Markdown;
        [SetUp]
        public void Setup()
        {
            Markdown = new Md();
        }

        [TestCaseSource(nameof(RenderData))]
        public void Markdown_Render(string text, string expected)
        {
            var actual = Markdown.Render(text);
            
            Assert.That(actual, Is.EqualTo(text));
        }

        private static IEnumerable<TestCaseData> RenderData()
        {
            (string testName, string text, string expected)[] testData = new[]
            {
                ("No style", "single", "single"),
                ("Angled", "_text_", "<em>text</em>"),
                ("Bold", "__text__", "<strong>text</strong>"),
            };
            
            return testData.Select(test
                => new TestCaseData(test.text, test.expected) {TestName = test.testName});
        }
    }
}