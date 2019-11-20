using FluentAssertions;
using Markdown;
using Markdown.RenderUtilities;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarkdownTests
{
    [TestFixture]
    public class Renderer_Should
    {
        private TokenReader tokenReader;
        private Renderer renderer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tokenReader = new TokenReader(MarkdownTokenUtilities.GetMarkdownTokenDescriptions());
            renderer = MarkdownRenderUtilities.GetMarkdownRenderer();
        }

        [TestCase("abcd")]
        [TestCase("abc12")]
        [TestCase("a b")]
        public void RenderSimpleText_Properly(string text)
        {
            var tokens = tokenReader.TokenizeText(text);

            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(text);
        }

        [Test]
        public void RenderEscapedText_Properly()
        {
            var text = @"\a\b\c\d\\";
            var expectedRenderedText = @"abcd\";

            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [Test]
        public void RenderEmptyText_Properly()
        {
            var resultText = renderer.RenderText(new List<Token>());

            resultText.Should().BeEquivalentTo("");
        }

        [Test]
        public void RenderEmphasisedText_Properly()
        {
            var text = "_a_";
            var expectedRenderedText = "<em>a</em>";

            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [Test]
        public void RenderStrongText_Properly()
        {
            var text = "__a__";
            var expectedRenderedText = "<strong>a</strong>";

            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [TestCase("_ab", TestName = "{m}EmphasisHasNoPair")]
        [TestCase("_1_2", TestName = "{m}EmphasisSurroundedByDigits")]
        [TestCase("_ ab_", TestName = "{m}EmphasisOpeningFollowedWithSpace")]
        [TestCase("_ab _", TestName = "{m}EmphasisClosingPrecededBySpace")]
        [TestCase("__ab", TestName = "{m}StrongHasNoPair")]
        [TestCase("__1__2", TestName = "{m}StrongSurroundedByDigits")]
        [TestCase("__ ab__", TestName = "{m}StrongOpeningFollowedWithSpace")]
        [TestCase("__ab __", TestName = "{m}StrongClosingPrecededBySpace")]
        public void RenderPairedTokensAsIs_When(string text)
        {
            var tokens = tokenReader.TokenizeText(text);

            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(text);
        }

        [Test]
        public void RenderEmphasisNestedInStrong_Properly()
        {
            var text = "__a _little_ fun__";
            var expectedRenderedText = "<strong>a <em>little</em> fun</strong>";

            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

    }
}
