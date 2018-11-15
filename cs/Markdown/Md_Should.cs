using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should // todo возможно стоит переписать на тест кейсы
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Return_EmptyString()
        {
            md.Render("").Should().Be("");
        }

        [Test]
        public void Return_OneItalicTeg()
        {
            md.Render("_word_").Should().Be("<em>word</em>");
        }

        [Test]
        public void Return_OneItalicTeg_WithTextOnSides()
        {
            md.Render("word _word_ word").Should().Be("word <em>word</em> word");
        }

        [Test]
        public void Return_MoreItalicTeg()
        {
            md.Render("word _word_ _word_ _word_ word").Should().Be("word <em>word</em> <em>word</em> <em>word</em> word");
        }

        [Test]
        public void Return_OneStrongTeg()
        {
            md.Render("__word__").Should().Be("<strong>word</strong>");
        }

        [Test]
        public void Return_OneStrongTeg_WithTextOnSides()
        {
            md.Render("word __word__ word").Should().Be("word <strong>word</strong> word");
        }

        [Test]
        public void Return_MoreStrongTeg()
        {
            md.Render("word __word__ __word__ __word__ word").Should().Be("word <strong>word</strong> <strong>word</strong> <strong>word</strong> word");
        }

        [Test]
        public void Return_NotClosedTag()
        {
            md.Render("word __word word").Should().Be("word __word word");
        }

        [Test]
        public void Return_NotOpenedTag()
        {
            md.Render("word word__ word").Should().Be("word word__ word");
        }

        [Test]
        public void Return_ItalicTegInStrongTeg()
        {
            md.Render("word __word _word_ word__ word").Should().Be("word <strong>word <em>word</em> word</strong> word");
        }

        [Test]
        public void Return_StrongTegInItalicTag()
        {
            md.Render("word _word __word__ word_ word").Should().Be("word <em>word __word__ word</em> word");
        }

        [Test]
        public void Return_NumbersThroughUnderscore()
        {
            md.Render("word _1_1_1_1_ word").Should().Be("word _1_1_1_1_ word");
        }

        [Test]
        public void Return_ScreeningItalicTeg()
        {
            md.Render(@"word \\ \_word\_ word").Should().Be(@"word \ _word_ word");
        }

        [Test]
        public void Return_ScreeningStrongTeg()
        {
            md.Render(@"word \_\_word\_\_ word").Should().Be(@"word __word__ word");
        }

        [Test]
        public void Return_ScreeningScreenChar()
        {
            md.Render(@"word \\ word").Should().Be(@"word \ word");
        }

        [Test]
        public void Return_ClosingItalicTegBetweenTwoWords()
        {
            md.Render("word _word_word_ word").Should().Be("word <em>word</em>word_ word");
        }
    }
}
