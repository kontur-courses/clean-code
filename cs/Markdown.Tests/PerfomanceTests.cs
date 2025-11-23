using System.Diagnostics;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class LinearComplexityTests
    {
        [Test]
        public void ConvertToHtml_ShouldHaveLinearTimeComplexity_WithGrowingTextLength()
        {
            var sizes = new[] { 100, 1000, 5000 };
            var times = new double[sizes.Length];

            for (var i = 0; i < sizes.Length; i++)
            {
                var text = GeneratePlainText(sizes[i]);
                times[i] = MeasureTime(() => Md.ConvertToHtml(text), 10);
            }

            var ratios = times.Select(t => t / times[0]).ToArray();
            var expectedRatios = sizes.Select(s => (double)s / sizes[0]).ToArray();

            TestContext.Out.WriteLine("Text Length vs Time Ratios:");
            for (var i = 0; i < sizes.Length; i++)
            {
                TestContext.Out.WriteLine($"Size: {sizes[i]}, Expected: {expectedRatios[i]:F1}x, Actual: {ratios[i]:F2}x");
            }

            for (var i = 1; i < ratios.Length; i++)
            {
                var quadraticThreshold = Math.Pow(expectedRatios[i], 1.5);
                ratios[i].Should().BeLessThan(quadraticThreshold, 
                    $"Time growth should be better than O(n^1.5). Size increased {expectedRatios[i]}x, time increased {ratios[i]}x");
            }
        }

        [Test]
        public void ConvertToHtml_ShouldHaveLinearTimeComplexity_WithGrowingWordCount()
        {
            var wordCounts = new[] { 100, 500, 1000 };
            var times = new double[wordCounts.Length];

            for (var i = 0; i < wordCounts.Length; i++)
            {
                var text = GenerateWords(wordCounts[i]);
                times[i] = MeasureTime(() => Md.ConvertToHtml(text), 10);
            }

            var ratios = times.Select(t => t / times[0]).ToArray();
            var expectedRatios = wordCounts.Select(w => (double)w / wordCounts[0]).ToArray();

            TestContext.Out.WriteLine("Word Count vs Time Ratios:");
            for (int i = 0; i < wordCounts.Length; i++)
            {
                TestContext.Out.WriteLine($"Words: {wordCounts[i]}, Expected: {expectedRatios[i]:F1}x, Actual: {ratios[i]:F2}x");
            }

            for (var i = 1; i < ratios.Length; i++)
            {
                var linearUpperBound = expectedRatios[i] * 2;
                ratios[i].Should().BeLessThan(linearUpperBound,
                    $"Time should grow linearly. Words increased {expectedRatios[i]}x, time increased {ratios[i]}x");
            }
        }

        [Test]
        public void ConvertToHtml_ShouldHaveLinearTimeComplexity_WithManyFormattingTags()
        {
            var tagCounts = new[] { 50, 200, 500 };
            var times = new double[tagCounts.Length];

            for (int i = 0; i < tagCounts.Length; i++)
            {
                var text = GenerateTextWithManyTags(tagCounts[i]);
                times[i] = MeasureTime(() => Md.ConvertToHtml(text), 10);
            }

            var ratios = times.Select(t => t / times[0]).ToArray();
            var expectedRatios = tagCounts.Select(t => (double)t / tagCounts[0]).ToArray();

            TestContext.Out.WriteLine("Tag Count vs Time Ratios:");
            for (var i = 0; i < tagCounts.Length; i++)
            {
                TestContext.Out.WriteLine($"Tags: {tagCounts[i]}, Expected: {expectedRatios[i]:F1}x, Actual: {ratios[i]:F2}x");
            }

            for (var i = 1; i < ratios.Length; i++)
            {
                var quadraticThreshold = Math.Pow(expectedRatios[i], 1.8);
                ratios[i].Should().BeLessThan(quadraticThreshold,
                    $"Tag processing should not be quadratic. Tags increased {expectedRatios[i]}x, time increased {ratios[i]}x");
            }
        }

        [Test]
        public void ConvertToHtml_ShouldHandleManyParagraphs_WithLinearComplexity()
        {
            var paragraphCounts = new[] { 10, 50, 100 };
            var times = new double[paragraphCounts.Length];

            for (int i = 0; i < paragraphCounts.Length; i++)
            {
                var text = GenerateManyParagraphs(paragraphCounts[i]);
                times[i] = MeasureTime(() => Md.ConvertToHtml(text), 5);
            }

            var ratios = times.Select(t => t / times[0]).ToArray();
            var expectedRatios = paragraphCounts.Select(p => (double)p / paragraphCounts[0]).ToArray();

            TestContext.Out.WriteLine("Paragraph Count vs Time Ratios:");
            for (int i = 0; i < paragraphCounts.Length; i++)
            {
                TestContext.Out.WriteLine($"Paragraphs: {paragraphCounts[i]}, Expected: {expectedRatios[i]:F1}x, Actual: {ratios[i]:F2}x");
            }

            for (var i = 1; i < ratios.Length; i++)
            {
                var tolerance = expectedRatios[i] * 0.5; 
                ratios[i].Should().BeLessThan(expectedRatios[i] + tolerance,
                    $"Paragraph processing should be near linear");
            }
        }

        private string GeneratePlainText(int length)
        {
            return new string('a', length);
        }

        private string GenerateWords(int wordCount)
        {
            var words = Enumerable.Range(1, wordCount)
                .Select(i => $"word{i}");
            return string.Join(" ", words);
        }

        private string GenerateTextWithManyTags(int tagCount)
        {
            var words = Enumerable.Range(1, tagCount * 2)
                .Select(i => i % 2 == 0 ? $"**word{i}**" : $"_word{i}_");
            return string.Join(" ", words);
        }

        private string GenerateManyParagraphs(int paragraphCount)
        {
            var paragraphs = Enumerable.Range(1, paragraphCount)
                .Select(i => $"Paragraph {i} with some text content.");
            return string.Join("\n\n", paragraphs);
        }

        private double MeasureTime(Action action, int iterations)
        {
            for (var i = 0; i < 3; i++)
                action();

            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++)
                action();
            sw.Stop();

            return sw.Elapsed.TotalMilliseconds;
        }
    }
}