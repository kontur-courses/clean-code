using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Md_Should
    {
        private Md md = new Md();

        [TestCase("_text_", "<em>text</em>")]
        [TestCase("_te xt_", "<em>te xt</em>")]
        [TestCase("_te0xt_", "<em>te0xt</em>")]
        [TestCase(@"_te\\xt_", @"<em>te\xt</em>")]
        [TestCase("_te_xt_", "<em>te</em>xt_")]
        [TestCase(@"_te\_xt_", "<em>te_xt</em>")]
        [TestCase("_3text5_", "<em>3text5</em>")]
        public static void Render_TextSurroundedByUnderscore_ShouldBeEmphasised(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public static void Render_NotEscapedEscapeChar_ThrowException()
        {
            var text = @"_te\xt_";

            Action action = () => Md.Render(text);

            action.Should()
                .Throw<NotSupportedException>();
        }

        [TestCase(@"\_text\_", "_text_")]
        [TestCase(@"\_\_text\_\_", "__text__")]
        [TestCase(@"\\", @"\")]
        public static void Render_AnySymbolCanBeEscaped(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("__text__", "<strong>text</strong>")]
        [TestCase("__te xt__", "<strong>te xt</strong>")]
        [TestCase("__te0xt__", "<strong>te0xt</strong>")]
        [TestCase(@"__te\\xt__", @"<strong>te\xt</strong>")]
        [TestCase("__te__xt__", "<strong>te</strong>xt__")]
        [TestCase(@"__te\_xt__", "<strong>te_xt</strong>")]
        [TestCase("__3text5__", "<strong>3text5</strong>")]
        public static void Render_TextSurroundedByDoubleUnderscore_ShouldBeBolded(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("__8_text_8__", "<strong>8<em>text</em>8</strong>")]
        [TestCase("__hello_text_hello__", "<strong>hello<em>text</em>hello</strong>")]
        public static void Render_InsideBoldedTextCanBeEmphasisedText(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("_8__text__8_", "<em>8__text__8</em>")]
        [TestCase("_hello__text__hello_", "<em>hello__text__hello</em>")]
        public static void Render_InsideEmphasisedTextCanNotBeBoldText(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("1_2_3", "1_2_3")]
        [TestCase("1__2__3", "1__2__3")]
        public static void Render_UnderscoreBetweenNumbersDoNotProcess(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("_text__", "_text__")]
        [TestCase("__text_", "__text_")]
        public static void Render_NotPairedUnderscoresDoNotProcess(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("_ hello", "_ hello")]
        public static void Render_IfAfterOpenUnderscoreFollowWhitespace_DoNotProcess(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("hello _", "hello _")]
        public static void Render_IfWhitespaceBeforeCloseUnderscore_DoNotProcess(string text, string expected)
        {
            Md.Render(text)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}