using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class MdTests
    {
        [TestCase("_This is a simple test_", ExpectedResult = "<em>This is a simple test</em>")]
        [TestCase("__This is a simple test__", ExpectedResult = "<strong>This is a simple test</strong>")]
        [TestCase("_a __abc__", ExpectedResult = "_a <strong>abc</strong>")]
        public string Render(string markedParagraph)
        {
            return Md.Render(markedParagraph);
        }
    }
}
