using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [Test]
        public void RenderThrowArgumentException_ThenInputIsNull()
        {
            var md = new Md();

            Action result = () => md.Render(null);

            result.Should().Throw<ArgumentException>();
        }

        [TestCase("", TestName = "Then input is empty")]
        [TestCase("1", TestName = "Then input is \"1\"")]
        [TestCase("a", TestName = "Then input is \"a\"")]
        [TestCase("_", TestName = "Then input is \"_\"")]
        [TestCase("__", TestName = "Then input is \"__\"")]
        public void RenderReturnHtmlParagraph(string markdown)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().StartWith("<p>")
                .And.EndWith("</p>");
        }

        [TestCase("_em_", TestName = "Then input is \"_em_\"")]
        [TestCase("_a_", TestName = "Then input is \"_a_\"")]
        [TestCase("_a_ _a_", 2, TestName = "Then input is \"_a_ _a_\"")]
        [TestCase("_long string   with spaces_", TestName = "Then input is \"_long string   with spaces_\"")]
        public void RenderParsesInput_WithEmTag(string markdown, int pairCount = 1)
        {
            var md = new Md();
            var renderedMd = md.Render(markdown);

            var result = CountTagPairs(renderedMd, "em");

            result.Should().Be(pairCount);
        }

        [TestCase("__strong__", TestName = "Then input is \"__strong__\"")]
        [TestCase("__s__ __s__", 2, TestName = "Then input is \"__s__ __s__\"")]
        [TestCase("__a__", TestName = "Then input is \"__a__\"")]
        [TestCase("__long string   with spaces__", TestName = "Then input is \"__long string   with spaces__\"")]
        public void RenderParsesInput_WithStrongTag(string markdown, int pairCount = 1)
        {
            var md = new Md();
            var renderedMd = md.Render(markdown);

            var result = CountTagPairs(renderedMd, "strong");

            result.Should().Be(pairCount);
        }

        private int CountTagPairs(string line, string tag)
        {
            var tagStart = $"<{tag}>";
            var tagEnd = $"</{tag}>";
            var count = 0;
            var index = 0;
            while (true)
            {
                index = line.IndexOf(tagStart, index, StringComparison.CurrentCulture);
                if (index == -1)
                    return count;
                index = line.IndexOf(tagEnd, index, StringComparison.CurrentCulture);
                if (index == -1)
                    return count;
                count++;
            }
        }

        [Test]
        public void RenderParsesInput_WithCombinableNestedTags()
        {
            var md = new Md();

            var result = md.Render("__s _e_ s__");

            result.Should().ContainAll("<strong>", "</strong>", "<em>", "</em>");
        }

        [Test]
        public void RenderParsesInput_WithUncombinableNestedTags()
        {
            var md = new Md();

            var result = md.Render("_e __s__ e_");

            result.Should().NotContainAll("<strong>", "</strong>")
                .And.ContainAll("<em>", "</em>");
        }

        [TestCase("_1_", TestName = "Then input is \"_1_\"")]
        [TestCase("__1 with space and words 1__", TestName = "Then input is \"__1 with space and words 1__\"")]
        [TestCase("_a 1_", TestName = "Then input is \"_a 1_\"")]
        [TestCase("__1 a__", TestName = "Then input is \"__1 a__\"")]
        [TestCase("_a b__", TestName = "Then unpaired characters")]
        [TestCase("_a b _", TestName = "Then input is \"_a b _\"")]
        [TestCase("_ a b_", TestName = "Then input is \"_ a b_\"")]
        [TestCase("__a b_", TestName = "Then unpaired characters 2")]
        public void RenderDoesntParsesTag(string markdown)
        {
            var md = new Md();

            var result = md.Render(markdown);

            result.Should().NotContainAll("<em>", "</em>", "<strong>", "</strong>");
        }

        [TestCase(10)]
        [TestCase(15)]
        [TestCase(18)]
        [TestCase(20)]
        public void WorksAlmostLinearly(int maxRepeats)
        {
            var hugeLine = " _almost huge string_ with spaces  and e.t.c.!@^$&* __1__ ";
            for (var i = 0; i < maxRepeats; i++)
                hugeLine += hugeLine;
            var halfOfHugeLine = hugeLine.Substring(hugeLine.Length / 2);

            var elapsedMsForHuge = GetElapsedMsForLine(hugeLine);
            var elapsedMsForHalfOfHuge = GetElapsedMsForLine(halfOfHugeLine);

            elapsedMsForHuge.Should().BeInRange(elapsedMsForHalfOfHuge,
                                                elapsedMsForHalfOfHuge * 4);
        }

        private long GetElapsedMsForLine(string line)
        {
            var md = new Md();
            var watch = new Stopwatch();

            watch.Start();
            md.Render(line);
            watch.Stop();

            var lineLength = line.Length;
            TestContext.WriteLine($"{lineLength / watch.ElapsedMilliseconds} characters per ms");

            return watch.ElapsedMilliseconds;
        }
    }
}
