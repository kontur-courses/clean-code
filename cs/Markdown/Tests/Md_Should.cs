using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public static class Md_Should
    {
        [TestCase("# Заголовок", ExpectedResult = "<h1>Заголовок</h1>", TestName = "RenderHeader")]
        [TestCase("# Заголовок\n", ExpectedResult = "<h1>Заголовок</h1>\n", TestName = "RenderHeader_WhenEndsWithNewLine")]
        [TestCase("\n# Заголовок", ExpectedResult = "\n<h1>Заголовок</h1>", TestName = "RenderHeader_WhenBeginsWithNewLine")]
        public static string RenderHeader(string markdown) => Md.Render(markdown);
    }
}