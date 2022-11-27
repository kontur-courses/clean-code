using FluentAssertions;
using Markdown.Core.Entities;
using Markdown.Core.Entities.TagMaps;

namespace Markdown.Core.Tests
{
    [Parallelizable(ParallelScope.Self)]
    internal class HeaderTagTests
    {
        private readonly HeaderTag _sut = new HeaderTag();

        [TestCase("# qwe", "qwe", 1)]
        [TestCase("#### qwe", "qwe", 4)]
        [TestCase("###### rty", "rty", 6)]
        [TestCase("#                  qwe                  ", "qwe", 1)]
        [TestCase("  ## qwe", "qwe", 2)]
        [TestCase("###   qwe    ###", "qwe", 3)]
        [TestCase("### qwe ### q", "qwe ### q", 3)]
        [TestCase("# qwe#", "qwe#", 1)]
        [TestCase("#", "", 1)]
        [TestCase("### ###", "", 3)]
        [TestCase("### qwerty   ", "qwerty", 3)]
        public void TryGetToken_CorrectInputCases_ShouldBeEqual(string input, string value, int level)
        {
            var res = _sut.TryGetToken(input, 0);
            res.Should().BeEquivalentTo(new Token(value, $"<h{level}>", $"</h{level}>", 1, input.Length, false));
        }

        [TestCase("####### qwe")]
        [TestCase("#5 qwe")]
        [TestCase("\\## qwe")]
        [TestCase("    # qwe")]
        public void TryGetToken_IncorrectInputCases_ShouldBeNull(string input)
        {
            _sut.TryGetToken(input, 0).Should().BeNull();
        }
    }
}