using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase(
            "Текст _окруженный с двух сторон_ одинарными символами подчерка",
            ExpectedResult = "Текст <em>окруженный с двух сторон</em> одинарными символами подчерка",
            TestName = "Text in <em> tag on pair underscore")]
        [TestCase(
            "Текст __окруженный с двух сторон__ двумя символами подчерка",
            ExpectedResult = "Текст <strong>окруженный с двух сторон</strong> двумя символами подчерка",
            TestName = "Text in <strong> tag on pair underscore")]
        [TestCase(
            @"Здесь \_подчеркивания экранированы\_",
            ExpectedResult = "Здесь _подчеркивания экранированы_",
            TestName = "Raw text on escaped underscore"
            )]
        [TestCase(
            "В __двойном выделении _одинарное_ выделение__",
            ExpectedResult = "В <strong>двойном выделении <em>одинарное</em> выделение</strong>",
            TestName = "One underscore in double underscore works"
            )]
        [TestCase(
            "В _одинарном выделении __двойное__ выделение_",
            ExpectedResult = "В <em>одинарном выделении __двойное__ выделение</em>",
            TestName = "Double underscore in one underscore doesn't' work"
        )]
        [TestCase(
            "Подчерки внутри текста c цифрами_12_3",
            ExpectedResult = "Подчерки внутри текста c цифрами_12_3",
            TestName = "Raw text on undescores in text with numbers"
            )]
        [TestCase(
            "Подчерки внутри текста это_не_выделение",
            ExpectedResult = "Подчерки внутри текста это_не_выделение",
            TestName = "Raw text on undescores in text"
        )]
        [TestCase(
            @"Это_ не_ выделение_",
            ExpectedResult = @"Это_ не_ выделение_",
            TestName = "Raw text on not closed underscores"
            )]
        [TestCase(
            @"Это _не завершение _ выделения_",
            ExpectedResult = @"Это <em>не завершение _ выделения</em>",
            TestName = "Text in <em> tag on underscores between spaces"
            )]
        [TestCase(
            "Это __не выделение_",
            ExpectedResult = "Это __не выделение_",
            TestName = "Raw text on unpaired symbols ")]
        public string Render_ShouldReturnCorrectlyRenderedText(string text)
        {
            return md.Render(text);
        }
    }
}
