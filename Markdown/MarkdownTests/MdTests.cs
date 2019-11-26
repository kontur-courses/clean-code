using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        [TestCase("Hello world!", "<p>Hello world!</p>", TestName = "RenderPlainString")]
        [TestCase("\\_Hello world\\_!", "<p>_Hello world_!</p>",TestName = "RenderEscapedEmphasis")]
        [TestCase("\\_\\_Hello world\\_\\_!", "<p>__Hello world__!</p>", TestName = "RenderEscapedStrongEmphasis")]
        [TestCase("_Hello world_!", "<p><em>Hello world</em>!</p>", TestName = "RenderEmphasis")]
        [TestCase("__Hello world__!", "<p><strong>Hello world</strong>!</p>", TestName = "RenderStrongEmphasis")]
        [TestCase("__Hello _world_!__", "<p><strong>Hello <em>world</em>!</strong></p>", TestName = "RenderEmphasisInsideStrongEmphasis")]
        [TestCase("_Hello __world__!_", "<p><em>Hello __world__!</em></p>", TestName = "RenderStrongEmphasisInsideEmphasis")]
        [TestCase("_hello123!_", "<p>_hello123!_</p>", TestName = "RenderEmphasisInsideTextWithNumbers")]
        [TestCase("__unpaired _symbols" ,"<p>__unpaired _symbols</p>", TestName = "RenderUnpairedDelimiters")]
        [TestCase("not_ emphasis_", "<p>not_ emphasis_</p>", TestName = "RenderBeginningEmphasisBeforeWhitespace")]
        [TestCase("not__ emphasis__", "<p>not__ emphasis__</p>", TestName = "RenderBeginningStrongEmphasisBeforeWhitespace")]
        [TestCase("_not _emphasis", "<p>_not _emphasis</p>", TestName = "RenderEndingEmphasisAfterWhitespace")]
        [TestCase("__not __emphasis", "<p>__not __emphasis</p>", TestName = "RenderEndingStrongEmphasisAfterWhitespace")]
        [TestCase("[Google](https://google.com)", "<p><a href=\"https://google.com\">Google</a></p>", TestName = "RenderLinks")]
        [TestCase("[_Google_](https://google.com)", "<p><a href=\"https://google.com\"><em>Google</em></a></p>", TestName = "RenderEmphasisInsideLinks")]
        [TestCase("[__Google__](https://google.com)", "<p><a href=\"https://google.com\"><strong>Google</strong></a></p>", TestName = "RenderStrongEmphasisInsideLinks")]
        [TestCase("[Google](https://_google_.com)", "<p><a href=\"https://_google_.com\">Google</a></p>", TestName = "RenderLinksNoEmphasisInsideAddress")]
        [TestCase("[Google](__https://google.com__)", "<p><a href=\"__https://google.com__\">Google</a></p>", TestName = "RenderLinksNoStrongEmphasisInsideAddress")]
        [TestCase("_[__Google__](https://google.com)_", "<p><em><a href=\"https://google.com\">__Google__</a></em></p>", TestName = "RenderStrongEmphasisNotInEmphasisWhenNested")]
        [TestCase("__[_Google_](https://google.com)__", "<p><strong><a href=\"https://google.com\"><em>Google</em></a></strong></p>", TestName = "RenderEmphasisInStrongEmphasisWhenNested")] 
        public void TestRender(string input, string expected)
        {
            var result = Md.Render(input);

            result.Should().Be(expected);
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