using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Markdown_should
    {
        [SetUp]
        public void Init()
        {
            markdownStringRenderer = new MarkdownStringRenderer();
        }

        private MarkdownStringRenderer markdownStringRenderer;


        [TestCase(@"__test__", ExpectedResult = "<strong>test</strong>", TestName = "Tag Strong works")]
        [TestCase(@"_test_", ExpectedResult = @"<em>test</em>", TestName = "Tag Em works")]
        public string TagRepresentationShouldWorksTests(string source)
        {
            return markdownStringRenderer.Render(source);
        }

        [TestCase(@"_ test_", ExpectedResult = @"_ test_",
            TestName = "Tag don't recognize as Open if next token is WhiteSpace")]
        [TestCase(@"__test __", ExpectedResult = @"__test __",
            TestName = "Tag don't recognize as Close if previous token is WhiteSpace")]
        [TestCase(@"__test_", ExpectedResult = "__test_", TestName = "Diferent tags didn't recognize")]
        [TestCase("_\ntest_", ExpectedResult = "_\ntest_", TestName = "\\n is WhiteSpace")]
        [TestCase("_\ttest_", ExpectedResult = "_\ttest_", TestName = "\\t is WhiteSpace")]
        public string WhiteSpacesTests(string source)
        {
            return markdownStringRenderer.Render(source);
        }

        [TestCase(@"__test _test__ab_", ExpectedResult = @"<strong>test _test</strong>ab_",
            TestName = "Tag collision test")]
        [TestCase(@"_te __test__ st_", ExpectedResult = @"<em>te __test__ st</em>",
            TestName = "Strong tag doesn't work in em tag")]
        [TestCase(@"__te _test_ st__", ExpectedResult = @"<strong>te <em>test</em> st</strong>",
            TestName = "Em tag works in strong tag")]
        public string TagCollisionTests(string source)
        {
            return markdownStringRenderer.Render(source);
        }

        [TestCase(@"1_1test_", ExpectedResult = "1_1test_", TestName = "Numbers near Tag")]
        [TestCase(@"1a_b1test_", ExpectedResult = "1a_b1test_",
            TestName = "Tag between text with number but number not near")]
        public string TagBetwinTextWithNumberTests(string source)
        {
            return markdownStringRenderer.Render(source);
        }

        [TestCase(@"_te\\st_", ExpectedResult = @"<em>te\st</em>", TestName = "Ecranate ecranator")]
        [TestCase(@"_\_test__", ExpectedResult = @"__test__", TestName = "\\ works on tag symbol")]
        [TestCase(@"_\_test_", ExpectedResult = @"<em>_test</em>", TestName = "\\ works on tag symbol 2")]
        [TestCase(@"_t\est_", ExpectedResult = @"<em>test</em>", TestName = "\\ works on not tag symbol")]
        public string EcranationTest(string source)
        {
            return markdownStringRenderer.Render(source);
        }


        [TestCase("__te _test_ st__")]
        [TestCase("_te _test_ _test_ st_")]
        [TestCase(" __test__ ")]
        public void TimeTests(string toAppend)
        {
            var timeSpans = new List<double>();
            var stringBuilder = new StringBuilder();
            var timer = new Stopwatch();
            for (int i = 0; i < 100; i++)
            {
                 
                stringBuilder.Append(toAppend);
                var str = stringBuilder.ToString();
                timer.Reset(); 
                timer.Start();
                markdownStringRenderer.Render(str);
                timeSpans.Add(timer.ElapsedMilliseconds/(double)stringBuilder.Length);
            }

            foreach (var ts in timeSpans)
                Console.WriteLine(ts);           
        }
    }
}