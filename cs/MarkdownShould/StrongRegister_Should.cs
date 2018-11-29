using FluentAssertions;
using Markdown.Registers;
using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class StrongRegister_Should
    {
        private StrongRegister register;

        [SetUp]
        public void SetUp()
        {
            register = new StrongRegister();
        }

        [TestCase("**foo bar**", 0, "foo bar")]
        [TestCase("foo**bar**", 3, "bar")]
        [TestCase("__foo bar__", 0, "foo bar")]
        [TestCase("__foo, __bar__, baz__", 0, "foo, __bar__, baz")]
        [TestCase("__some__", 0, "some")]
        public void BeWithStrongTag(string input, int startPos, string val)
        {
            var res = register.TryGetToken(input, startPos);
            res.ShouldBeEquivalentTo(new Token(val, "<strong>", "</strong>", 1, val.Length + 4, true));
        }

        [TestCase("5__6__78", 0, ExpectedResult = null)]
        [TestCase("__ foo bar__", 0, ExpectedResult = null)]
        [TestCase("** foo bar**", 0, ExpectedResult = null)]
        public Token BeWithoutStrongTag(string input, int startPos)
        {
            return register.TryGetToken(input, startPos);
        }
    }
}