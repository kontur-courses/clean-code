using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Interfaces;
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
        [TestCase("_For each_", ExpectedResult = "<em>For each</em>", TestName = "Single selection is used")]
        [TestCase("__new day__", ExpectedResult = "<strong>new day</strong>", TestName = "Double selection is used")]
        [TestCase(@"I \_will\_ pay", ExpectedResult = "I _will_ pay", TestName = "Used escape character for single selection")]
        [TestCase(@"I _will\_ pay_", ExpectedResult = "I <em>will_ pay</em>", TestName = "Used escape character in single selection")]
        [TestCase(@"I \_\_will\_\_ pay", ExpectedResult = "I __will__ pay", TestName = "Used escape character for double selection")]
        [TestCase(@"\I \will\ pay\", ExpectedResult = @"\I \will\ pay\", TestName = "Used escape character without selection")]
        [TestCase(@"_I_ __will__ pay", ExpectedResult = @"<em>I</em> <strong>will</strong> pay", TestName = "Single and double selections is used")]
        [TestCase(@"I wi_ll p_ay", ExpectedResult = @"I wi_ll p_ay", TestName = "Underscore between letters is used")]

        public string Render_Should_renderingStringToHtmlCode_When(string input)
        {
            return md.Render(input);
        }
    }
}