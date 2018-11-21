using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [TestCase("plain text", ExpectedResult = "plain text", TestName = "Plain text as is")]
        [TestCase("", ExpectedResult = "", TestName = "Empty string as is")]


        [TestCase("_undersored_ text", ExpectedResult = "<em>undersored</em> text", TestName = "Underscored text in italic")]
        [TestCase("_undersored_ and again _undersored_ text", ExpectedResult = "<em>undersored</em> and again <em>undersored</em> text", 
            TestName = "Several occurences of underscored text in italic")]
        [TestCase(@"ecranned \_undersored\_ text", ExpectedResult = @"ecranned _undersored_ text", 
            TestName = "Ecranned underscored text in plain text")]

        [TestCase("double __undersored__ text", ExpectedResult = "double <strong>undersored</strong> text", 
            TestName = "Double underscored text in bold")]
        [TestCase(@"double \_\_undersored\_\_ text", ExpectedResult = @"double __undersored__ text",
            TestName = "Ecranned double underscored text in plain text")]
        [TestCase("double __undersored__ and again double __undersored__ text",
            ExpectedResult = "double <strong>undersored</strong> and again double <strong>undersored</strong> text",
            TestName = "Several occurences of double underscored text in bold")]


        [TestCase("_single __double__ single_", ExpectedResult = "<em>single __double__ single</em>", TestName = "Double inside single")]
        [TestCase("_single __double__ single_ _again_", ExpectedResult = "<em>single __double__ single</em> <em>again</em>",
            TestName = "Double inside single with several single")]
        [TestCase("_single __double__ single_ __again__", ExpectedResult = "<em>single __double__ single</em> <strong>again</strong>",
            TestName = "Double inside single with several double")]
        [TestCase("_single __double__ single_ _again single_ __again double__", 
            ExpectedResult = "<em>single __double__ single</em> <em>again single</em> <strong>again double</strong>",
            TestName = "Double inside single with extra tags")]

        [TestCase("__double _single_ double__", ExpectedResult = "<strong>double <em>single</em> double</strong>", 
            TestName = "Single inside double")]
        [TestCase("__double _single_ double__ _again_", ExpectedResult = "<strong>double <em>single</em> double</strong> <em>again</em>", 
            TestName = "Single inside double with several single")]
        [TestCase("__double _single_ double__ __again__", ExpectedResult = "<strong>double <em>single</em> double</strong> <strong>again</strong>", 
            TestName = "Single inside double with several double")]
        [TestCase("_single underscore", ExpectedResult = "_single underscore", TestName = "Not matching single underscore")]
        [TestCase("__double underscore", ExpectedResult = "__double underscore", TestName = "Not matching double underscore")]


        [TestCase("_ single underscored whitespaced text_", ExpectedResult = "_ single underscored whitespaced text_", 
            TestName = "Whitespace after starting single underscore indicator as plain text")]
        [TestCase("_single underscored whitespaced text _", ExpectedResult = "_single underscored whitespaced text _", 
            TestName = "Whitespace after finishing single underscore indicator as plain text")]

        [TestCase("__double underscored whitespaced text __", ExpectedResult = "__double underscored whitespaced text __", 
            TestName = "Whitespace after finishing double underscore indicator as plain text")]
        [TestCase("__ double underscored whitespaced text__", ExpectedResult = "__ double underscored whitespaced text__", 
            TestName = "Whitespace after starting double underscore indicator as plain text")]


        [TestCase("_12_3", ExpectedResult = "_12_3", TestName = "Single underscore inside digits")]
        [TestCase("__12__3", ExpectedResult = "__12__3", TestName = "Double underscore inside digits")]
        [TestCase("1__23__4", ExpectedResult = "1__23__4", TestName = "Double underscore all text inside digits")]
        [TestCase("1_23_4", ExpectedResult = "1_23_4", TestName = "Single underscore all text inside digits")]
        public string Render(string markdown)
        {
            var md = new Md(new Element.HtmlElement("__", "<strong>"), new Element.HtmlElement("_", "<em>"));
            return md.Render(markdown);
        }
    }
}
