using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("# __d__ _dd_", "<h1> <strong>d</strong> <em>dd</em></h1>")]
        [TestCase("__d__", "<strong>d</strong>")]
        [TestCase("_d_", "<em>d</em>")]
        [TestCase("__d  _fa_ f__", "<strong>d  <em>fa</em> f</strong>")]
        [TestCase("_d  __fa__ f_", "<em>d  __fa__ f</em>")]
        [TestCase("_aa_aa", "<em>aa</em>aa")]
        [TestCase("aa_aa_", "aa<em>aa</em>")]
        [TestCase("aa_a_a", "aa<em>a</em>a")]
        [TestCase("a_aa _aa", "a_aa _aa")]
        [TestCase("a_3_aa aa", "a_3_aa aa")]
        [TestCase("a_1_", "a_1_")]
        [TestCase("a_ 1_", "a_ 1_")]
        [TestCase("_1_", "<em>1</em>")]
        [TestCase(@"\a_ 1_", @"\a_ 1_")]
        [TestCase(@"\a \_a_", @"\a _a_")]
        [TestCase(@"\a \\_a_", @"\a \<em>a</em>")]
        [TestCase("#_a \n\n a_\n\na", "<h1>_a </h1>\n\n a_\n\na")]
        [TestCase("_a \n\n\n a_\n\na", "_a \n\n\n a_\n\na")]
        public void Render_Should(string input, string exeptedOutput)
        {
            Md.Render(input).Should().Be(exeptedOutput);
        }
    }
}