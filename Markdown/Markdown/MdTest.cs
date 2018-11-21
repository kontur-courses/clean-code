using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class Md_Should
    {
        private Md _md;
        
        [SetUp]
        public void SetUp()
        {
            _md = new Md();
        }

        [TestCase("some text", ExpectedResult = "some text", TestName = "Without tags")]
        [TestCase("_some __strong__ text_", ExpectedResult = "<em>some __strong__ text</em>", TestName = "Strong tag inside em tag with spaces")]
        [TestCase("_some__strong__text_", ExpectedResult = "<em>some__strong__text</em>", TestName = "Strong tag inside em tag without spaces")]
        [TestCase("__some _strong_ text__", ExpectedResult = "<strong>some <em>strong</em> text</strong>", TestName = "Em tag inside strong tag with spaces")]
        [TestCase("__some_strong_text__", ExpectedResult = "<strong>some_strong_text</strong>", TestName = "Em tag inside strong tag without spaces")]
        [TestCase("__some _bugged text__ bug_", ExpectedResult = "<strong>some _bugged text</strong> bug_", TestName = "Invalid tags pairs")]
        [TestCase("__text_", ExpectedResult = "__text_", TestName = "Invalid tag")]
        public string MdRender_ShouldReturnCorrectHtml_When(string md)
        {
            return _md.Render(md);
        }
        
        [TestCase("_text_", ExpectedResult = "<em>text</em>", TestName = "Valid em tag with word")]
        [TestCase("_text with spaces_", ExpectedResult = "<em>text with spaces</em>", TestName = "Valid em tag contains multiple words")]
        [TestCase("_ text_", ExpectedResult = "_ text_", TestName = "Invalid em tag begins by space")]
        [TestCase("_text _", ExpectedResult = "_text _", TestName = "Invalid em tag ends by space")]
        [TestCase("_text_ ", ExpectedResult = "<em>text</em> ", TestName = "Valid em tag with right outer spaces")]
        [TestCase(" _text_", ExpectedResult = " <em>text</em>", TestName = "Valid em tag with left outer spaces")]
        [TestCase("_a 1_", ExpectedResult = "<em>a 1</em>", TestName = "Valid em tag ends by number divided by space")]
        [TestCase("_1 a_", ExpectedResult = "<em>1 a</em>", TestName = "Valid em tag starts by number divided by space")]
        [TestCase("_1_2_3 4_", ExpectedResult = "<em>1_2_3 4</em>", TestName = "Valid em tag with numbers divided by space")]
        public string MdRender_ShouldReturnCorrectEmHtmlTag_When(string md)
        {
            return _md.Render(md);
        }
        
        [TestCase("__text__", ExpectedResult = "<strong>text</strong>", TestName = "Valid strong tag")]
        [TestCase("__text with spaces__", ExpectedResult = "<strong>text with spaces</strong>", TestName = "Valid strong tag contains multiple words")]
        [TestCase("__ text__", ExpectedResult = "__ text__", TestName = "Invalid strong tag begins by space")]
        [TestCase("__text __", ExpectedResult = "__text __", TestName = "Invalid strong tag ends by space")]
        [TestCase("__text__ ", ExpectedResult = "<strong>text</strong> ", TestName = "Valid strong tag with right outer spaces")]
        [TestCase(" __text__", ExpectedResult = " <strong>text</strong>", TestName = "Valid strong tag with left outer spaces")]
        [TestCase("__a 1__", ExpectedResult = "<strong>a 1</strong>", TestName = "Valid strong tag ends by number divided by space")]
        [TestCase("__1 a__", ExpectedResult = "<strong>1 a</strong>", TestName = "Valid strong tag starts by number divided by space")]
        [TestCase("__1_2_3 4__", ExpectedResult = "<strong>1_2_3 4</strong>", TestName = "Valid strong tag with numbers divided by space")]
        public string MdRender_ShouldReturnCorrectStrongHtmlTag_When(string md)
        {
            return _md.Render(md);
        }
        
        [TestCase("_12_", ExpectedResult = "_12_", TestName = "Number like em tag")]
        [TestCase("_12", ExpectedResult = "_12", TestName = "Number starts with underline")]
        [TestCase("12_", ExpectedResult = "12_", TestName = "Number ends with underline")]
        [TestCase("_1_23_4", ExpectedResult = "_1_23_4", TestName = "Numbers divided by single underline")]
        [TestCase("__1___23_4____1", ExpectedResult = "__1___23_4____1", TestName = "Numbers divided by multiple underlines")]
        public string MdRender_ShouldReturnCorrectNumber_When(string md)
        {
            return _md.Render(md);
        }
        
        [TestCase("text with \\escape", ExpectedResult = "text with escape", TestName = "Some text with escapes")]
        [TestCase("\\_text\\_", ExpectedResult = "_text_", TestName = "Escaped em tag with word")]
        [TestCase("\\__text\\__", ExpectedResult = "__text__", TestName = "Escaped strong tag with word")]
        public string MdRender_ShouldReturnCorrectEscapedCharacters_When(string md)
        {
            return _md.Render(md);
        }
    }
}