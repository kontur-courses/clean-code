using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class MdTests
    {
        [TestCase("_This is a simple test_", ExpectedResult = "<em>This is a simple test</em>")]
        [TestCase("__This is a simple test__", ExpectedResult = "<strong>This is a simple test</strong>")]
        [TestCase("_a __abc__", ExpectedResult = "_a <strong>abc</strong>")]
        [TestCase("a_ _abc__", ExpectedResult = "a_ _abc__")]
        [TestCase("a_ _abc_", ExpectedResult = "a_ <em>abc</em>")]
        [TestCase("a \n abc ", ExpectedResult = "a \n abc ")]
        [TestCase("___", ExpectedResult = "___")]
        [TestCase("_a __D__ b_", ExpectedResult = "<em>a __D__ b</em>")]

        public string Render(string markedParagraph)
        {
            return Md.Render(markedParagraph);
        }
    }
}
