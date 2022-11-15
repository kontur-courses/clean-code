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
    internal class MdShould
    {
        [TestCase("_text_", "<em>text</em>",TestName = "Simple em test with one tag")]
        [TestCase("_text_ _text_", "<em>text</em> <em>text</em>", TestName = "Simple em test with two tags in row")]
        [TestCase("__txt_ __text_", "<em>text</em> <em>text</em>", TestName = "Simple eeem test with two tags in row")]
        public void EmTagTests(string mdString,string htmlString)
        {
            var md = new Md();
            var result = md.Render(mdString);
            result.Should().Be(htmlString);
        }
    }
}
