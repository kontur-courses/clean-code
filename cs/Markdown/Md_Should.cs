using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("# __d__ sf", "<h1> <strong>d</strong> sf</h1>")]
        [TestCase("__d__", "<strong>d</strong>")]
        [TestCase("__d  _fa_ f__", "<strong>d  <em>fa</em> f</strong>")]
        [TestCase("_d  __fa__ f_", "<em>d  __fa__ f</em>")]
        [TestCase("_aa_aa", "<em>aa</em>aa")]
        [TestCase("a_1_", "a_1_")]
        [TestCase("a_ 1_", "a_ 1_")]
        [TestCase("\\a_ 1_", "\\a_ 1_")]
        [TestCase("\\a \\_a_", "\\a _a_")]
        public void Rander(string input, string exeptedOutput)
        {
            Md.Render(input).Should().Be(exeptedOutput);
        }
    }
}