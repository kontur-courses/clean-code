using NUnit.Framework;

namespace ControlDigit
{
    [TestFixture]
    public class UpcExtensions_Tests
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
    }
}