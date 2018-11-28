using FluentAssertions;
using Markdown;
using Markdown.Registers;
using NUnit.Framework;


namespace MarkdownShould
{
    [TestFixture]
    public class HeaderRegister_Should
    {
        private HeaderRegister register;

        [SetUp]
        public void SetUp()
        {
            register = new HeaderRegister();
        }


        [TestCase("# foo", "foo", 1)]
        [TestCase("#### foo", "foo", 4)]
        [TestCase("###### foo", "foo", 6)]
        [TestCase("#                  foo                  ", "foo", 1)]
        [TestCase("  ## foo", "foo", 2)]
        [TestCase("###   bar    ###", "bar", 3)]
        [TestCase("### foo ### b", "foo ### b", 3)]
        [TestCase("# foo#", "foo#", 1)]
        [TestCase("#", "", 1)]
        [TestCase("### ###", "", 3)]
        [TestCase("### felix   ", "felix", 3)]
        public void BeWithHTag(string input, string value, int level)
        {
            var res = register.TryGetToken(input, 0);
            res.Should().Be(new Token(value, $"<h{level}>", $"</h{level}>", 1, input.Length, false));
        }


        [TestCase("####### foo", ExpectedResult = null)]
        [TestCase("#5 bolt", ExpectedResult = null)]
        [TestCase("\\## foo", ExpectedResult = null)]
        [TestCase("    # foo", ExpectedResult = null)]
        public Token BeWithoutHTag(string input)
        {
            return register.TryGetToken(input, 0);
        }
    }
}
