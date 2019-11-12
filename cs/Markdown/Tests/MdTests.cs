using Markdown.Core;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class MdTests
    {
        [TestCase("_foo bar_", ExpectedResult = "<em>foo bar</em>", TestName = "When_AtBeginningAndEndText")]
        [TestCase("fo_o b_ar", ExpectedResult = "fo<em>o b</em>ar", TestName = "When_InMiddleText")]
        [TestCase("_fo_o b_ar_", ExpectedResult = "<em>fo</em>o b<em>ar</em>", TestName = "When_SeveralTimesInText")]
        public string Render_ReplacedEm(string rawText)
        {
            return Md.Render(rawText);
        }
    }
}
