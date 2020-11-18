using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public static class Md_Should
    {
        [TestCase("# Заголовок",
            ExpectedResult = "<h1>Заголовок</h1>",
            TestName = "RenderHeader")]
        [TestCase("# Заголовок\n",
            ExpectedResult = "<h1>Заголовок</h1>\n",
            TestName = "RenderHeader_WhenEndsWithNewLine")]
        [TestCase("\n# Заголовок",
            ExpectedResult = "\n<h1>Заголовок</h1>",
            TestName = "RenderHeader_WhenBeginsWithNewLine")]
        public static string RenderHeader(string markdown) => Md.Render(markdown);
        
        [TestCase("____",
            ExpectedResult = "____",
            TestName = "NotRenderBold_WithoutWords")]
        [TestCase("__    __",
            ExpectedResult = "__    __",
            TestName = "NotRenderBold_WithJustSpaces")]
        [TestCase("__  выделение__",
            ExpectedResult = "__  выделение__",
            TestName = "NotRenderBold_WhenBeginWithSpaceChar")]
        [TestCase("__выделение  __",
            ExpectedResult = "__выделение  __",
            TestName = "NotRenderBold_WhenEndWithSpaceChar")]
        [TestCase("__выделение__",
            ExpectedResult = "<strong>выделение</strong>",
            TestName = "RenderBold_WithOneWord")]
        [TestCase("__выделение из нескольких слов__",
            ExpectedResult = "<strong>выделение из нескольких слов</strong>",
            TestName = "RenderBold_WithSeveralWords")]
        [TestCase("Некоторый текст __выделение__ ещё текст",
            ExpectedResult = "Некоторый текст <strong>выделение</strong> ещё текст",
            TestName = "RenderBold_InMiddleOfLine")]
        [TestCase("__сло__во",
            ExpectedResult = "<strong>сло</strong>во",
            TestName = "RenderBold_InBeginOfWord")]
        [TestCase("сл__о__во",
            ExpectedResult = "сл<strong>о</strong>во",
            TestName = "RenderBold_InMiddleOfWord")]
        [TestCase("сло__во__",
            ExpectedResult = "сло<strong>во</strong>",
            TestName = "RenderBold_InEndOfWord")]
        public static string RenderBold(string markdown) => Md.Render(markdown);
        
        [TestCase("__",
            ExpectedResult = "__",
            TestName = "NotRenderItalic_WithoutWords")]
        [TestCase("__    __",
            ExpectedResult = "__    __",
            TestName = "NotRenderItalic_WithJustSpaces")]
        [TestCase("__  курсив__",
            ExpectedResult = "__  курсив__",
            TestName = "NotRenderItalic_WhenBeginWithSpaceChar")]
        [TestCase("__курсив  __",
            ExpectedResult = "__курсив  __",
            TestName = "NotRenderItalic_WhenEndWithSpaceChar")]
        [TestCase("_курсив_",
            ExpectedResult = "<em>курсив</em>",
            TestName = "RenderItalic_WithOneWord")]
        [TestCase("_курсив из нескольких слов_",
            ExpectedResult = "<em>курсив из нескольких слов</em>",
            TestName = "RenderItalic_WithSeveralWords")]
        [TestCase("Некоторый текст _курсив_ ещё текст",
            ExpectedResult = "Некоторый текст <em>курсив</em> ещё текст",
            TestName = "RenderItalic_InMiddleOfLine")]
        [TestCase("_сло_во",
            ExpectedResult = "<em>сло</em>во",
            TestName = "RenderItalic_InBeginOfWord")]
        [TestCase("сл_о_во",
            ExpectedResult = "сл<em>о</em>во",
            TestName = "RenderItalic_InMiddleOfWord")]
        [TestCase("сло_во_",
            ExpectedResult = "сло<em>во</em>",
            TestName = "RenderItalic_InEndOfWord")]
        public static string RenderItalic(string markdown) => Md.Render(markdown);
    }
}