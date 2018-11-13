using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Readers;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            SetupReaders.Setup();
            var readers = new IReader[]{ new SlashReader(), new StrongReader(), new EmReader(), new CharReader() };
            md = new Md(readers);
        }
      
        [TestCase("simple text", ExpectedResult = "simple text", TestName = "when text without tags")]
        [TestCase("_a_", ExpectedResult = "<em>a</em>", TestName = "when one tag em")]
        [TestCase("_a", ExpectedResult = "_a", TestName = "when em tag have not pair")]
        [TestCase("_1_", ExpectedResult = "_1_", TestName = "when text is digits")]
        [TestCase("_1a_", ExpectedResult = "_1a_", TestName = "when first symbol in text is digit")]
        [TestCase("_a1_", ExpectedResult = "_a1_", TestName = "when last symbol in text is digit")]
        [TestCase("_a1a_", ExpectedResult = "_a1a_", TestName = "when text have digit")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "when after open em tag have whitespace")]
        [TestCase("_a _", ExpectedResult = "_a _", TestName = "when before close em tag have whitespace")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", TestName = "when text have one strong tag")]
        [TestCase("__a", ExpectedResult = "__a", TestName = "when strong tag have not pair")]
        [TestCase("__ a__", ExpectedResult = "__ a__", TestName = "when after open strong tag have whitespace")]
        [TestCase("__a __", ExpectedResult = "__a __", TestName = "when before close strong tag have whitespace")]
        [TestCase("_\\_a_", ExpectedResult = "<em>_a</em>", TestName = "when text have slash tag inner em tag")]
        [TestCase("__\\_a__", ExpectedResult = "<strong>_a</strong>", TestName = "when text have slash tag inner strong tag")]
        [TestCase("__a_b_c__", ExpectedResult = "<strong>a<em>b</em>c</strong>", TestName = "when em tag inner strong tag")]
        [TestCase("_a__b__c_", ExpectedResult = "<em>a__b__c</em>", TestName = "when strong tag inner em tag")]
        [TestCase("_a_ __b__", ExpectedResult = "<em>a</em> <strong>b</strong>", TestName = "when text have em and strong tags in line")]
        [TestCase("_simple text_", ExpectedResult = "<em>simple text</em>", TestName = "when sentence inner em tag")]
        [TestCase("__simple text__", ExpectedResult = "<strong>simple text</strong>", TestName = "when sentence inner strong tag")]
        [TestCase("\\__a_", ExpectedResult = "_<em>a</em>", TestName = "when in text using slash")]
        [TestCase("__", ExpectedResult = "__", TestName = "when text is only one not pair tag")]
        public string Md_ShouldCorrectRenderText(string input)
        {
           return md.Render(input);
        }
    }
}
