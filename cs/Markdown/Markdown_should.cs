
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class Markdown_should
    {
        private Markdown markdown;

        [SetUp]
        public void Initialize()
        {
            markdown = new Markdown();
            markdown.AddNewTag("_", "em");
            markdown.AddNewTag("__", "strong");
        }

        [TestCase(@"_test__", ExpectedResult = "_test__")]
        [TestCase(@"_test_ __test__", ExpectedResult = "<em>test</em> <strong>test</strong>")]
        [TestCase(@"_p __test__ p_", ExpectedResult = "<em>p <strong>test</strong> p</em>")]
        [TestCase(@"\_test_",ExpectedResult = @"_test_")]
        [TestCase(@"_ test_",ExpectedResult = @"_ test_")]
        [TestCase(@"__test\___",ExpectedResult = @"<strong>test_</strong>")]
        [TestCase(@"test \\ \\",ExpectedResult = @"test \ \")]
        public string MarkdownTest(string input)
        {
            return markdown.Render(input);
        }

    }
}