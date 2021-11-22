using FluentAssertions;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HeaderTokenPatternTests
    {
        [Test]
        public void TryStart_False_WithoutSpaceAfter()
        {
            var context = new Context("#");
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_False_InLineMiddleWhenLineStartsCorrect()
        {
            var context = new Context("# qwe # ", 6);
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_True_WithSpaceAfter()
        {
            var context = new Context("# ");
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeTrue();
        }

        [Test]
        public void TryStart_True_LetterAfterSpace()
        {
            var context = new Context("# ");
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeTrue();
        }

        [TestCase("a")]
        [TestCase(" ")]
        [TestCase("\t")]
        [TestCase("_")]
        public void TryContinue_True_After(string text)
        {
            var context = new Context(text + "qwe");
            new HeaderTokenPattern()
                .TryContinue(context)
                .Should().BeTrue();
        }

        [TestCase("\n")]
        [TestCase("\r")]
        public void TryContinue_False_AfterNewLineChar(string ending)
        {
            var context = new Context(ending);
            new HeaderTokenPattern()
                .TryContinue(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryContinue_False_LineEnd()
        {
            var context = new Context("a", 1);
            new HeaderTokenPattern()
                .TryContinue(context)
                .Should().BeFalse();
        }

        [Test]
        public void LastCloseSucceed_True_CorrectEnding()
        {
            var context = new Context("qwe", 3);
            var pattern = new HeaderTokenPattern();

            pattern.TryContinue(context);

            pattern.LastCloseSucceed.Should().BeTrue();
        }
    }
}