using System;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics;

namespace Markdown
{
    [TestFixture]
    class Render_should
    {        
        [TestCase("_work_", "<em>work</em>", TestName = "AllStringIsItalic")]
        [TestCase("a_work_", "a<em>work</em>", TestName = "CharBeforeItalic")]
        [TestCase("_work_a", "<em>work</em>a", TestName = "CharAfterItalic")]
        [TestCase("__work__", "<strong>work</strong>", TestName = "AllStringIsStrong")]
        [TestCase("__work__a", "<strong>work</strong>a", TestName = "CharBeforeStrong")]
        [TestCase("a__work__", "a<strong>work</strong>", TestName = "CharAfterStrong")]        
        public void RenderShouldWorkCorrect_OnTextWithSimpleToken(string text, string expectedRenderedText)
        {
            var resultText = new Renderer().Render(text);
            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [TestCase("_work_ __work__", "<em>work</em> <strong>work</strong>", TestName = "StrongAfterItalic")]
        [TestCase("__work__ _work_", "<strong>work</strong> <em>work</em>", TestName = "ItalicAfterStrong")]        
        public void RenderShouldWorkCorrect_OnTextWithSeveralSimpleTokens(string text, string expectedRenderedText)
        {
            var resultText = new Renderer().Render(text);
            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [TestCase("__should _work_ well__ work", "<strong>should <em>work</em> well</strong> work", TestName = "ItalicInsideStrong")]
        [TestCase("_should __work__ well_ work", "<em>should work well</em> work", TestName = "StrongInsideItalic")]
        [TestCase("_should __work__ well_ __work__", "<em>should work well</em> <strong>work</strong>", TestName = "StrongAfterItalicWithStrongInside")]
        [TestCase("__should _work_ well__ _work_", "<strong>should <em>work</em> well</strong> <em>work</em>", TestName = "ItalicAfterStrongWithItalicInside")]
        [TestCase("__a _b_ _c_ d__", "<strong>a <em>b</em> <em>c</em> d</strong>", TestName = "TwoItalicInsideStrong")]
        [TestCase("_a __b__ __c__ d_", "<em>a b c d</em>", TestName = "TwoStrongInsideItalic")]
        [TestCase("__a _b_ _c_ _d_ e__", "<strong>a <em>b</em> <em>c</em> <em>d</em> e</strong>", TestName = "ThreeItalicInsideStrong")]
        public void RenderShouldWorkCorrect_OnTextWithTokensInsideTokens(string text, string expectedRenderedText)
        {
            var resultText = new Renderer().Render(text);            
            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }

        [TestCase(@"\_work_", "_work_", TestName = "BackslashBeforeItalicOpen")]
        [TestCase(@"_work\_", "_work_", TestName = "BackslashBeforeItalicClose")]
        [TestCase(@"\__work__", "__work__", TestName = "BackslashBeforeStrongOpen")]
        [TestCase(@"__work\__", "__work__", TestName = "BackslashBeforeStrongClose")]
        public void RenderShouldWorkCorrect_OnEscapedText(string text, string expectedRenderedText)
        {
            var resultText = new Renderer().Render(text);            
            resultText.Should().BeEquivalentTo(expectedRenderedText);
        }



        [TestCase("_should __not work", TestName = "ItalicOpenAndStrongOpen")]
        [TestCase("__should _not", TestName = "StrongOpenAndItalicOpen")]
        public void RenderShouldWorkCorrect_OnNotPairTags(string text)
        {
            var resultString = new Renderer().Render(text);
            resultString.Should().BeEquivalentTo(text);
        }

        [TestCase("_should_ _not work", "<em>should</em> _not work",  TestName = "ThreeItalic")]
        [TestCase("__should__ __not work", "<strong>should</strong> __not work", TestName = "ThreeStrong")]
        public void RenderShouldWorkCorrect_OnNotEvenAmountOfTags(string text, string expectedString)
        {
            var resultString = new Renderer().Render(text);
            resultString.Should().BeEquivalentTo(expectedString);
        }

        [TestCase("_3_4", TestName = "ItalicSurroundedByDigitsAfter")]                
        [TestCase("3_4", TestName = "ItalicSurroundedByDigitsBefore")]
        [TestCase("3__4__", TestName = "StrongSurroundedByDigitsBefore")]
        [TestCase("__3__4", TestName = "StrongSurroundedByDigitsAfter")]        
        public void RenderShouldWorkCorrect_OnTextWithTokenSurroundedByDigits(string text)
        {
            var resultString = new Renderer().Render(text);
            resultString.Should().BeEquivalentTo(text);
        }
              
        [TestCase(500)]
        [TestCase(5000)]
        [TestCase(50000)]        
        public void RenderShouldWorkFast(int count)
        {
            var nestedText = new StringBuilder();
            for (var i = 0; i < count; i++)
                nestedText.Append("_abc ");
            for (var i = 0; i < count; i++)
                nestedText.Append(" abc_");
            var text = nestedText.ToString();
            var resultText = new Renderer();
            var time =
                MeasureTime((t) => resultText.Render(t), text);
            time.Should().BeLessOrEqualTo(count);
        }

        private double MeasureTime(Func<string, string> renderText, string text)
        {
            var repeatCount = 10;
            var timer = Stopwatch.StartNew();
            for (var i = 0; i < repeatCount; i++)
                renderText(text);
            timer.Stop();
            return timer.Elapsed.TotalMilliseconds / repeatCount;
        }
    }
}
