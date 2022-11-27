using FluentAssertions;
using Markdown.Core.Entities;
using Markdown.Core.Entities.TagMaps;

namespace Markdown.Core.Tests
{
    [Parallelizable(ParallelScope.Self)]
    internal class PTagTests
    {
        private readonly PTag _sut = new PTag();
        
        [TestCase("qwerty any text", 0, "qwerty any text")]
        [TestCase("1__2__34", 0, "1__2__34")]
        [TestCase("            try\n       catch\n       finally", 0, "try\ncatch\nfinally")]
        [TestCase("aaa\nbbb\n\n", 0, "aaa\nbbb")]
        [TestCase("qwe\nrty\n\n", 0, "qwe\nrty")]
        [TestCase("__\nqwe rty__", 0, "__\nqwe rty__")]
        [TestCase("__\"qwe\"__", 0, "__\"qwe\"__")]
        public void TryGetToken_CorrectInputCases_ShouldBeEqual(string input, int startPos, string val)
        {
            var res = _sut.TryGetToken(input, startPos);
            res.Should().BeEquivalentTo(new Token(val, "<p>", "</p>", 0, input.Length, true));
        }

        [TestCase("", 0)]
        [TestCase("\n", 0)]
        public void TryGetToken_IncorrectInputCases_ShouldBeNull(string input, int startPos)
        {
            var res = _sut.TryGetToken(input, startPos);
            res.Should().BeEquivalentTo(new Token("", "", "", 0, input.Length, true));
        }
    }
}