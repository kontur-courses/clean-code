using Markdown.Entities.Builders;
using Markdown.Entities.Converters;
using Markdown.Entities.Parsers;
using NUnit.Framework;
using System.Diagnostics;
using System.Text;

namespace Markdown.MarkdownTests.ConverterTests
{
    [TestFixture]
    [Category("Performance")]
    public class ConverterPerformanceTest
    {
        private Converter _converter;

        [SetUp]
        public void Setup()
        {
            var builder = new HtmlBuilder();
            var tokenizer = new MarkdownTokenizer();
            _converter = new Converter(builder, tokenizer);
        }

        [Test]
        public void Convert_ShouldHaveNearLinearTimeComplexity()
        {
            var sizes = new[] { 1000, 2000, 4000, 8000, 16000 };
            var executionTimes = new List<long>();
            var basePattern = "Простой текст с _курсивом_ и __жирным__ форматированием.";

            foreach (var size in sizes)
            {
                var testText = GenerateText(basePattern, size);
                var time = MeasureExecutionTime(() => _converter.Convert(testText));
                executionTimes.Add(time);
            }

            AssertNearLinearComplexity(sizes, executionTimes);
        }

        private long MeasureExecutionTime(Action action)
        {
            // Прогрев
            action();

            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }

        private string GenerateText(string pattern, int targetSize)
        {
            var result = new StringBuilder();
            while (result.Length < targetSize)
            {
                result.Append(pattern);
            }
            return result.ToString().Substring(0, targetSize);
        }

        private void AssertNearLinearComplexity(int[] sizes, List<long> times)
        {
            for (int i = 1; i < sizes.Length; i++)
            {
                var sizeRatio = (double)sizes[i] / sizes[i - 1];
                var timeRatio = (double)times[i] / times[i - 1];
                var deviation = (timeRatio - sizeRatio) / sizeRatio;

                // Допускаем отклонение до 40% от идеальной линейной сложности
                Assert.That(deviation, Is.LessThan(0.40),
                    $"При увеличении размера с {sizes[i - 1]} до {sizes[i]} " +
                    $"время выросло в {timeRatio:F2} раз вместо ожидаемых {sizeRatio:F2} " +
                    $"(отклонение {deviation:P0})");
            }
        }
    }
}
