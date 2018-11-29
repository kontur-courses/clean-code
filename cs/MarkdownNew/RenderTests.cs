using NUnit.Framework;

namespace MarkdownNew
{
    [TestFixture]
    class MarkdownRenderer
    {
        [TestCase("__w\nw__", ExpectedResult = "<strong>w\nw</strong>", TestName = "Transform __ Tag In Different Lines")]
        [TestCase("_w\nw_", ExpectedResult = "<em>w\nw</em>", TestName = "Transform _ Tag In Different Lines")]
        [TestCase("__w _w_ w__", ExpectedResult = "<strong>w <em>w</em> w</strong>", TestName = "Transform _ Tag In __ Tag")]
        [TestCase("__w_", ExpectedResult = "__w_", TestName = "Not Transform Tags Without Pair")]
        [TestCase("_1_2_", ExpectedResult = "<em>1_2</em>", TestName = "Transform _ Tag With Numbers")]
        [TestCase("__1__2__", ExpectedResult = "<strong>1__2</strong>", TestName = "Not Transform __ Tag With Numbers")]
        [TestCase("_w", ExpectedResult = "_w", TestName = "Not Transform _ Tag Without Pair")]
        [TestCase("__w", ExpectedResult = "__w", TestName = "Not Transform __ Tag Without Pair")]
        [TestCase("__ w__", ExpectedResult = "__ w__", TestName = "Not Transform __ Tag With Space After First")]
        [TestCase("_wow_", ExpectedResult = "<em>wow</em>", TestName = "Work With \"_\" Tag")]
        [TestCase("__w__", ExpectedResult = "<strong>w</strong>", TestName = "Work With \"__\" Tag")]
        public string Should(string mdString)
        {
            var actual = MarkdownRender.Render(mdString);
            return actual;
        }
    }
}
