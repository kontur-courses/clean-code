using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public static class Md_Should
    {
        [TestCase("# Заголовок", "<h1>Заголовок</h1>",
            TestName = "RenderHeader")]
        [TestCase("# Заголовок\n", "<h1>Заголовок</h1>\n",
            TestName = "RenderHeader_WhenEndsWithNewLine")]
        [TestCase("\n# Заголовок", "\n<h1>Заголовок</h1>",
            TestName = "RenderHeader_WhenBeginsWithNewLine")]
        [TestCase("# __Заголовок__", "<h1><strong>Заголовок</strong></h1>",
            TestName = "RenderHeader_WithBold")]
        [TestCase("# _Заголовок_", "<h1><em>Заголовок</em></h1>",
            TestName = "RenderHeader_WithItalic")]
        public static void RenderHeader(string markdown, string expected)
            => Md.Render(markdown).Should().Be(expected);

        [TestCase("____", "____",
            TestName = "NotRenderBold_WithoutWords")]
        [TestCase("__    __", "__    __",
            TestName = "NotRenderBold_WithJustSpaces")]
        [TestCase("__  выделение__", "__  выделение__",
            TestName = "NotRenderBold_WhenBeginWithSpaceChar")]
        [TestCase("__выделение  __", "__выделение  __",
            TestName = "NotRenderBold_WhenEndWithSpaceChar")]
        [TestCase("__выделение", "__выделение",
            TestName = "NotRenderBold_WithoutPair")]
        [TestCase("__выделение__", "<strong>выделение</strong>",
            TestName = "RenderBold_WithOneWord")]
        [TestCase("__выделение из нескольких слов__", "<strong>выделение из нескольких слов</strong>",
            TestName = "RenderBold_WithSeveralWords")]
        [TestCase("Некоторый текст __выделение__ ещё текст", "Некоторый текст <strong>выделение</strong> ещё текст",
            TestName = "RenderBold_InMiddleOfLine")]
        [TestCase("__сло__во", "<strong>сло</strong>во",
            TestName = "RenderBold_InBeginOfWord")]
        [TestCase("сл__о__во", "сл<strong>о</strong>во",
            TestName = "RenderBold_InMiddleOfWord")]
        [TestCase("сло__во__", "сло<strong>во</strong>",
            TestName = "RenderBold_InEndOfWord")]
        [TestCase("_курсив __выделение___", "<em>курсив __выделение__</em>",
            TestName = "NotRenderBold_InsideItalic")]
        public static void RenderBold(string markdown, string expected)
            => Md.Render(markdown).Should().Be(expected);

        [TestCase("__", "__",
            TestName = "NotRenderItalic_WithoutWords")]
        [TestCase("__    __", "__    __",
            TestName = "NotRenderItalic_WithJustSpaces")]
        [TestCase("__  курсив__", "__  курсив__",
            TestName = "NotRenderItalic_WhenBeginWithSpaceChar")]
        [TestCase("__курсив  __", "__курсив  __",
            TestName = "NotRenderItalic_WhenEndWithSpaceChar")]
        [TestCase("_выделение", "_выделение",
            TestName = "NotRenderItalic_WithoutPair")]
        [TestCase("_курсив_", "<em>курсив</em>",
            TestName = "RenderItalic_WithOneWord")]
        [TestCase("_курсив из нескольких слов_", "<em>курсив из нескольких слов</em>",
            TestName = "RenderItalic_WithSeveralWords")]
        [TestCase("Некоторый текст _курсив_ ещё текст", "Некоторый текст <em>курсив</em> ещё текст",
            TestName = "RenderItalic_InMiddleOfLine")]
        [TestCase("_сло_во", "<em>сло</em>во",
            TestName = "RenderItalic_InBeginOfWord")]
        [TestCase("сл_о_во", "сл<em>о</em>во",
            TestName = "RenderItalic_InMiddleOfWord")]
        [TestCase("сло_во_", "сло<em>во</em>",
            TestName = "RenderItalic_InEndOfWord")]
        [TestCase("__выделение _курсив___", "<strong>выделение <em>курсив</em></strong>",
            TestName = "RenderItalic_InsideBold")]
        public static void RenderItalic(string markdown, string expected)
            => Md.Render(markdown).Should().Be(expected);

        [TestCase("__безподходящейпары_", "__безподходящейпары_",
            TestName = "NotRenderItalic_WithBoldPair")]
        [TestCase("_безподходящейпары__", "_безподходящейпары__",
            TestName = "NotRenderItalic_WithBoldPair")]
        [TestCase("_пересечение __разных_ выделений__", "_пересечение __разных_ выделений__",
            TestName = "NotRenderIntersections")]
        public static void NotRenderIntersections(string markdown, string expected)
            => Md.Render(markdown).Should().Be(expected);

        [TestCase("_123_", TestName = "NotRenderItalic_InDigits")]
        [TestCase("_12_3", TestName = "NotRenderItalic_InBeginOfWord_InDigits")]
        [TestCase("1_23_", TestName = "NotRenderItalic_InEndOfWord_InDigits")]
        [TestCase("1_23_4", TestName = "NotRenderItalic_InMiddleOfWord_InDigits")]
        [TestCase("a_123_b", TestName = "NotRenderItalic_InMiddleOfWord_InWordsWithDigits")]
        [TestCase("__a a_123_b b__", "<strong>a a_123_b b</strong>",
            TestName = "RenderBold_AroundWordsWithDigits_WithItalic")]
        [TestCase("__123__", TestName = "NotRenderBold_InDigits")]
        [TestCase("__12__3", TestName = "NotRenderBold_InBeginOfWord_InDigits")]
        [TestCase("1__23__", TestName = "NotRenderBold_InEndOfWord_InDigits")]
        [TestCase("1__23__4", TestName = "NotRenderBold_InMiddleOfWord_InDigits")]
        [TestCase("a__123__b", TestName = "NotRenderBold_InMiddleOfWord_InWordsWithDigits")]
        [TestCase("_a a__123__b b_", "<em>a a__123__b b</em>",
            TestName = "RenderItalic_AroundWordsWithDigits_WithBold")]
        public static void NotRender_WordsWithDigits(string markdown, string expected = null)
        {
            expected ??= markdown;
            Md.Render(markdown).Should().Be(expected);
        }

        [TestCase(@"\_abcd\_", "_abcd_", TestName = "Render_EscapedChars")]
        [TestCase(@"\_\_abcd\_\_", "__abcd__", TestName = "Render_EscapedChars")]
        [TestCase(@"\__abcd_\_", "_<em>abcd</em>_", TestName = "Render_EscapedChars")]
        [TestCase(@"_\_abcd\__", "<em>_abcd_</em>", TestName = "Render_EscapedChars")]
        [TestCase(@"\ab\  c\d", @"\ab\  c\d", TestName = "NotRender_NotEscapedChars")]
        [TestCase(@"\\_abcd\\_", @"\<em>abcd\</em>", TestName = "Render_EscapedBackslash")]
        public static void Render_EscapedChars(string markdown, string expected = null)
        {
            expected ??= markdown;
            Md.Render(markdown).Should().Be(expected);
        }

        [TestCase(
            @"[google!](https://google.com)",
            @"<a href=""https://google.com"">google!</a>",
            TestName = "Render_Links")]
        [TestCase(
            @"[__google__!](https://google.com)",
            @"<a href=""https://google.com""><strong>google</strong>!</a>",
            TestName = "Render_BoldLinks")]
        [TestCase(
            @"[_google_!](https://google.com)",
            @"<a href=""https://google.com""><em>google</em>!</a>",
            TestName = "Render_ItalicLinks")]
        [TestCase(
            @"[ссылка с _курсивом_ и __выделением__](ссылка)",
            @"<a href=""ссылка"">ссылка с <em>курсивом</em> и <strong>выделением</strong></a>",
            TestName = "Render_BoldAndItalicLinks_WithSpaces")]
        public static void Render_Links(string markdown, string expected = null)
        {
            expected ??= markdown;
            Md.Render(markdown).Should().Be(expected);
        }
    }
}