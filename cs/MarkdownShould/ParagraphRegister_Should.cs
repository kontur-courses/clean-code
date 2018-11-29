using FluentAssertions;
using Markdown;
using NUnit.Framework;
using Markdown.Registers;

namespace MarkdownShould
{
    [TestFixture]
    public class ParagraphRegister_Should
    {
        private ParagraphRegister register;

        [SetUp]
        public void SetUp()
        {
            register = new ParagraphRegister();
        }


        [TestCase("some simple text", 0, "some simple text")]
        [TestCase("5__6__78", 0, "5__6__78")]
        [TestCase("            some\n       another\n                 text", 0, "some\nanother\ntext")]
        [TestCase("aaa\nbbb\n\n", 0, "aaa\nbbb")]
        [TestCase("some\ndiff\n\n", 0, "some\ndiff")]
        [TestCase("__\nfoo bar__", 0, "__\nfoo bar__")]
        [TestCase("__\"foo\"__", 0, "__\"foo\"__")]
        public void ShouldBeWithParagraphTag(string input, int startPos, string val)
        {
            var res = register.TryGetToken(input, startPos);
            res.ShouldBeEquivalentTo(new Token(val, "<p>", "</p>", 0, input.Length, true));
        }

        [TestCase("", 0)]
        [TestCase("\n", 0)]
        public void ShouldBeEmpty(string input, int startPos)
        {
            var res = register.TryGetToken(input, startPos);
            res.ShouldBeEquivalentTo(new Token("", "", "", 0, input.Length, true));
        }
    }
}
