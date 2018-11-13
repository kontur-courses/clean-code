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
        [TestCase("_For each_", ExpectedResult = "<em>For each</em>", TestName = "One selection is used")]
        [TestCase("__new day__", ExpectedResult = "<strong>new day</strong>", TestName = "Double selection is used")]
        [TestCase(@"I \_will\_ pay", ExpectedResult = "I _will_ pay", TestName = "Used escape character for single selection")]
        public string Render_Should_renderingStringToHtmlCode_When(string input)
        {
            return md.Render(input);
        }
    }
}