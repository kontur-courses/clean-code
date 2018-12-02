using System.Collections.Generic;
using System.Linq;
using Markdown;
using NUnit.Framework;
using BenchmarkDotNet.Running;
using Markdown.MarkdownConfigurations;

namespace MdTesting
{
    [TestFixture]
    public class RenderTesting
    {
        private Md md;

        [SetUp]
        public void Init()
        {
            md = new Md(new HtmlConfig());
        }

        [TestCase("_For each_", ExpectedResult = "<em>For each</em>", TestName = "Single underscore is used")]
        [TestCase("__new day__", ExpectedResult = "<strong>new day</strong>", TestName = "Double underscore is used")]
        [TestCase(@"I \_will\_ pay", ExpectedResult = "I _will_ pay", TestName = "Used escape character for single underscore")]
        [TestCase(@"I _will\_ pay_", ExpectedResult = "I <em>will_ pay</em>", TestName = "Used escape character in single underscore")]
        [TestCase(@"I \_\_will\_\_ pay", ExpectedResult = "I __will__ pay", TestName = "Used escape character for double underscore")]
        [TestCase(@"\I \will\ pay\", ExpectedResult = @"\I \will\ pay\", TestName = "Used escape character without underscore")]
        [TestCase(@"_I_ __will__ pay", ExpectedResult = @"<em>I</em> <strong>will</strong> pay", TestName = "Single and double underscores is used")]
        [TestCase(@"I wi_ll p_ay", ExpectedResult = @"I wi_ll p_ay", TestName = "Underscore between letter or digit is used")]
        [TestCase(@"__I _will_ pay__", ExpectedResult = @"<strong>I <em>will</em> pay</strong>", TestName = "One underscore is enclosed in the two underscores")]
        [TestCase(@"I __will _pay", ExpectedResult = @"I __will _pay", TestName = "Unpaired underscore is used")]
        [TestCase(@"_I _will pay_", ExpectedResult = @"<em>I _will pay</em>", TestName = "Unpaired underscore in single underscore is used")]
        [TestCase(@"For_ each_ I _will _pay", ExpectedResult = @"For_ each_ I _will _pay", TestName = "Wrong underscore is used")]
        [TestCase("For _each\r\nI will_ pay", ExpectedResult = "For <em>each\r\nI will</em> pay", TestName = "Newline character is used")]
        [TestCase("For _each\r\n\r\nI _will_ pay", ExpectedResult = "For _each\r\n\r\nI <em>will</em> pay", TestName = "New paragraph character is used")]
        [TestCase("\t\t\t_I will_\r\n\r\n    ", ExpectedResult = "\t\t\t<em>I will</em>\r\n\r\n    ", TestName = "Control characters is used")]
        [TestCase("____I will____", ExpectedResult = "____I will____", TestName = "4 underscores are used")]
        [TestCase("___I will___", ExpectedResult = "<strong><em>I will</em></strong>", TestName = "3 underscores are used")]
        public string Render_Should_renderingStringToHtmlCode_When(string input)
        {
            return md.Render(input);
        }

        [Test]
        public void Render_Should_HandleLargeTextCorrectly()
        {
            var content = Properties.Resources.FastTest;
            var expectedResult = Properties.Resources.FastTestResult;
            var result = md.Render(content);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Render_Should_BeFast()
        {
            var summary = BenchmarkRunner.Run<BenchmarkRender>(new BenchmarkConfig());
            var meanResults = summary.Reports.Select(report => report.ResultStatistics?.Mean ?? 10).ToArray();
            var result = new List<double>();
            for (int i = 0; i < meanResults.Length - 1; i++)
                result.Add(meanResults[i] / meanResults[i + 1]);

            result.ForEach(e => Assert.Less(e, 4));
        }
    }
}
