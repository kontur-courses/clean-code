using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests.StringExtensions_Tests
{
    class StringExtensions_GetAllSubStrEntries_Tests
    {
        [Test]
        public void ShouldThrow_WhenSubStrsIsNull()
        {
            var str = "123asd";
            Action act = () => str.GetAllSubStrEntries(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldReturnAllSubStrsEntries()
        {
            var str = "*zxc*qwe**vcxqwe**";

            var result = str.GetAllSubStrEntries(new[] { "*", "**", "*" });

            result.Should().BeEquivalentTo(
                new Token() { StartIndex = 0, Count = 1, Str = str },
                new Token() { StartIndex = 4, Count = 1, Str = str },
                new Token() { StartIndex = 8, Count = 1, Str = str },
                new Token() { StartIndex = 9, Count = 1, Str = str },
                new Token() { StartIndex = 16, Count = 1, Str = str },
                new Token() { StartIndex = 17, Count = 1, Str = str },
                new Token() { StartIndex = 8, Count = 2, Str = str },
                new Token() { StartIndex = 16, Count = 2, Str = str });
        }
    }
}