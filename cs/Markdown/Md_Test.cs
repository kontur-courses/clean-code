using System;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class Md_Test
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("asdfg", ExpectedResult = "asdfg")]
        [TestCase("_asdfg_", ExpectedResult = "<em>asdfg</em>")]
        [TestCase("__asdfg__", ExpectedResult = "<strong>asdfg</strong>")]
        public string Render_OpeningAndClosingTagMatch(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase("__aaa _ddd_ sss__", ExpectedResult = "<strong>aaa <em>ddd</em> sss</strong>")]
        [TestCase("_aaa __ddd__ sss_", ExpectedResult = "<em>aaa __ddd__ sss</em>")]
        public string Render_TagPairInsideAnotherPair(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase("aaaaa_12_3", ExpectedResult = "aaaaa_12_3")]
        public string Render_TagBetweenNumbers(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase("__aaaaa_", ExpectedResult = "__aaaaa_")]
        [TestCase("_aaaaa__", ExpectedResult = "_aaaaa__")]
        public string Render_UnpairedTags(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase("_aaa __ddd_ sss__", ExpectedResult = "<em>aaa __ddd</em> sss__")]
        [TestCase("__aaa __dd sss__", ExpectedResult = "__aaa <strong>dd sss</strong>")]
        public string Render_IntersectionTagAreas(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase("_ aaaaa_", ExpectedResult = "_ aaaaa_")]
        [TestCase("_aaaaa _", ExpectedResult = "_aaaaa _")]
        public string Render_SpaceNextTag(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }

        [TestCase(@"\_asdfg_", ExpectedResult = "_asdfg_")]
        [TestCase(@"_asdfg\_", ExpectedResult = "_asdfg_")]
        [TestCase(@"asdfg\a", ExpectedResult = @"asdfg\a")]
        [TestCase(@"asdfg\", ExpectedResult = @"asdfg\")]
        [TestCase(@"_asd_\", ExpectedResult =@"_asd_\")]
        [TestCase(@"_asd__\_", ExpectedResult = @"_asd___")]
        public string Render_BackSlash(string mdText)
        {
            var result = md.Render(mdText);
            return result;
        }
    }
}
