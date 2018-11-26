using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class EmphasisRegister_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("*foo bar*", ExpectedResult = "<p><em>foo bar</em></p>")]
        [TestCase("*(*foo*)*", ExpectedResult = "<p><em>(<em>foo</em>)</em></p>")]
        [TestCase("_(_foo_)_", ExpectedResult = "<p><em>(<em>foo</em>)</em></p>")]
        [TestCase("_foo bar_", ExpectedResult = "<p><em>foo bar</em></p>")]
        [TestCase("5*6*78", ExpectedResult = "<p>5<em>6</em>78</p>")]
        public string BeWithEmphasisTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("a * foo bar*", ExpectedResult = "<p>a * foo bar*</p>")]
        [TestCase("* a *", ExpectedResult = "<p>* a *</p>")]
        [TestCase("foo_bar_", ExpectedResult = "<p>foo_bar_</p>")]
        [TestCase("_foo*", ExpectedResult = "<p>_foo*</p>")]
        public string NotBeWithEmphasisTag(string input)
        {
            return parser.Render(input);
        }
    }
}
