using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using MathNet.Numerics;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new Markdown();
        }

        private Markdown sut;

        [TestCase("hello", TestName = "Ordinary Text")]
        [TestCase("hello! it's() me.. i w*a*s wonder/==", TestName = "Ordinary TextWithout Special Symbols")]
        [TestCase("", TestName = "EmptyText")]
        public void Render_OrdinaryText_ExactText(string mdText)
        {
            var htmlText = sut.Render(mdText);
            htmlText.Should().Be(mdText);
        }

        [TestCase("hello _its me_", "hello <em>its me</em>", TestName = "Underscore")]
        [TestCase("hello __its me__", "hello <strong>its me</strong>", TestName = "Double underscore")]
        [TestCase("hello `its me`", "hello <code>its me</code>", TestName = "Underscore")]
        public void Render_OnePairedSymbol_CorrectHtmlText(string mdText, string expectedText)
        {
            var htmlText = sut.Render(mdText);
            htmlText.Should().Be(expectedText);
        }


        [TestCase("__ _ hello its me_ __", TestName = "Wrong White Space Position")]
        [TestCase("_hello its__ me i` was", TestName = "Unpaired Symbols")]
        [TestCase("hello 78_its__820 me_9 i`_0", TestName = "Symbols Between Numbers")]
        public void Render_(string mdText)
        {
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(mdText);
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            var mdText = "_hel __llo__ !";
            var htmlText = sut.Render(mdText);
            Console.WriteLine(htmlText);
        }

        [Test]
        public void Render_DoubleUnderscoreBetweenUnderscore_DoubleUnderscoreOrdinary()
        {
            var mdText = "_hello __its__ me_ i was";
            var expectedHtmlText = "<em>hello __its__ me</em> i was";
            var htmlText = sut.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_EscapedSymbols_NotShowBackslash()
        {
            var mdText = "\\__la";
            var expectedHtmlText = "__la";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_OnePairAndOneEscapedSymbol()
        {
            var mdText = "__hello \\`friend__";
            var expectedHtmlText = "<strong>hello `friend</strong>";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_OrdinaryBackslash_ExactText()
        {
            var mdText = "hello \\ its me";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(mdText);
        }

        [Test]
        public void Render_Sharp_TextWithHead()
        {
            var mdText = "#HEAD\n" +
                         "ordinary text";
            var expectedHtmlText = "<h1>HEAD</h1>" +
                                   "ordinary text";
            var htmlText = sut.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }


        [Test]
        public void Render_TextWithSpecialSymbols_CorrectHTMLText()
        {
            var mdText = "__hi _my `friend` how_ you__";
            var expectedHtmlText = "<strong>hi <em>my <code>friend</code> how</em> you</strong>";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_WorkFast()
        {
            var mdText =
                "The 680-bed __Ospedale dell’Angelo__ in Venice-Mestre, a _40-year-long project_ completed by ` Italian architect `Emilio` Ambasz, is billed`` as the ‘world’s first green general hospital’_=-";
            var times = new List<double>();
            var strb = new StringBuilder(mdText);
            var stopwatch = new Stopwatch();
            var lengths = new List<double>();
            for (var i = 0; i < 10; i++)
            {
                var text = strb.ToString();

                stopwatch.Start();
                var htmlText = sut.Render(text);
                stopwatch.Stop();

                times.Add(stopwatch.ElapsedTicks);
                lengths.Add(strb.Length);
                strb.Append(mdText);
                stopwatch.Reset();
            }

            var coeffQuadraticEquation = Fit.Polynomial(times.ToArray(), lengths.ToArray(), 2);
            Console.WriteLine(coeffQuadraticEquation[0]);
            Assert.That(coeffQuadraticEquation[0] < 1e-2);
        }
    }
}