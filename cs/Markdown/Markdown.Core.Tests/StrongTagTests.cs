using FluentAssertions;
using Markdown.Core.Entities;
using Markdown.Core.Entities.TagMaps;

namespace Markdown.Core.Tests
{
    [Parallelizable(ParallelScope.Self)]
    internal class StrongTagTests
    {
        private readonly StrongTag _sut = new StrongTag();
            
        [TestCase("**qwe rty**", 0, "qwe rty")]
        [TestCase("qwe**rty**", 3, "rty")]
        [TestCase("__qwe rty__", 0, "qwe rty")]
        [TestCase("__qwe, __rty__, uio__", 0, "qwe, __rty__, uio")]
        [TestCase("__qwerty__", 0, "qwerty")]
        public void TryGetToken_CorrectInputCases_ShouldBeEqual(string input, int startPos, string val)
        {
            var res = _sut.TryGetToken(input, startPos);
            res.Should().BeEquivalentTo(new Token(val, "<strong>", "</strong>", 1, val.Length + 4, true));
        }

        [TestCase("3__4__31", 0)]
        [TestCase("__ qwe rty__", 0)]
        [TestCase("** qwe rty**", 0)]
        public void TryGetToken_IncorrectInputCases_ShouldBeNull(string input, int startPos)
        {
           _sut.TryGetToken(input, startPos).Should().BeNull();
        }
    }
}
