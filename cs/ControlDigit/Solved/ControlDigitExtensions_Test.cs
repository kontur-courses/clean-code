using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace ControlDigit.Solved
{
    [TestFixture]
    public class ControlDigitExtensions_Tests
    {
        [TestCase(00000000000, ExpectedResult = 0)]
        [TestCase(00000000001, ExpectedResult = 7)]
        [TestCase(00000000002, ExpectedResult = 4)]
        [TestCase(00000000009, ExpectedResult = 3)]
        [TestCase(00000000010, ExpectedResult = 9)]
        [TestCase(00000000013, ExpectedResult = 0)]
        [TestCase(00000000015, ExpectedResult = 4)]
        [TestCase(00000000017, ExpectedResult = 8)]
        [TestCase(00000000018, ExpectedResult = 5)]
        [TestCase(11111111111, ExpectedResult = 7)]
        [TestCase(12345678901, ExpectedResult = 2)]
        [TestCase(98765432101, ExpectedResult = 2)]
        [TestCase(11223344556, ExpectedResult = 2)]
        [TestCase(32512312431, ExpectedResult = 1)]
        [TestCase(98439874398, ExpectedResult = 8)]
        [TestCase(98439876398, ExpectedResult = 6)]
        public int Upc(long x)
        {
            return x.CalculateUpc();
        }

        [TestCase(1, ExpectedResult = 1)]
        [TestCase(10, ExpectedResult = 2)]
        [TestCase(100, ExpectedResult = 3)]
        [TestCase(1001, ExpectedResult = 4 + 1)]
        [TestCase(1111, ExpectedResult = 4 + 3 + 2 + 1)]
        [TestCase(112233445, ExpectedResult = 95)]
        [TestCase(87654303, ExpectedResult = 0)]
        [TestCase(87654302, ExpectedResult = 0)]
        [TestCase(116973385, ExpectedResult = 89)]
        [TestCase(152675138, ExpectedResult = 70)]
        [TestCase(463436384, ExpectedResult = 96)]
        [TestCase(158757369, ExpectedResult = 28)]
        [TestCase(192168000, ExpectedResult = 62)]
        public int Snils(long x)
        {
            return x.CalculateSnils();
        }


        [Test]
        public void CompareUpcImplementations()
        {
            for (long i = 0; i < 100000; i++)
                Assert.AreEqual(i.CalculateUpcOld(), i.CalculateUpc(), i.ToString());
        }
    }

    [TestFixture]
    public class Upc_PerformanceTests
    {
        [Test]
        public void UpcPerformance()
        {
            var count = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                12345678L.CalculateUpcOld();
            Console.WriteLine("Old " + sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < count; i++)
                12345678L.CalculateUpc();
            Console.WriteLine("New " + sw.Elapsed);
        }
    }
}