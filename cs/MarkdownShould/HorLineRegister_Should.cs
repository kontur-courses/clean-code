using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class HorLineRegister_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("***", ExpectedResult = "<hr />")]
        [TestCase("---", ExpectedResult = "<hr />")]
        [TestCase("___", ExpectedResult = "<hr />")]
        [TestCase("   ***", ExpectedResult = "<hr />")]
        [TestCase("* ***** ***** ********", ExpectedResult = "<hr />")]
        public string BeWithHRTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("+++", ExpectedResult = "<p>+++</p>")]
        [TestCase("Foo\r\n    ***", ExpectedResult = "<p>Foo\r\n***</p>")]
        public string BeWithoutHRTag(string input)
        {
            return parser.Render(input);
        }
    }
}
