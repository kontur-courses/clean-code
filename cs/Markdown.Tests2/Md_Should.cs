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
        [TestCase("_single __double__ single_ _again single_", ExpectedResult = "<em>single __double__ single</em> <em>again single</em>",
            TestName = "Double inside single with extra tags")]
        [TestCase("__double _single_ double__", ExpectedResult = "<strong>double <em>single</em> double</strong>", TestName = "Single inside double")]
        [TestCase("_single underscore", ExpectedResult = "_single underscore", TestName = "Not matching single underscore")]
        [TestCase("__double underscore", ExpectedResult = "__double underscore", TestName = "Not matching double underscore")]

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
