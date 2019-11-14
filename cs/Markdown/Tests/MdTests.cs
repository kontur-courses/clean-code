using Markdown.Core;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class MdTests
    {
        [TestCase("_foo bar_", ExpectedResult = "<em>foo bar</em>", TestName = "When_AtBeginningAndEndText")]
        [TestCase("fo _o b_ ar", ExpectedResult = "fo <em>o b</em> ar", TestName = "When_InMiddleText")]
        [TestCase("_fo _o b_ ar_", ExpectedResult = "<em>fo <em>o b</em> ar</em>", TestName = "When_SeveralTimesInText")]
        public string Render_ReplacedEm(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase("__foo bar__", ExpectedResult = "<strong>foo bar</strong>", TestName = "When_AtBeginningAndEndText")]
        [TestCase("fo __o b__ ar", ExpectedResult = "fo <strong>o b</strong> ar", TestName = "When_InMiddleText")]
        [TestCase("__fo __o b__ ar__", ExpectedResult = "<strong>fo <strong>o b</strong> ar</strong>", TestName = "When_SeveralTimesInText")]
        public string Render_ReplacedStrong(string rawText)
        {
            return Md.Render(rawText);
        }
    }
}
