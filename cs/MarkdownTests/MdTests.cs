using Markdown;
using Markdown.BuilderNamespace;
using Markdown.TokenizerNamespace;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        [TestCase("A", ExpectedResult = "A", TestName = "{m}_WithoutTags")]
        [TestCase("_A", ExpectedResult = "_A", TestName = "{m}_WithNotClosingTag")]
        [TestCase("_A_", ExpectedResult = "<em>A</em>", TestName = "{m}_WithItalic")]
        [TestCase("__A__", ExpectedResult = "<strong>A</strong>", TestName = "{m}_WithBold")]
        [TestCase("# A", ExpectedResult = "<h1>A</h1>", TestName = "{m}_WithHeading")]
        [TestCase("# A\nB", ExpectedResult = "<h1>A</h1>\nB", 
            TestName = "{m}_WithHeadingHighlightOnlyParagraph")]
        [TestCase("__A_B_C__", ExpectedResult = "<strong>A<em>B</em>C</strong>", 
            TestName = "{m}_WithItalicInsideBold")]
        [TestCase("_A__B__C_", ExpectedResult = "<em>A__B__C</em>", TestName = "{m}_BoldInsideItalic")]
        [TestCase("__A_", ExpectedResult = "__A_", TestName = "{m}_WithUnpairedTags")]
        [TestCase("__A_B__C_", ExpectedResult = "__A_B__C_", TestName = "{m}_WithIntersectionTags")]
        [TestCase("# __A_B_C__", ExpectedResult = "<h1><strong>A<em>B</em>C</strong></h1>", 
            TestName = "{m}_WithMultiTags")]
        [TestCase("1_23_4", ExpectedResult = "1_23_4", TestName = "{m}_WithNumbersInsideTag")]
        [TestCase("_A_BC", ExpectedResult = "<em>A</em>BC", TestName = "{m}_WithStartingOfWord")]
        [TestCase("A_B_C", ExpectedResult = "A<em>B</em>C", TestName = "{m}_WithMiddleOfWord")]
        [TestCase("AB_C_", ExpectedResult = "AB<em>C</em>", TestName = "{m}_WithEndingOfWord")]
        [TestCase("A_BC DE_F", ExpectedResult = "A_BC DE_F", TestName = "{m}_WithTagInDifferentWords")]
        [TestCase("_ ABC_", ExpectedResult = "_ ABC_", TestName = "{m}_WithStartsWithNonWhitespace")]
        [TestCase("_ABC _", ExpectedResult = "_ABC _", TestName = "{m}_WithEndsWithNonWhitespace")]
        [TestCase("____", ExpectedResult = "____", TestName = "{m}_WithEmptyContent")]
        public string Render_TagHighlight_ShouldProceedRules(string text)
        {
            return Md.Render(text);
        }

        [TestCase("A", ExpectedResult = "A", TestName = "{m}_WithoutTags")]
        [TestCase("_A", ExpectedResult = "_A", TestName = "{m}_WithNotClosingTag")]
        [TestCase("_A_", ExpectedResult = "_A_", TestName = "{m}_WithItalic")]
        [TestCase("__A__", ExpectedResult = "__A__", TestName = "{m}_WithBold")]
        [TestCase("# A", ExpectedResult = "# A", TestName = "{m}_WithHeading")]
        [TestCase("# A\nB", ExpectedResult = "# A\nB", 
            TestName = "{m}_WithHeadingHighlightOnlyParagraph")]
        [TestCase("__A_B_C__", ExpectedResult = "__A_B_C__", TestName = "{m}_WithItalicInsideBold")]
        [TestCase("_A__B__C_", ExpectedResult = "_A__B__C_", TestName = "{m}_BoldInsideItalic")]
        [TestCase("__A_", ExpectedResult = "__A_", TestName = "{m}_WithUnpairedTags")]
        [TestCase("__A_B__C_", ExpectedResult = "__A_B__C_", TestName = "{m}_WithIntersectionTags")]
        [TestCase("# __A_B_C__", ExpectedResult = "# __A_B_C__", TestName = "{m}_WithMultiTags")]
        [TestCase("1_23_4", ExpectedResult = "1_23_4", TestName = "{m}_WithNumbersInsideTag")]
        [TestCase("_A_BC", ExpectedResult = "_A_BC", TestName = "{m}_WithStartingOfWord")]
        [TestCase("A_B_C", ExpectedResult = "A_B_C", TestName = "{m}_WithMiddleOfWord")]
        [TestCase("AB_C_", ExpectedResult = "AB_C_", TestName = "{m}_WithEndingOfWord")]
        [TestCase("A_BC DE_F", ExpectedResult = "A_BC DE_F", TestName = "{m}_WithTagInDifferentWords")]
        [TestCase("_ ABC_", ExpectedResult = "_ ABC_", TestName = "{m}_WithStartsWithNonWhitespace")]
        [TestCase("_ABC _", ExpectedResult = "_ABC _", TestName = "{m}_WithEndsWithNonWhitespace")]
        [TestCase("____", ExpectedResult = "____", TestName = "{m}_WithEmptyContent")]
        public string Render_TagHighlight_ShouldNotProceedRules(string text)
        {
            var tokenizer = new Tokenizer(text);
            var builder = new Builder();
            return builder.Build(tokenizer.Tokenize());
        }

        [TestCase(@"\\", ExpectedResult = @"\", TestName = "WithEscapingToSelf")]
        [TestCase(@"\_A\_", ExpectedResult = @"_A_", TestName = "WithEscapingTag")]
        [TestCase(@"\# A", ExpectedResult = @"# A", TestName = "WithEscapingHeading")]
        [TestCase(@"\A", ExpectedResult = @"\A", TestName = "WithEscapingText")]
        [TestCase(@"\\_A_", ExpectedResult = @"\<em>A</em>", 
            TestName = "WithEscapingToSelfBeforeTag")]
        public string Render_EscapeCharacter_ShouldProceedRules(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"\\", ExpectedResult = @"\\", TestName = "WithEscapingToSelf")]
        [TestCase(@"\_A\_", ExpectedResult = @"\_A\_", TestName = "WithEscapingTag")]
        [TestCase(@"\# A", ExpectedResult = @"\# A", TestName = "WithEscapingHeading")]
        [TestCase(@"\A", ExpectedResult = @"\A", TestName = "WithEscapingText")]
        [TestCase(@"\\_A_", ExpectedResult = @"\\_A_", TestName = "WithEscapingToSelfBeforeTag")]
        public string Render_EscapeCharacter_ShouldNotProceedRules(string text)
        {
            var tokenizer = new Tokenizer(text);
            var builder = new Builder();
            return builder.Build(tokenizer.Tokenize());
        }
    }
}