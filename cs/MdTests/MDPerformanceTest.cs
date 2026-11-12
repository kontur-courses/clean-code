using FluentAssertions;
using Markdown;
using System.Diagnostics;
using System.Text;

namespace MdTests
{
    public class MDPerformanceTest
    {
        private Md _md = new Md();

        [TestCase("_a ")]
        [TestCase("__a ")]
        [TestCase("__a _b ")]
        [TestCase("_a_ ")]
        [TestCase("__a__ ")]
        [TestCase("__a _b_ c__ ")]
        [TestCase("_a __b__ c_ ")]
        [TestCase("1_2_3 ")]
        [TestCase("_a b ")]
        public void Parse_LargeInput_FinishesInReasonableTime(string pattern)
        {
            var input = Repeat(pattern, 200_000);

            MeasureMs(input); 
            var elapsed = MeasureMs(input);

            elapsed.Should().BeLessThan(3000,
                because: "алгоритм должен работать за линейное время, а не за квадратичное");
        }

        [TestCase("_a ")]
        [TestCase("__a _b ")]
        [TestCase("_a_ ")]
        [TestCase("__a _b_ c__ ")]
        public void Parse_DoublingInput_DoublesTimeAtMost(string pattern)
        {
            var small = Repeat(pattern, 100_000);
            var large = Repeat(pattern, 200_000);

            MeasureMs(small); 
            var smallTime = Math.Min(MeasureMs(small), Math.Min(MeasureMs(small), MeasureMs(small)));
            var largeTime = Math.Min(MeasureMs(large), Math.Min(MeasureMs(large), MeasureMs(large)));

            largeTime.Should().BeLessThan(Math.Max(smallTime, 20) * 3);
        }

        [Test]
        public void Parse_ManyLines_FinishesInReasonableTime()
        {
            var input = Repeat("# Заголовок __с _разными_ символами__\nТекст _курсив_ и __жирный__\n", 50_000);

            MeasureMs(input);
            var elapsed = MeasureMs(input);

            elapsed.Should().BeLessThan(3000);
        }

        [Test]
        public void Parse_LongSingleWordWithManyUnderscores_FinishesInReasonableTime()
        {
            var input = new string('_', 1_000_000);

            MeasureMs(input).Should().BeLessThan(3000);
        }


        [Test]
        public void Parse_ManyListItems_FinishesInReasonableTime()
        {
            var input = Repeat("- _a_ __b__ _c\n", 100_000);

            MeasureMs(input); 
            MeasureMs(input).Should().BeLessThan(3000);
        }


        private static string Repeat(string pattern, int times)
        {
            var sb = new StringBuilder(pattern.Length * times);
            for (int i = 0; i < times; i++) sb.Append(pattern);
            return sb.ToString();
        }

        private long MeasureMs(string input)
        {
            var sw = Stopwatch.StartNew();
            _md.Parse(input);
            return sw.ElapsedMilliseconds;
        }


    }
}
