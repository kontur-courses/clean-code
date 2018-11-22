using NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [NUnit.Framework.TestFixture]
    public class Markdown_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("", ExpectedResult="")]
        [TestCase("\n", ExpectedResult = "<p></p>")]
        [TestCase("some text", ExpectedResult= "<p>some text</p>")]
        [TestCase("some\ndiff\nlines", ExpectedResult = "<p>some</p>\n<p>diff</p>\n<p>lines</p>")]
        public string ShouldBeWithParagraphTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("__some__", ExpectedResult = "<p><strong>some</strong></p>")]
        public string ShouldBeWithStrongTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
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
        public string ShouldBeWithEmTag(string input)
        {
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("a * foo bar*", ExpectedResult = "<p>a * foo bar*</p>")]
        [TestCase("* a *", ExpectedResult = "<p>* a *</p>")]
        [TestCase("a*\"foo\"*", ExpectedResult = "<p>a*\"foo\"*</p>")]
        [TestCase("foo_bar_", ExpectedResult = "<p>foo_bar_</p>")]
        [TestCase("_foo*", ExpectedResult = "<p>_foo*</p>")]
        public string ShouldNotBeWithEmTag(string input)
        {
            return parser.Render(input);
        }

        //TODO Тест на производительность

        [NUnit.Framework.Test]
        public void ComplexTest()
        {
            parser.Render("_some __bike_ is__").Should().Be("<p><em>some <em><em>bike</em> is</em></em></p>");
        }
    }
}