using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public  class Extensions_Should
    {
        [TestCase("string", 3, true, 'i')]
        [TestCase("string", -1, false, default(char))]
        [TestCase("string", 6, false, default(char))]
        public void TryGetCharTests(string text, int index, bool expectedRes, char expectedChar)
        {
            var res = text.TryGetChar(index, out var ch);

            res.Should().Be(expectedRes);
            ch.Should().Be(expectedChar);
        }

        [TestCase("string", "str", 0, true)]
        [TestCase("string", "f", 0, false)]
        [TestCase("string", "ng", 6, false)]
        public void ContainsItOnIndexTests(string text, string substring, int index, bool expectedRes)
        {
            var res = text.ContainsItOnIndex(substring, index);

            res.Should().Be(expectedRes);
        }

        [TestCase("string", "ing", true)]
        [TestCase("string", "f", false)]
        [TestCase("string", "string123", false)]
        public void StringBuilderEndsWith(string text, string end, bool expectedRes)
        {
            var builder = new StringBuilder(text);
            var res = builder.EndsWith(end);

            res.Should().Be(expectedRes);
        }
    }
}
