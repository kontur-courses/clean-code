using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
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
            var readers = ReaderCreator.Create();
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
        [TestCase("_simple text_", ExpectedResult = "<em>simple text</em>", TestName = "when sentence inner em tag")]

        [TestCase("__simple text__", ExpectedResult = "<strong>simple text</strong>", TestName = "when sentence inner strong tag")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", TestName = "when text have one strong tag")]
        [TestCase("__a", ExpectedResult = "__a", TestName = "when strong tag have not pair")]
        [TestCase("__ a__", ExpectedResult = "__ a__", TestName = "when after open strong tag have whitespace")]
        [TestCase("__a __", ExpectedResult = "__a __", TestName = "when before close strong tag have whitespace")]
        [TestCase("__", ExpectedResult = "__", TestName = "when text is only one not pair tag")]
        
        [TestCase("_\\_a_", ExpectedResult = "<em>_a</em>", TestName = "when text have slash tag inner em tag")]
        [TestCase("__\\_a__", ExpectedResult = "<strong>_a</strong>", TestName = "when text have slash tag inner strong tag")]
        [TestCase("\\__a_", ExpectedResult = "_<em>a</em>", TestName = "when in text using slash")]
        
        [TestCase("__a_b_c__", ExpectedResult = "<strong>a<em>b</em>c</strong>", TestName = "when em tag inner strong tag")]
        [TestCase("_a__b__c_", ExpectedResult = "<em>a__b__c</em>", TestName = "when strong tag inner em tag")]
        [TestCase("_a_ __b__", ExpectedResult = "<em>a</em> <strong>b</strong>", TestName = "when text have em and strong tags in line")]
        [TestCase("___e___", ExpectedResult = "<strong><em>e</em></strong>", TestName = "when em tag and strong tag stay together")]
        
        [TestCase("`simple code`", ExpectedResult = "<code>simple code</code>", TestName = "when use code tag")]
        [TestCase("_`simple code`_", ExpectedResult = "<em><code>simple code</code></em>", TestName = "when use code tag inner em tag")]
        [TestCase("__`simple code`__", ExpectedResult = "<strong><code>simple code</code></strong>", TestName = "when use code tag inner strong tag")]
        [TestCase("`_simple code_`", ExpectedResult = "<code>_simple code_</code>", TestName = "when em tag inner code tag")]
        [TestCase("`__simple code__`", ExpectedResult = "<code>__simple code__</code>", TestName = "when strong tag inner code tag")]
        public string Md_ShouldCorrectRenderText(string input)
        {
           return md.Render(input);
        }
    }
}
