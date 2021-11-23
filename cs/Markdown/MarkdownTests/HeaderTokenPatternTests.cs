using FluentAssertions;
using Markdown;
using Markdown.Tokens.Patterns;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HeaderTokenPatternTests
    {
        [Test]
        public void TryStart_False_IfNoSpaceAfterSharp()
        {
            var context = new Context("#");
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_False_IfSharpInLineMiddleButLineStartsCorrect()
        {
            var context = new Context("# qwe # ", 6);
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeFalse();
        }

        [Test]
        public void TryStart_True_IfSpaceAfterSharp()
        {
            var context = new Context("# ");
            new HeaderTokenPattern()
                .TrySetStart(context)
                .Should().BeTrue();
        }

        [Test]
        public void TryStart_True_IfLetterAfterSpace()
        {
            var context = new Context("# a");
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
            var context = new Context($"abc{text}def");
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
        public void TryContinue_False_IfLineEnd()
        {
            var context = new Context("a", 1);
            new HeaderTokenPattern()
                .TryContinue(context)
                .Should().BeFalse();
        }

        [Test]
        public void LastCloseSucceed_True_IfEndingCorrect()
        {
            var context = new Context("qwe", 3);
            var pattern = new HeaderTokenPattern();

            pattern.TryContinue(context);

            pattern.LastEndingSucceed.Should().BeTrue();
        }
    }
}