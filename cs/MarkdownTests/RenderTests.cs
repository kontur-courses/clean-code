using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class RenderTests
    {
        [TestCase("ab__aa_b_a__", ExpectedResult = "ab<strong>aa<em>b</em>a</strong>")]
        [TestCase("ab__aaa__bb_b_", ExpectedResult = "ab<strong>aaa</strong>bb<em>b</em>")]
        [TestCase("ab_a aa_bb__b__", ExpectedResult = "ab<em>a aa</em>bb<strong>b</strong>")]
        [TestCase("ab_aa__b__a_", ExpectedResult = "ab<em>aa__b__a</em>")]
        [TestCase("_1_",ExpectedResult = "<em>1</em>")]
        [TestCase("1_1_ _2_",ExpectedResult = "1_1_ <em>2</em>")]
        [TestCase("__1__",ExpectedResult = "<strong>1</strong>")]
        [TestCase("1__1__ __2__",ExpectedResult = "1__1__ <strong>2</strong>")]
        [TestCase("____",ExpectedResult = "<em>__</em>")]
        [TestCase("[link] (some)", ExpectedResult = "[link] (some)")]
        public string Render_ShouldRenderCorrectly1(string text)
        {
            return new Md().Render(text);
        }
    }
}
