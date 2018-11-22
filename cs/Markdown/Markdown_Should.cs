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

        [TestCase("")]
        [TestCase("Some text with some _wooords_ !")]
        [TestCase("__hahaha__")]
        public void ShouldBeSameString_WhenNoRegisters(string str)
        {
            parser.Render(str).Should().Be(str);
        }

        [TestCase("", ExpectedResult="")]
        [TestCase("\n", ExpectedResult = "<p></p>")]
        [TestCase("some text", ExpectedResult= "<p>some text</p>")]
        [TestCase("some\ndiff\nlines", ExpectedResult = "<p>some</p>\n<p>diff</p>\n<p>lines</p>")]
        public string ShouldBeWithParagraphTag(string input)
        {
            parser.registerGlobalReader(new ParagraphRegister());

            return parser.Render(input);
        }

        [TestCase("__some__", ExpectedResult = "<strong>some</strong>")]
        public string ShouldBeWithStrongTag(string input)
        {
            parser.registerLocalReader(new StrongRegister());
            return parser.Render(input);
        }

        [TestCase("", ExpectedResult = "")]
        public string ShouldNotBeWithStrongTag(string input)
        {
            parser.registerLocalReader(new StrongRegister());
            return parser.Render(input);
        }

        [Test]
        public string ShouldBeWithEmTag(string input)
        {
            return null;
        }

        [Test]
        public string ShouldNotBeWithEmTag(string input)
        {
            return null;
        }

        //TODO Тест на производительность

        [NUnit.Framework.Test]
        public void ComplexTest()
        {
            parser.registerGlobalReader(new ParagraphRegister());
            parser.registerGlobalReader(new HorLineRegister());

            parser.registerLocalReader(new StrongRegister());
            parser.registerLocalReader(new EmRegister());

            parser.Render("_some __bike_ is__").Should().Be("<p><em>some <em><em>bike</em> is</em></em></p>");
        }
    }
}