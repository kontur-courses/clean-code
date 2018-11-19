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

        public string Render_Should_renderingStringToHtmlCode_When(string input)
        {
            return md.Render(input);
        }
    }
}