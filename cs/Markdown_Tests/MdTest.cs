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
            TestName = "allow em contain strong")]
        [TestCase("_hello __world__ !_", ExpectedResult = "_hello <strong>world</strong> !_",
            TestName = "not allow strong contain em")]
        [TestCase("___hello___", ExpectedResult = "___hello___",
            TestName = "not convert unrecognized paired underscores")]
        [TestCase("___hello__", ExpectedResult = "___hello__",
            TestName = "not convert unrecognized unpaired underscores")]
        public string RenderShould(string markdown)
        {
            var md = new Md();
            return md.Render(markdown);
        }
    }
}
