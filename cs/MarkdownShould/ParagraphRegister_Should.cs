using NUnit.Framework;
using Markdown;

namespace MarkdownShould
{
    [TestFixture]
    public class ParagraphRegister_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("\n", ExpectedResult = "")]
        [TestCase("5__6__78", ExpectedResult = "<p>5__6__78</p>")]
        [TestCase("            some\n       another\n                 text", ExpectedResult = "<p>some\nanother\ntext</p>")]
        [TestCase("aaa\nbbb\n\nccc\nddd", ExpectedResult = "<p>aaa\nbbb</p>\n<p>ccc\nddd</p>")]
        [TestCase("some\ndiff\n\nlines", ExpectedResult = "<p>some\ndiff</p>\n<p>lines</p>")]
        [TestCase("__\nfoo bar__", ExpectedResult = "<p>__\nfoo bar__</p>")]
        [TestCase("a__\"foo\"__", ExpectedResult = "<p>__\"foo\"__</p>")]
        public string ShouldBeWithParagraphTag(string input)
        {
            return parser.Render(input);
        }
    }
}
