using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class MarkdownShould
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("\n", ExpectedResult = "")]
        [TestCase("            some\n       another\n                 text", ExpectedResult = "<p>some\nanother\ntext</p>")]
        [TestCase("aaa\nbbb\n\nccc\nddd", ExpectedResult = "<p>aaa\nbbb</p>\n<p>ccc\nddd</p>")]
        [TestCase("some\ndiff\n\nlines", ExpectedResult = "<p>some\ndiff</p>\n<p>lines</p>")]
        public string ShouldBeWithParagraphTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("**foo bar**", ExpectedResult = "<p><strong>foo bar</strong></p>")]
        [TestCase("foo**bar**", ExpectedResult = "<p>foo<strong>bar</strong></p>")]
        [TestCase("__foo bar__", ExpectedResult = "<p><strong>foo bar</strong></p>")]
        [TestCase("__foo, __bar__, baz__", ExpectedResult = "<p><strong>foo, <strong>bar</strong>, baz</strong></p>")]
        [TestCase("__some__", ExpectedResult = "<p><strong>some</strong></p>")]
        public string ShouldBeWithStrongTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("5__6__78", ExpectedResult = "<p>5__6__78</p>")]
        [TestCase("__ foo bar__", ExpectedResult = "<p>__ foo bar__</p>")]
        [TestCase("** foo bar**", ExpectedResult = "<p>** foo bar**</p>")]
        public string ShouldNotBeWithStrongTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("*foo bar*", ExpectedResult = "<p><em>foo bar</em></p>")]
        [TestCase("*(*foo*)*", ExpectedResult = "<p><em>(<em>foo</em>)</em></p>")]
        [TestCase("_(_foo_)_", ExpectedResult = "<p><em>(<em>foo</em>)</em></p>")]
        [TestCase("_foo bar_", ExpectedResult = "<p><em>foo bar</em></p>")]
        [TestCase("5*6*78", ExpectedResult = "<p>5<em>6</em>78</p>")]
        public string ShouldBeWithEmphasisTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("a * foo bar*", ExpectedResult = "<p>a * foo bar*</p>")]
        [TestCase("* a *", ExpectedResult = "<p>* a *</p>")]
        [TestCase("foo_bar_", ExpectedResult = "<p>foo_bar_</p>")]
        [TestCase("_foo*", ExpectedResult = "<p>_foo*</p>")]
        public string ShouldNotBeWithEmphasisTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase(), Timeout(2000)]
        public void PotentiallyLongRunningTest()
        {
            // TODO
        }

        [Test]
        public void ComplexTest()
        {
            parser.Render("_some __bike_ is__").Should().Be("<p><em>some <em><em>bike</em> is</em></em></p>");
        }
    }
}
