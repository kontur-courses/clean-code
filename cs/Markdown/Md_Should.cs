using System;
using System.Diagnostics;
using System.Linq;
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

        [TestCase("", "",
            TestName = "Empty string")]

        [TestCase("_word_", "<em>word</em>",
            TestName = "One italic tag")]

        [TestCase("word _ word _ word", "word _ word _ word",
            TestName = "Not render italics if space after opening low line or space before closing low line")]

        [TestCase("word _word_ _word_ _word_ word", "word <em>word</em> <em>word</em> <em>word</em> word",
            TestName = "Some italic tag")]

        [TestCase("__word__", "<strong>word</strong>",
            TestName = "One strong tag")]

        [TestCase("word __ word __ word", "word __ word __ word",
            TestName = "Not render strong if space after opening low line or space before closing low line")]

        [TestCase("word __word__ __word__ __word__ word", "word <strong>word</strong> <strong>word</strong> <strong>word</strong> word",
            TestName = "More strong tag")]

        [TestCase("word _word word", "word _word word",
            TestName = "Low line has no closing pair")]

        [TestCase("word word__ word", "word word__ word",
            TestName = "Double low line has no opening pair")]

        [TestCase("word __word _word_ word__ word", "word <strong>word <em>word</em> word</strong> word",
            TestName = "Italic tag in strong tag")]

        [TestCase("word _word __word__ word_ word", "word <em>word __word__ word</em> word",
            TestName = "Double low line in italic tag")]

        [TestCase("word _1_1_1_1_ word", "word _1_1_1_1_ word",
            TestName = "Not render italics within digits")]

        [TestCase(@"word \_word\_ word", @"word _word_ word",
            TestName = "Escaping low line")]

        [TestCase(@"word \__word\__ word", @"word __word__ word",
            TestName = "Escaping double low line")]

        [TestCase(@"word \\ word", @"word \ word",
            TestName = "Escaping escape char")]

        [TestCase(@"[This link](http://example.net/) has no title attribute.", "<a href=\"http://example.net/\">This link</a> has no title attribute.",
            TestName = "One link tag")]

        [TestCase("[_This link_](http://example.net/)", "<a href=\"http://example.net/\"><em>This link</em></a>",
            TestName = "Italic tag in link tag")]

        [TestCase("[__This link__](http://example.net/)", "<a href=\"http://example.net/\"><strong>This link</strong></a>",
            TestName = "Strong tag in link tag")]

        [TestCase("[This link](http://ex_amp_le.net/)", "<a href=\"http://ex_amp_le.net/\">This link</a>",
            TestName = "Not render italics in link \"href\" attribute")]

        [TestCase("[This link](http://ex__amp__le.net/)", "<a href=\"http://ex__amp__le.net/\">This link</a>",
            TestName = "Not render strong in link \"href\" attribute")]
        public void Return(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
        
        [Test]
        public void Render_LinearСomplexity()
        {
            const string pieceOfInputText = "_word_ __word__ word __word _word_ word__ word [This link](http://example.net/)";
            var inputText = "";

            inputText = string.Join("", Enumerable.Repeat(pieceOfInputText, 100));
            var time1 = MeasureFunctionTime(s => md.Render(s), inputText);

            inputText = string.Join("", Enumerable.Repeat(pieceOfInputText, 200));
            var time2 = MeasureFunctionTime(s => md.Render(s), inputText);

            inputText = string.Join("", Enumerable.Repeat(pieceOfInputText, 300));
            var time3 = MeasureFunctionTime(s => md.Render(s), inputText);
            
            time2.Should().BeInRange(time1 * 1, time1 * 4);
            time3.Should().BeInRange(time1 * 2, time1 * 8);
        }

        private static long MeasureFunctionTime<T>(Action<T> func, T inputText)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            func(inputText);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
