using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
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

        [Test]
        public void ReturnLink()
        {
            md.Render(@"[This link](http://example.net/) has no title attribute.").Should().Be("<a href=\"http://example.net/\">This link</a> has no title attribute.");
        }

        [Test]
        public void ReturnLink123()
        {
            md.Render(@"[_This link_](http://example.net/) has no title attribute.").Should().Be("<a href=\"http://example.net/\"><em>This link</em></a> has no title attribute.");
        }

        [Test]
        public void ReturnLink1231212()
        {
            md.Render(@"[_This link_](http://ex_amp_le.net/) has no title attribute.").Should().Be("<a href=\"http://ex_amp_le.net/\"><em>This link</em></a> has no title attribute.");
        }

        [Test]
        public void arr()
        {
            var arr1 = new int[10];
            for (int i = 0; i < 9; i++)
            {
                arr1[i] = i;
            }
            var sw1 = new Stopwatch();
            int count = 0;
            sw1.Start();
            for (int i = 0; i < 9; i++)
            {
                count += arr1[i];
            }
            sw1.Stop();
            var sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < 9; i++)
            {
                count += arr1[i % 30];
            }
            sw2.Stop();
            sw1.ElapsedMilliseconds.Should().Be(sw2.ElapsedMilliseconds);
            //sw2.ElapsedMilliseconds.Should().Be(1);
        }

        //[TestCase("", "",
        //    TestName = "Empty string")]

        //[TestCase("_word_", "<em>word</em>",
        //    TestName = "One italic tag")]

        //[TestCase("word _ word _ word", "word _ word _ word",
        //    TestName = "Not render italics if space after opening low line or space before closing low line")]

        //[TestCase("word _word_ _word_ _word_ word", "word <em>word</em> <em>word</em> <em>word</em> word", 
        //    TestName = "Some italic tag")]

        //[TestCase("__word__", "<strong>word</strong>", 
        //    TestName = "One strong tag")]

        //[TestCase("word __word__ word", "word <strong>word</strong> word", 
        //    TestName = "Not render strong if space after opening low line or space before closing low line")]

        //[TestCase("word __word__ __word__ __word__ word", "word <strong>word</strong> <strong>word</strong> <strong>word</strong> word", 
        //    TestName = "More strong tag")]

        //[TestCase("word _word word", "word _word word", 
        //    TestName = "Low line has no closing pair")]

        //[TestCase("word word__ word", "word word__ word", 
        //    TestName = "Double low line has no opening pair")]

        //[TestCase("word __word _word_ word__ word", "word <strong>word <em>word</em> word</strong> word", 
        //    TestName = "Italic tag in strong tag")]

        //[TestCase("word _word __word__ word_ word", "word <em>word __word__ word</em> word", 
        //    TestName = "Double low line in italic tag")]

        //[TestCase("word _1_1_1_1_ word", "word _1_1_1_1_ word", 
        //    TestName = "Not render italics within digits")]

        //[TestCase(@"word \_word\_ word", @"word _word_ word", 
        //    TestName = "Escaping low line")]

        //[TestCase(@"word \__word\__ word", @"word __word__ word", 
        //    TestName = "Escaping double low line")]

        //[TestCase(@"word \\ word", @"word \ word", 
        //    TestName = "Escaping escape char")]

        //[TestCase(@"[This link](http://example.net/) has no title attribute.", "<a href=\"http://example.net/\">This link</a> has no title attribute.", 
        //    TestName = "lllink")]
        //public void Return(string input, string expected)
        //{
        //    md.Render(input).Should().Be(expected);
        //}
    }
}
