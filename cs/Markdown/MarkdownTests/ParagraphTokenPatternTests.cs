using FluentAssertions;
using Markdown;
using Markdown.Tokens.Patterns;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ParagraphTokenPatternTests
    {
        private ParagraphTokenPattern headerPattern;

        [SetUp]
        public void SetUp()
        {
            headerPattern = new ParagraphTokenPattern("#");
        }

        [Test]
        public void TryStart_False_IfNoSpaceAfterSharp()
        {
            var context = new Context("#");
            headerPattern
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_False_IfSharpInLineMiddleButLineStartsCorrect()
        {
            var context = new Context("# qwe # ", 6);
            headerPattern
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_True_IfSpaceAfterSharp()
        {
            var context = new Context("# ");
            headerPattern
                .TrySetStart(context)
                .Should().BeTrue();
        }

        [Test]
        public void TryStart_True_IfLetterAfterSpace()
        {
            var context = new Context("# a");
            headerPattern
                .TrySetStart(context)
                .Should().BeTrue();
        }

        [TestCase("a")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("_")]
        public void TryContinue_True_After(string text)
        {
            var context = new Context($"abc{text}def");
            headerPattern
                .TryContinue(context)
                .Should().BeTrue();
        }

        [TestCase("\n")]
        [TestCase("\r")]
        public void TryContinue_False_AfterNewLineChar(string ending)
        {
            var context = new Context(ending);
            headerPattern
                .TryContinue(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryContinue_False_IfLineEnd()
        {
            var context = new Context("a", 1);
            headerPattern
                .TryContinue(context)
                .Should().BeFalse();
        }

        [Test]
        public void LastCloseSucceed_True_IfEndingCorrect()
        {
            var context = new Context("qwe", 3);
            headerPattern.TryContinue(context);
            headerPattern.LastEndingSucceed.Should().BeTrue();
        }
    }
}