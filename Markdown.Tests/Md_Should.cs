using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        private string RandomString(int size)
        {
            var characters = new List<char>()
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                ' ',
                '\\',
                ' ',
                '_',
                '_',
                '1',
                '2',
                '[',
                ']',
                '(',
                ')'
                
            };
            var builder = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var symbol = characters[random.Next(characters.Count)];
                builder.Append(symbol);
            }

            return builder.ToString();
        }

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void notСhangeText_WhenNoFormatting()
        {
            var text = "text without formatting";
            md.Render(text).Should().Be(text);
        }

        [Test]
        public void addItalicTag_WhenIsSingleUnderline()
        {
            var text = "_italic text_";
            md.Render(text).Should().Be("<em>italic text</em>");
        }

        [Test]
        public void addBoldTag_WhenIsDoubleUnderline()
        {
            var text = "__bold text__";
            md.Render(text).Should().Be("<strong>bold text</strong>");
        }

        [Test]
        public void renderItalic_WhenInsideBoldTag()
        {
            var text = "__bold _italic and bold_ bold__";
            md.Render(text).Should().Be("<strong>bold <em>italic and bold</em> bold</strong>");
        }

        [Test]
        public void notRenderBold_WhenInsideItalicTag()
        {
            var text = "_italic __not bold__ italic_";
            md.Render(text).Should().Be("<em>italic _</em>not bold<em></em> italic_");
        }

        [Test]
        public void notFindShell_WhenSurroundedByNumbers()
        {
            var text = "_italic2_0italic_";
            md.Render(text).Should().Be("<em>italic2_0italic</em>");
        }

        [TestCase("\\_simple text\\_", ExpectedResult = "_simple text_")]
        [TestCase("_italic\\_ text_", ExpectedResult = "<em>italic_ text</em>")]
        public string removeShieldingCharacters_AfterRender(string text)
        {
            return md.Render(text);
        }

        [TestCase("abcd _italic_aer __Bold__", ExpectedResult = "abcd <em>italic</em>aer <strong>Bold</strong>")]
        [TestCase("__bold___italic_", ExpectedResult = "<strong>bold</strong><em>italic</em>")]
        [TestCase("__b _i_b_i_ b__", ExpectedResult = "<strong>b <em>i</em>b<em>i</em> b</strong>")]
        public string RenderMultipleTokens(string text)
        {
            return md.Render(text);
        }

        [Test]
        public void shouldWorkInLinearTime_WhenBigData()
        {
            var measurementResult = new List<Tuple<int, long>>();
            var stopwatch = new Stopwatch();
            for (var size = 500; size < 10000000; size *= 5)
            {
                stopwatch.Reset();
                var text = RandomString(size);
                stopwatch.Start();
                md.Render(text);
                stopwatch.Stop();
                measurementResult.Add(Tuple.Create(size, stopwatch.ElapsedTicks));
            }
            for (var i = 0; i < measurementResult.Count - 1; i++)
            {
                var firstTime = measurementResult[i].Item2;
                var secondTime = measurementResult[i + 1].Item2;
                var firstSize = measurementResult[i].Item1;
                var secondSize = measurementResult[i + 1].Item1;
                var quotientSizes = (double) firstSize/secondSize;
                var quotientTimes = (double) firstTime/secondTime;
                quotientTimes.Should().BeGreaterThan(quotientSizes / 2);
            }
        }

        [TestCase("__bold__", "qwerty", ExpectedResult = "<strong style=qwerty>bold</strong>")]
        [TestCase("_italic_", "css", ExpectedResult = "<em style=css>italic</em>")]
        [TestCase("[text](link)", "aaa", ExpectedResult = "<a href=link style=aaa>text</a>")]
        public string addStyleAttribute_WhenSetCss(string content, string attributeValue)
        {
            md.CssAtribute = attributeValue;
            return md.Render(content);
        }

        [Test]
        public void RenderTextWithUrlFormatting()
        {
            const string text = "[text](http://link.com)";
            md.Render(text).Should().Be("<a href=http://link.com>text</a>");
        }

        [Test]
        public void AddBaseUrlToAttribute()
        {
            md.BaseUrl = "base_url/";
            const string text = "[text](relative_url)";
            md.Render(text).Should().Be("<a href=base_url/relative_url>text</a>");
        }

    }
}
