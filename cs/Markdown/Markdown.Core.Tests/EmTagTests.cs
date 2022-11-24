using FluentAssertions;
using Markdown.Core.Entities;
using Markdown.Core.Entities.TagMaps;

namespace Markdown.Core.Tests
{
    internal class EmTagTests
    {
        private readonly EmTag _emTag = new EmTag();
        
        [TestCase("*qwe rty*", 0, "qwe rty")]
        [TestCase("*(*qwe*)*", 0, "(*qwe*)")]
        [TestCase("_(qwe)_", 0, "(qwe)")]
        [TestCase("_qwe rty_", 0, "qwe rty")]
        [TestCase("1*2*34", 1, "2")]
        public void TryGetToken_CorrectInputCases_ShouldBeEqual(string input, int startPos, string val)
        {
            var res = _emTag.TryGetToken(input, startPos);
            res.Should().BeEquivalentTo(new Token(val, "<em>", "</em>", 0, val.Length + 2, true));
        }

        [TestCase("")]
        [TestCase("a * qwe rty*")]
        [TestCase("* q *")]
        [TestCase("qwe_rty_")]
        [TestCase("_qwe*")]
        public void TryGetToken_IncorrectInputCases_ShouldBeNull(string input)
        {
            _emTag.TryGetToken(input, 0).Should().BeNull();
        }
    }
}