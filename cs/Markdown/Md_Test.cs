using System;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class Md_Test
    {
        [TestCase("asdfg", ExpectedResult = "asdfg")]
        [TestCase("_asdfg_", ExpectedResult = "<em>asdfg</em>")]
        [TestCase("__asdfg__", ExpectedResult = "<strong>asdfg</strong>")]
        [TestCase("__aaa _ddd_ sss__", ExpectedResult = "<strong>aaa <em>ddd</em> sss</strong>")]
        [TestCase("_aaa __ddd__ sss_", ExpectedResult = "<em>aaa __ddd__ sss</em>")]
        [TestCase("aaaaa_12_3", ExpectedResult = "aaaaa_12_3")]
        [TestCase("__aaaaa_", ExpectedResult = "__aaaaa_")]
        [TestCase("_aaaaa__", ExpectedResult = "_aaaaa__")]
        [TestCase("_ aaaaa_", ExpectedResult = "_ aaaaa_")]
        [TestCase("_aaaaa _", ExpectedResult = "_aaaaa _")]
        [TestCase(@"\_asdfg_", ExpectedResult = "_asdfg_")]
        public string Render_SimleCorrectString(string mdText)
        {
            var md = new Md();
            var result = md.Render(mdText);
            return result;
        }
    }
}
