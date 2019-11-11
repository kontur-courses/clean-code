using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        [Test]
        public void ParsePlainString()
        {
            const string expectedResult = "<p>Hello world!</p>";

            var result = Md.Render("Hello world!");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEscapedEmphasis()
        {
            const string expectedResult = "<p>_Hello world_!</p>";

            var result = Md.Render("\\_Hello world\\_!");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEscapedStrongEmphasis()
        {
            const string expectedResult = "<p>__Hello world__!</p>";

            var result = Md.Render("\\_\\_Hello world\\_\\_!");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEmphasis()
        {
            const string expectedResult = "<p><em>Hello world</em>!</p>";

            var result = Md.Render("_Hello world_!");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseStrongEmphasis()
        {
            const string expectedResult = "<p><strong>Hello world</strong>!</p>";

            var result = Md.Render("__Hello world__!");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEmphasisInsideStrongEmphasis()
        {
            const string expectedResult = "<p><strong>Hello <em>world</em>!</strong></p>";

            var result = Md.Render("__Hello _world_!__");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseStrongEmphasisInsideEmphasis()
        {
            const string expectedResult = "<p><em>Hello __world__!</em></p>";

            var result = Md.Render("_Hello __world__!_");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEmphasisInsideTextWithNumbers()
        {
            const string expectedResult = "<p>_hello123!_</p>";

            var result = Md.Render("_hello123!_");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseUnpairedDelimiters()
        {
            const string expectedResult = "<p>__unpaired _symbols</p>";

            var result = Md.Render("__unpaired _symbols");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseBeginningEmphasisBeforeWhitespace()
        {
            const string expectedResult = "<p>not_ emphasis_</p>";

            var result = Md.Render("not_ emphasis_");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseBeginningStrongEmphasisBeforeWhitespace()
        {
            const string expectedResult = "<p>not__ emphasis__</p>";

            var result = Md.Render("not__ emphasis__");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEndingEmphasisAfterWhitespace()
        {
            const string expectedResult = "<p>_not _emphasis</p>";

            var result = Md.Render("_not _emphasis");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEndingStrongEmphasisAfterWhitespace()
        {
            const string expectedResult = "<p>__not __emphasis</p>";

            var result = Md.Render("__not __emphasis");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseLinks()
        {
            const string expectedResult = "<p><a href=\"https://google.com\">Google</a></p>";

            var result = Md.Render("[Google](https://google.com)");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseEmphasisInsideLinks()
        {
            const string expectedResult = "<p><a href=\"https://google.com\"><em>Google</em></a></p>";

            var result = Md.Render("[_Google_](https://google.com)");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseStrongEmphasisInsideLinks()
        {
            const string expectedResult = "<p><a href=\"https://google.com\"><strong>Google</strong></a></p>";

            var result = Md.Render("[__Google__](https://google.com)");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseLinksNoEmphasisInsideAddress()
        {
            const string expectedResult = "<p><a href=\"https://_google_.com\">Google</a></p>";

            var result = Md.Render("[Google](https://_google_.com)");

            result.Should().Be(expectedResult);
        }

        [Test]
        public void ParseLinksNoStrongEmphasisInsideAddress()
        {
            const string expectedResult = "<p><a href=\"__https://google.com__\">Google</a></p>";

            var result = Md.Render("[Google](__https://google.com__)");

            result.Should().Be(expectedResult);
        }

        [Test]
        [Retry(5)]
        public void LinearPerformance()
        {
            const string lineToParse = "__Site is [_Google_](https://google.com)__ ";

            var ts1 = RunRepeatedString(lineToParse, 500);
            var ts2 = RunRepeatedString(lineToParse, 5000);
            var ts3 = RunRepeatedString(lineToParse, 50000);
            
            (ts2 / ts1).Should().BeApproximately(ts3 / ts2, 1,
                "Time complexity should be linear");
        }

        [Test]
        public void StrongEmphasisNotInEmphasisWhenNested()
        {
            const string expectedResult = "<p><em><a href=\"https://google.com\">__Google__</a></em></p>";

            var result = Md.Render("_[__Google__](https://google.com)_");

            result.Should().Be(expectedResult);
        }
        
        [Test]
        public void EmphasisInStrongEmphasisWhenNested()
        {
            const string expectedResult = "<p><strong><a href=\"https://google.com\"><em>Google</em></a></strong></p>";

            var result = Md.Render("__[_Google_](https://google.com)__");

            result.Should().Be(expectedResult);
        }

        private static TimeSpan RunRepeatedString(string stringToRepeat, int count)
        {
            var lineToRender = new StringBuilder(stringToRepeat.Length * count)
                .Insert(0, stringToRepeat, count)
                .ToString();

            var sw = new Stopwatch();
            sw.Start();
            var result = Md.Render(lineToRender);
            sw.Stop();
            return sw.Elapsed;
        }
    }
}