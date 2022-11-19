using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdShould
    {

        [TestCase("# Заголовок __с _разными_ символами__\n",
            "\\<h1>Заголовок \\<strong>с \\<em>разными\\</em> символами\\</strong>\\</h1>",
            TestName = "Base test")]
        [TestCase("Заголовок _с __разными__ символами_", "Заголовок \\<em>с __разными__ символами\\</em>",
            TestName = "incorrect input")]

        public void EmTagTests(string mdString, string htmlString)
        {
            var result = Md.Render(mdString);
            result.Should().Be(htmlString);
        }

        [Test]
        public void Test()
        {
            var mdString = "# markdown _test_ sentence__\n";
            Md.Render(mdString).Should().Be("\\<h1>markdown \\<em>test\\</em> sentence__\\</h1>");
        }
    }
}
