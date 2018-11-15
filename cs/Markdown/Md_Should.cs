using System;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        public Md Md;

        [SetUp]
        public void SetUp()
        {
            Md = new Md();
        }

        [TestCase(-1, 1, TestName = "width less than zero")]
        [TestCase(1, -1, TestName = "height less than zero")]
        public void PutNextRectangle_ThrowsArgumentExceptionWhen
            (int width, int height)
        {
            Action action = () => throw new ArgumentException();
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Return_EmptyString()
        {
            Md.Render("").Should().Be("");
        }

        [Test]
        public void Return_OneItalicTeg()
        {
            Md.Render("_word_").Should().Be("<em>word</em>");
        }

        [Test]
        public void Return_OneItalicTeg_WithTextOnSides()
        {
            Md.Render("word _word_ word").Should().Be("word <em>word</em> word");
        }

        [Test]
        public void Return_MoreItalicTeg()
        {
            Md.Render("word _word_ _word_ _word_ word").Should().Be("word <em>word</em> <em>word</em> <em>word</em> word");
        }

        [Test]
        public void Return_OneStrongTeg()
        {
            Md.Render("__word__").Should().Be("<strong>word</strong>");
        }

        [Test]
        public void Return_OneStrongTeg_WithTextOnSides()
        {
            Md.Render("word __word__ word").Should().Be("word <strong>word</strong> word");
        }

        [Test]
        public void Return_MoreStrongTeg()
        {
            Md.Render("word __word__ __word__ __word__ word").Should().Be("word <strong>word</strong> <strong>word</strong> <strong>word</strong> word");
        }

        [Test]
        public void Return_NotClosedTag()
        {
            Md.Render("word __word word").Should().Be("word __word word");
        }

        [Test]
        public void Return_NotOpenedTag()
        {
            Md.Render("word word__ word").Should().Be("word word__ word");
        }

        [Test]
        public void Return_ItalicTegInStrongTeg()
        {
            Md.Render("word __word _word_ word__ word").Should().Be("word <strong>word <em>word</em> word</strong> word");
        }

        [Test]
        public void Return_StrongTegInItalicTag()
        {
            Md.Render("word _word __word__ word_ word").Should().Be("word <em>word __word__ word</em> word");
        }

        [Test]
        public void Return_NumbersThroughUnderscore()
        {
            Md.Render("word _1_1_1_1_ word").Should().Be("word _1_1_1_1_ word");
        }



        [TearDown]
        public void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                Console.WriteLine($"Tag cloud visualization saved to file <{TestContext.CurrentContext.Test.FullName}>");
            }
        }
    }
}
