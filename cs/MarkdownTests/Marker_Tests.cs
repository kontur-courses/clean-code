using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Marker_Tests
    {
        [TestCase("q1_2_3", 2, ExpectedResult = true)]
        [TestCase("_2_3", 0, ExpectedResult = true)]
        [TestCase("23_", 2, ExpectedResult = true)]
        [TestCase("q_w_e", 2, ExpectedResult = false)]
        public bool IsInsideDigits(string line, int pos)
        {
            return Marker.IsInsideDigits(line, pos);
        }

        [TestCase("_qwerty", 0, ExpectedResult = true)]
        [TestCase("qw _erty", 4, ExpectedResult = true)]
        [TestCase("qwe1_3rty", 4, ExpectedResult = false)]
        [TestCase("qwer_ ty", 4, ExpectedResult = false)]
        public bool IsOpeningTag(string line, int pos)
        {
            return Marker.IsOpeningTag(line, pos);
        }

        [TestCase("_qwerty", 0, ExpectedResult = false)]
        [TestCase("_qw _erty", 4, ExpectedResult = false)]
        [TestCase("qwe1_3rty", 4, ExpectedResult = false)]
        [TestCase("qwer_ ty", 4, ExpectedResult = true)]
        [TestCase("qwerty_", 6, ExpectedResult = true)]
        public bool IsClosingTag(string line, int pos)
        {
            return Marker.IsClosingTag(line, pos);
        }
    }
}