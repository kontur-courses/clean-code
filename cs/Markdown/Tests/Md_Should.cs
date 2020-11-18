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
        [TestCase("__выделение__",
            ExpectedResult = "<strong>выделение</strong>",
            TestName = "RenderBold_WithOneWord")]
        [TestCase("__выделение из нескольких слов__",
            ExpectedResult = "<strong>выделение из нескольких слов</strong>",
            TestName = "RenderBold_WithSeveralWords")]
        [TestCase("Некоторый текст __выделение__ ещё текст",
            ExpectedResult = "Некоторый текст <strong>выделение</strong> ещё текст",
            TestName = "RenderBold_InMiddleOfLine")]
        public static string RenderBold(string markdown) => Md.Render(markdown);
        
        [TestCase("__",
            ExpectedResult = "__",
            TestName = "NotRenderItalic_WithoutWords")]
        [TestCase("_курсив_",
            ExpectedResult = "<em>курсив</em>",
            TestName = "RenderItalic_WithOneWord")]
        [TestCase("_курсив из нескольких слов_",
            ExpectedResult = "<em>курсив из нескольких слов</em>",
            TestName = "RenderItalic_WithSeveralWords")]
        [TestCase("Некоторый текст _курсив_ ещё текст",
            ExpectedResult = "Некоторый текст <em>курсив</em> ещё текст",
            TestName = "RenderItalic_InMiddleOfLine")]
        public static string RenderItalic(string markdown) => Md.Render(markdown);
    }
}