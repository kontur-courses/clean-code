using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("A", ExpectedResult = "A", TestName = "WithoutTags")]
        [TestCase("_A_", ExpectedResult = "<em>A</em>", TestName = "Italic")]
        [TestCase("__A__", ExpectedResult = "<strong>A</strong>", TestName = "Bold")]
        [TestCase("# A", ExpectedResult = "<h1>A</h1>", TestName = "Heading")]
        public string Render_SingleTag_ReturnsStringCorrectly(string text)
        {
            return md.Render(text);
        }

        [TestCase("__A_B_C__", ExpectedResult = "<strong>A<em>B</em>C</strong>", TestName = "ItalicInsideBold")]
        [TestCase("_A__B__C_", ExpectedResult = "<em>A__B__C</em>", TestName = "BoldInsideItalic")]
        [TestCase("__A_", ExpectedResult = "__A_", TestName = "UnpairedTags")]
        [TestCase("__A_B__C_", ExpectedResult = "__A_B__C_", TestName = "IntersectionTags")]
        [TestCase("# __A_B_C__", ExpectedResult = "<h1><strong>A<em>B</em>C</strong></h1>", 
            TestName = "WithAllTags")]
        public string Render_TagCollaborations_ReturnsStringCorrectly(string text)
        {
            return md.Render(text);
        }

        [TestCase("1_23_4", ExpectedResult = "1_23_4", TestName = "SelectingNumbers")]
        [TestCase("_A_BC", ExpectedResult = "<em>A</em>BC", TestName = "SelectingBeginningOfWord")]
        [TestCase("A_B_C", ExpectedResult = "A<em>B</em>C", TestName = "SelectingMiddleOfWord")]
        [TestCase("AB_C_", ExpectedResult = "AB<em>C</em>", TestName = "SelectingEndingOfWord")]
        [TestCase("A_BC DE_F", ExpectedResult = "A_BC DE_F", TestName = "SelectingInDifferentWords")]
        [TestCase("_ ABC_", ExpectedResult = "_ ABC_", TestName = "SelectingStartsWithNonWhitespace")]
        [TestCase("_ABC _", ExpectedResult = "_ABC _", TestName = "SelectingEndsWithNonWhitespace")]
        [TestCase("____", ExpectedResult = "<strong></strong>", TestName = "SelectingEmptyContent")]
        public string Render_TagSelections_ReturnsStringCorrectly(string text)
        {
            return md.Render(text);
        }
    }
}