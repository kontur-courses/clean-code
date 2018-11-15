using System;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class MdTest
    {
        [TestCase("hello world", ExpectedResult = "hello world", 
            TestName = "leave string the same if no markdown elements")]
        [TestCase("_hello_", ExpectedResult = "<em>hello</em>",
            TestName = "wrap underscore with em tag")]
        [TestCase("__hello__", ExpectedResult = "<strong>hello</strong>",
            TestName = "wrap double underscore with strong tag")]
        [TestCase("_hello__", ExpectedResult = "_hello__",
            TestName = "not treat unpaired underscores as markup")]
        [TestCase("_ hello _", ExpectedResult = "_ hello _",
            TestName = "not convert invalid open and close underscores")]
        [TestCase("__._hello_.__", ExpectedResult = "<strong>.<em>hello</em>.</strong>",
            TestName = "allow em contain strong")]
        [TestCase("_.__hello__._", ExpectedResult = "_.<strong>hello</strong>._",
            TestName = "not allow strong contain em")]
        public string RenderShould(string markdown)
        {
            var md = new Md();
            return md.Render(markdown);
        }
    }
}
