using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests.TokenExtensions_Tests
{
    class TokenExtensions_GetOccupiedIndexes_Tests
    {
        [TestCase(0, 5, "asdqwexz")]
        [TestCase(7, 3, "1234567890")]
        public void ShouldReturnOccupiedIndexes(int startIndex, int length, string text)
        {
            var token = new Token { StartIndex = startIndex, Length = length, Str = text };
            var expected = new List<int>();
            for (var i = startIndex; i < startIndex + length; i++)
                expected.Add(i);

            var actual = token.GetOccupiedIndexes();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}