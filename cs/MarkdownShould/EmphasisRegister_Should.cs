using Markdown.Registers;
using Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownShould
{
    [TestFixture]
    public class EmphasisRegister_Should
    {
        private EmphasisRegister register;

        [SetUp]
        public void SetUp()
        {
            register = new EmphasisRegister();
        }

        [TestCase("*foo bar*", 0, "foo bar")]
        [TestCase("*(*foo*)*", 0, "(*foo*)")]
        [TestCase("_(_foo_)_", 0, "(_foo_)")]
        [TestCase("_foo bar_", 0, "foo bar")]
        [TestCase("5*6*78", 1, "6")]
        public void BeWithEmphasisTag(string input, int startPos, string val)
        {
            var res = register.TryGetToken(input, startPos);
            res.Should().Be(new Token(val, "<em>", "</em>", 0, val.Length + 2, true));
        }

        [TestCase("", ExpectedResult = null)]
        [TestCase("a * foo bar*", ExpectedResult = null)]
        [TestCase("* a *", ExpectedResult = null)]
        [TestCase("foo_bar_", ExpectedResult = null)]
        [TestCase("_foo*", ExpectedResult = null)]
        public Token BeWithoutEmphasisTag(string input)
        {
            return register.TryGetToken(input, 0);
        }
    }
}
