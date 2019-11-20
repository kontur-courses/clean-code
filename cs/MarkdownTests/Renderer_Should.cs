using FluentAssertions;
using Markdown;
using Markdown.RenderUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        [TestCase("1_a_", "1<em>a</em>", TestName = "{m}DigitsOnLeftOfEmphasisOpening")]
        [TestCase("_1a_", "<em>1a</em>", TestName = "{m}DigitsOnRightOfEmphasisOpening")]
        [TestCase("_a1_", "<em>a1</em>", TestName = "{m}DigitsOnLeftOfEmphasisClosing")]
        [TestCase("_a_1", "<em>a</em>1", TestName = "{m}DigitsOnRightOfEmphasisOpening")]
        [TestCase("1__a__", "1<strong>a</strong>", TestName = "{m}DigitsOnLeftOfStrongOpening")]
        [TestCase("__1a__", "<strong>1a</strong>", TestName = "{m}DigitsOnRightOfStrongOpening")]
        [TestCase("__a1__", "<strong>a1</strong>", TestName = "{m}DigitsOnLeftOfStrongClosing")]
        [TestCase("__a__1", "<strong>a</strong>1", TestName = "{m}DigitsOnRightOfStrongOpening")]
        public void RenderPairedTokensNearDigits_When(string text, string expectedRenderedText)
        {
            var tokens = tokenReader.TokenizeText(text);

            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
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

        [Test]
        public void RenderIntersectingPairedTags_Properly()
        {
            var text = "__a _little __bit of_ fun__";
            var expectedRenderedText = "<strong>a <em>little __bit of</em> fun</strong>";

            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);

            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(25000)]
        public void RenderFast_ForNestedPairedText(int nestLevel)
        {
            var nestedText = new StringBuilder();
            for (var i = 0; i < nestLevel; i++)
                nestedText.Append("_a ");
            for (var i = 0; i < nestLevel; i++)
                nestedText.Append(" a_");
            var nestedTokens = tokenReader.TokenizeText(nestedText.ToString());

            var renderingTime = 
                MeasureAverageRenderingTime((tokens) => renderer.RenderText(tokens), nestedTokens);

            renderingTime.Should().BeLessOrEqualTo(nestLevel);
        }

        private double MeasureAverageRenderingTime(Func<List<Token>, string> renderText, 
            List<Token> tokens)
        {
            var repeatCount = 5;
            var timer = Stopwatch.StartNew();
            for (var i = 0; i < repeatCount; i++)
                renderText(tokens);
            timer.Stop();

            return timer.Elapsed.TotalMilliseconds / repeatCount;
        }

    }
}
