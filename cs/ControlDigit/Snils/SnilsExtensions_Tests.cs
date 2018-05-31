using System;
using System.Diagnostics;
using NUnit.Framework;

namespace ControlDigit
{
    [TestFixture]
    public class SnilsExtensions_Tests
    {
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
    }
}