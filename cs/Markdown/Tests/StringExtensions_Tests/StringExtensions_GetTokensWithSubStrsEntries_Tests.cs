using System;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests.StringExtensions_Tests
{
    class StringExtensions_GetTokensWithSubStrsEntries_Tests
    {
        [Test]
        public void ShouldThrow_WhenSubStrsIsNull()
        {
            var str = "123asd";
            Action act = () => str.GetTokensWithSubStrsEntries(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldReturnTokensWithAllSubStrsEntries()
        {
            var str = "*zxc*qwe**vcxqwe**";

            var result = str.GetTokensWithSubStrsEntries(new[] { "*", "**", "*" });

            result.Should().BeEquivalentTo(
                new Token { StartIndex = 0, Length = 1, Str = str },
                new Token { StartIndex = 4, Length = 1, Str = str },
                new Token { StartIndex = 8, Length = 1, Str = str },
                new Token { StartIndex = 9, Length = 1, Str = str },
                new Token { StartIndex = 16, Length = 1, Str = str },
                new Token { StartIndex = 17, Length = 1, Str = str },
                new Token { StartIndex = 8, Length = 2, Str = str },
                new Token { StartIndex = 16, Length = 2, Str = str });
        }

        [TestCase("")]
        [TestCase("qwe", "")]
        [TestCase("a", "asd", "qwe")]
        [TestCase("qweertrtydddsfdfgsdfas", "_", "__")]
        public void ShouldReturnEmptyEnumeration(string str, params string[] subStrs)
        {
            var result = str.GetTokensWithSubStrsEntries(subStrs);

            result.Should().BeEmpty();
        }
    }
}