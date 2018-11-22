using NUnit.Framework;

namespace MarkdownNew
{
    [TestFixture]
    class RenderTests
    {
        [TestCase("__w\nw__", TestName = "Md Render Should Transform __ Tag In Different Lines", ExpectedResult = "<strong>w\nw</strong>")]
        [TestCase("_w\nw_", TestName = "Md Render Should Transform _ Tag In Different Lines", ExpectedResult = "<em>w\nw</em>")]
        [TestCase("__w _w_ w__", TestName = "Md Render Should Transform _ Tag In __ Tag", ExpectedResult = "<strong>w <em>w</em> w</strong>")]
        [TestCase("__w_", TestName = "Md Render Should Not Transform Tags Without Pair", ExpectedResult = "__w_")]
        [TestCase("_1_2_", TestName = "Md Render Should Transform _ Tag With Numbers", ExpectedResult = "<em>1_2</em>")]
        [TestCase("__1__2__", TestName = "Md Render Should Not Transform __ Tag With Numbers", ExpectedResult = "<strong>1__2</strong>")]
        [TestCase("_w", TestName = "Md Render Should Not Transform _ Tag Without Pair", ExpectedResult = "_w")]
        [TestCase("__w", TestName = "Md Render Should Not Transform __ Tag Without Pair", ExpectedResult = "__w")]
        [TestCase("__ w__", TestName = "Md Render Should Not Transform __ Tag With Space After First", ExpectedResult = "__ w__")]
        [TestCase("_wow_", TestName = "Md Render Should Work With \"_\" Tag", ExpectedResult = "<em>wow</em>")]
        [TestCase("__w__", TestName = "Md Render Should Work With \"__\" Tag", ExpectedResult = "<strong>w</strong>")]
        public string test(string mdString)
        {
            var actual = MarkdownRender.Render(mdString);
            return actual;
        }
    }
}
