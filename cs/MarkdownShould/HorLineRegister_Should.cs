using FluentAssertions;
using Markdown;
using NUnit.Framework;


namespace MarkdownShould
{
    [TestFixture]
    public class HorLineRegister_Should
    {
        private Markdown.Registers.HorLineRegister register;

        [SetUp]
        public void SetUp()
        {
            register = new Markdown.Registers.HorLineRegister();
        }

        [TestCase("***")]
        [TestCase("---")]
        [TestCase("___")]
        [TestCase("   ***")]
        [TestCase("* ***** ***** ********")]
        public void BeWithHRTag(string input)
        {
            var res = register.TryGetToken(input, 0);
            res.Should().Be(new Token("", "<hr />", "", 1, input.Length, false));
        }

        [TestCase("+++", ExpectedResult = null)]
        [TestCase("Foo\r\n    ***", ExpectedResult = null)]
        public Token BeWithoutHRTag(string input)
        {
            return register.TryGetToken(input, 0);
        }
    }
}
