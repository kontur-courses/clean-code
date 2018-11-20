using System;
using System.Diagnostics;
using System.Text;
using Markdown.Properties;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdTesting
    {
        private Md md;
        [SetUp]
        public void Init()
        {
            md = new Md();
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
        public string Render_Should_renderingStringToHtmlCode_When(string input)
        {
            return md.Render(input);
        }

        [Test]
        public void Render_Should_HandleLargeTextCorrectly()
        {
            var content = Resources.FastTest;
            var expectedResult = Resources.FastTestResult;
            var result = md.Render(content);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Render_Should_BeFast()
        {
            var content = Resources.FastTest;
            var time1 = Measure(content, ReadContent);
            var time2 = Measure(content, md.Render);

            Console.WriteLine(@"Text reading time: {0} ms", time1.Milliseconds);
            Console.WriteLine(@"Render time: {0} ms", time2.Milliseconds); ;
            Assert.Less(time2.TotalMilliseconds, 10 * time1.TotalMilliseconds, "Render is too slow!");
        }

        private TimeSpan Measure(string content, Func<string, string> action)
        {
            var timer = Stopwatch.StartNew();
            action(content);
            timer.Stop();
            return timer.Elapsed;
        }

        private string ReadContent(string content)
        {
            char Action(int i) => content[i];
            var builder = new StringBuilder();
            var result = "";
            for (int i = content.Length - 1; i > -1; i--)
            {
                if (i % 100 == 0)
                {
                    builder.Append(result + Action(i));
                    result = "";
                }
                result += Action(i);
            }

            return builder.Append(result).ToString();
        }
    }
}