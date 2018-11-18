using System;
using System.Diagnostics;
using System.Runtime;
using FluentAssertions;
using Markdown.Renderer;
using Markdown.Translator;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [Test]
        public void RenderThrowArgumentException_WhenInputIsNull()
        {
            var md = new Md();

            Action result = () => md.Render(null);

            result.Should().Throw<ArgumentException>();
        }

        [TestCase("", TestName = "When input is empty")]
        [TestCase("1", TestName = "When input is number")]
        [TestCase("a", TestName = "When input is letter")]
        [TestCase("_", TestName = "When input is underscore")]
        [TestCase("__", TestName = "When input is 2 underscores")]
        public void RenderReturnHtmlParagraph(string markdown)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().StartWith("<p>")
                .And.EndWith("</p>");
        }

        [TestCase("_em_", TestName = "When input is word in em tag")]
        [TestCase("_a_ _a_", TestName = "When input is 2 words in em tags")]
        [TestCase("_long string   with spaces_", TestName = "When input contains words and spaces")]
        public void RenderParsesInput_WithEmTag(string markdown)
        {
            var md = new Md();
            var expected = GetCorrectLine(markdown, "_", "em");

            var result = md.Render(markdown);

            result.Should().Be(expected);
        }

        [TestCase("__strong__", TestName = "When input is word in strong tag")]
        [TestCase("__s__ __s__", TestName = "When input is 2 words in strong tags")]
        [TestCase("__almost long string   with spaces__", TestName = "When input contains spaces and words")]
        public void RenderParsesInput_WithStrongTag(string markdown)
        {
            var md = new Md();
            var expected = GetCorrectLine(markdown, "__", "strong");

            var result = md.Render(markdown);

            result.Should().Be(expected);
        }

        private string GetCorrectLine(string markdown, string tag, string htmlTag)
        {
            var tagStart = $"<{htmlTag}>";
            var tagEnd = $"</{htmlTag}>";
            while (true)
            {
                var indexOfTag = markdown.IndexOf(tag, StringComparison.CurrentCulture);
                if (indexOfTag == -1)
                    break;
                markdown = markdown.Remove(indexOfTag, tag.Length);
                markdown = markdown.Insert(indexOfTag, tagStart);

                indexOfTag = markdown.IndexOf(tag, indexOfTag + 1, StringComparison.CurrentCulture);
                markdown = markdown.Remove(indexOfTag, tag.Length);
                markdown = markdown.Insert(indexOfTag, tagEnd);
            }

            return $"<p>{markdown}</p>";
        }

        [TestCase("__s _e_ s__", "<strong>s <em>e</em> s</strong>", TestName = "When em in strong tag")]
        [TestCase("_e _ee_ e_", "<em>e <em>ee</em> e</em>", TestName = "When double nesting of em tag")]
        [TestCase("__s __s __s__ s__ s__", "<strong>s <strong>s <strong>s</strong> s</strong> s</strong>", TestName = "When deep nesting of strong tag")]
        public void RenderParsesCorrect_WhenTagsAreCombinable(string markdown, string expected)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().Be($"<p>{expected}</p>");
        }

        [TestCase("_e __s__ e_", "<em>e __s__ e</em>", TestName = "When strong in em tag")]
        [TestCase("_e__s _e_ s__e_", "<em>e__s <em>e</em> s__e</em>", TestName = "When nesting of em and strong tags")]
        public void RenderParsesCorrect_WhenTagsAreUncombinable(string markdown, string expected)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().Be($"<p>{expected}</p>");
        }

        [TestCase("_1_", TestName = "When input is number in tag")]
        [TestCase("__1 with space and words 1__", TestName = "When input tag starts and ends with numbers")]
        [TestCase("_a 1_", TestName = "When tag ends with number")]
        [TestCase("__1 a__", TestName = "When tag starts with number")]
        [TestCase("_a b _", TestName = "When input with space between letter and tag end")]
        [TestCase("_ a b_", TestName = "When input with space between tag start and letter")]
        [TestCase("a_12_3", TestName = "When input with numbers")]
        [TestCase("_a b__", TestName = "When unpaired tags")]
        [TestCase("__a b_", TestName = "When unpaired tags 2")]
        public void RenderDoesNotParsesTag(string markdown)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().Be($"<p>{markdown}</p>");
        }

        [TestCase(17)]
        [TestCase(19)]
        public void WorksAlmostLinearly(int maxRepeats)
        {
            var hugeLine = "_START _almost huge string_ with spaces  and e.t.c.!@^$&* __1__  END_";
            for (var i = 0; i < maxRepeats; i++)
                hugeLine += hugeLine;
            var halfOfHugeLine = hugeLine.Substring(hugeLine.Length / 2);

            var elapsedMsForHuge = GetElapsedMsForLine(hugeLine);

            var elapsedMsForHalfOfHuge = GetElapsedMsForLine(halfOfHugeLine);

            elapsedMsForHuge.Should().BeLessOrEqualTo(elapsedMsForHalfOfHuge * 2 + elapsedMsForHuge / 10L);
        }

        private long GetElapsedMsForLine(string line)
        {
            GC.Collect();
            var oldMode = GCSettings.LatencyMode;
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            var renderer = new HtmlRenderer();
            var translator = new MarkdownTranslator();
            var watch = new Stopwatch();

            watch.Start();
            var translatedText = translator.Translate(line);
            watch.Stop();
            var elapsedOnTranslate = watch.ElapsedMilliseconds;
            PrintOperationPerformanceResult("translating", line.Length, elapsedOnTranslate);

            watch.Restart();
            renderer.Render(translatedText);
            watch.Stop();
            var elapsedOnRendering = watch.ElapsedMilliseconds;
            PrintOperationPerformanceResult("rendering", translatedText.Length, elapsedOnRendering);

            GCSettings.LatencyMode = oldMode;
            GC.Collect();

            var totalTime = elapsedOnTranslate + elapsedOnRendering;
            TestContext.WriteLine("-----TOTAL:\n"
                                  + $"Reading of {line.Length} characters consumed {totalTime} ms\n" +
                                  $"{line.Length / totalTime} characters per ms\n\n\n");
            return totalTime;
        }

        private void PrintOperationPerformanceResult(string operationName, int inputLength, long elapsedMs)
        {
            TestContext.WriteLine($"-----{operationName.ToUpper()}:\n" +
                                  $"Input length: {inputLength} characters\n" +
                                  $"Time consumed: {elapsedMs} ms\n"
                                  + $"Speed: {inputLength / Math.Max(elapsedMs, 1)} characters per ms\n");
        }
    }
}