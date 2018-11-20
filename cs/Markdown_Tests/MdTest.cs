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
        [TestCase("__hello _world_ !__", ExpectedResult = "<strong>hello <em>world</em> !</strong>",
            TestName = "allow strong contain em")]
        [TestCase("_hello __world__ !_", ExpectedResult = "_hello <strong>world</strong> !_",
            TestName = "not allow em contain strong")]
        [TestCase("___hello___", ExpectedResult = "___hello___",
            TestName = "not convert unrecognized paired underscores")]
        [TestCase("___hello__", ExpectedResult = "___hello__",
            TestName = "not convert unrecognized unpaired underscores")]
        [TestCase(@"\_hello\_", ExpectedResult = "_hello_", 
            TestName = "not convert escaped underscores")]
        [TestCase(@"\\hello", ExpectedResult = @"\hello",
            TestName = "remover escape slash")]
        [TestCase(@"\__hello_", ExpectedResult = "_<em>hello</em>",
            TestName = "not convert to em when first underscore is escaped")]
        [TestCase(@"\__hello__", ExpectedResult = "__hello__",
            TestName = "not convert to strong when first underscore is escaped")]
        [TestCase("__hello _world__ !_", ExpectedResult = "<strong>hello _world</strong> !_",
            TestName = "markup strong and ignore em when underscores intersect")]
        [TestCase("do not123_recognize_", ExpectedResult = "do not123_recognize_",
            TestName = "ignore underscores inside word with digits")]
        public string RenderShould(string markdown)
        {
            var md = new Md();
            return md.Render(markdown);
        }
    }
}
