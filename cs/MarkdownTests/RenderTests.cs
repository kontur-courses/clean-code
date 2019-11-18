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
        public string Render_ShouldRenderCorrectly1(string text)
        {
            return new Md().Render(text);
        }
    }
}
