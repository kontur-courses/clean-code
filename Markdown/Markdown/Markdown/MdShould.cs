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
        [TestCase("_text_ _ _text_", "<Italic>text</Italic>", TestName = "Simple Italic test with one tag")]
        [TestCase("_text\\_ _text_", "<Italic>text</Italic> <Italic>text</Italic>",
            TestName = "11Simple Italic test with two tags in row")]
        [TestCase("__txt_ __text_", "<Italic>text</Italic> <Italic>text</Italic>",
            TestName = "Simple eeem test with two tags in row")]
        [TestCase("__txt\\_ \\__te  xt_fef123_e 3_3", "<Italic>text</Italic> <Italic>text</Italic>",
            TestName = "Field")]
        [TestCase("_3  __", "<Italic>text</Italic> <Italic>text</Italic>", TestName = "digits")]
        
        public void EmTagTests(string mdString, string htmlString)
        {
            var result = Md.Render(mdString);
            result.Should().Be(htmlString);
        }

        [Test]
        public void Test()
        {
            var mdString = "# markdown _test_ sentence__";
            Md.Render(mdString).Should().Be("<h1>markdown <em>test</em> sentence</strong></h1>");
        }
    }
    
}
