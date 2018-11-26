using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class StrongRegister_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("**foo bar**", ExpectedResult = "<p><strong>foo bar</strong></p>")]
        [TestCase("foo**bar**", ExpectedResult = "<p>foo<strong>bar</strong></p>")]
        [TestCase("__foo bar__", ExpectedResult = "<p><strong>foo bar</strong></p>")]
        [TestCase("__foo, __bar__, baz__", ExpectedResult = "<p><strong>foo, <strong>bar</strong>, baz</strong></p>")]
        [TestCase("__some__", ExpectedResult = "<p><strong>some</strong></p>")]
        public string BeWithStrongTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("5__6__78", ExpectedResult = "<p>5__6__78</p>")]
        [TestCase("__ foo bar__", ExpectedResult = "<p>__ foo bar__</p>")]
        [TestCase("** foo bar**", ExpectedResult = "<p>** foo bar**</p>")]
        public string BeWithoutStrongTag(string input)
        {
            return parser.Render(input);
        }
    }
}