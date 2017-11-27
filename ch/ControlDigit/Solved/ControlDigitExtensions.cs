using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace ControlDigit.Solved
{
	public static class ControlDigitExtensions
	{
		public static int ControlDigit(this long number)
		{
			int sum = 0;
			int factor = 1;
			do
			{
				int digit = (int)(number % 10);
				sum += factor * digit;
				factor = 4 - factor;
				number /= 10;

			}
			while (number > 0);

			int result = sum % 11;
			if (result == 10)
				result = 1;
			return result;
		}

        private static readonly int[] weights = new[] { 1, 3 }.Repeat().Take(20).ToArray();

        public static int ControlDigit2(this long number)
        {
            var sum = number
                .GetDigitsFromLeastSignificant()
                .SumWithWeights(weights) % 11;
            return sum == 10 ? 1 : sum;
        }
    }

    // Общий код, ничего не знает про контрольные разряды и прикладную задачу.
    public static class MoreEnumerableExtensions
    {
        public static IEnumerable<int> GetDigitsFromLeastSignificant(this long number)
        {
            do
            {
                yield return (int)(number % 10);
                number /= 10;
            } while (number > 0);
        }

        public static int SumWithWeights(this IEnumerable<int> numbers, IEnumerable<int> weights)
        {
            return numbers.Zip(weights, (n, w) => n * w).Sum();
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items)
        {
            var i2 = items.ToList();
            while (true)
                foreach (var item in i2) yield return item;
            // ReSharper disable once FunctionNeverReturns
        }
    }
    [TestFixture]
    public class ControlDigitExtensions_Tests
    {
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(10, ExpectedResult = 3)]
        [TestCase(15, ExpectedResult = 8)]
        [TestCase(17, ExpectedResult = 1)]
        [TestCase(18, ExpectedResult = 0)]
        public int TestControlDigit(long x)
        {
            return x.ControlDigit();
        }

        [Test]
        public void CompareImplementations()
        {
            for (long i = 0; i < 100000; i++)
                Assert.AreEqual(i.ControlDigit(), i.ControlDigit2());
        }
    }

    [TestFixture]
    public class ControlDigit_PerformanceTests
    {
        [Test]
        public void TestControlDigitSpeed()
        {
            var count = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit();
            Console.WriteLine("Old " + sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit2();
            Console.WriteLine("New " + sw.Elapsed);
        }
    }

}
