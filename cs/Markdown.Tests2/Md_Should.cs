using System.Text;
using Markdown.Element;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md(new HtmlElement("__", "strong"), new HtmlElement("_", "em"), new HeaderHtmlElement("#", "h"));
        }

        [TestCase("plain text", ExpectedResult = "plain text", TestName = "Plain text as is")]
        [TestCase("", ExpectedResult = "", TestName = "Empty string as is")]


        [TestCase("_undersored_ text", ExpectedResult = "<em>undersored</em> text", TestName = "Underscored text in italic")]
        [TestCase("_undersored_ and again _undersored_ text", ExpectedResult = "<em>undersored</em> and again <em>undersored</em> text",
            TestName = "Several occurences of underscored text in italic")]
        [TestCase(@"escaped \_undersored\_ text", ExpectedResult = @"escaped _undersored_ text",
            TestName = "Escaped underscored text in plain text")]

        [TestCase("double __undersored__ text", ExpectedResult = "double <strong>undersored</strong> text",
            TestName = "Double underscored text in bold")]
        [TestCase(@"double \_\_undersored\_\_ text", ExpectedResult = @"double __undersored__ text",
            TestName = "Escaped double underscored text in plain text")]
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


        [TestCase("# sharped text", ExpectedResult = "<h1>sharped text</h1>", TestName = "1 level header")]
        [TestCase("## sharped text", ExpectedResult = "<h2>sharped text</h2>", TestName = "2 level header")]
        [TestCase("### sharped text", ExpectedResult = "<h3>sharped text</h3>", TestName = "3 level header")]
        [TestCase("#### sharped text", ExpectedResult = "<h4>sharped text</h4>", TestName = "4 level header")]
        [TestCase("##### sharped text", ExpectedResult = "<h5>sharped text</h5>", TestName = "5 level header")]
        [TestCase("###### sharped text", ExpectedResult = "<h6>sharped text</h6>", TestName = "6 level header")]

        [TestCase("#sharped text", ExpectedResult = "#sharped text", TestName = "1 sharped text with no space as plain text")]
        [TestCase("##sharped text", ExpectedResult = "##sharped text", TestName = "2 sharped text with no space as plain text")]
        [TestCase("sharped # text", ExpectedResult = "sharped # text", TestName = "Text with sharp in the middle as is")]
        [TestCase("####### sharped text", ExpectedResult = "####### sharped text", TestName = "Text with more than 6 sharps as is")]

        [TestCase(@"\# escaped sharped text", ExpectedResult = "# escaped sharped text",
            TestName = "Escaped 1 level header as plain text")]
        [TestCase(@"\### sharped text", ExpectedResult = "### sharped text", TestName = "Escaped several sharps as plain text")]
        public string Render(string markdown)
        {
            return md.Render(markdown);
        }

        // Проходит на Intel Core i7-4771
        [Test, Timeout(100)]
        public void Render_InTimeLimit()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < 10000; i++)
                stringBuilder.Append("_single underscored_ ");

            var markdown = stringBuilder.ToString();

            md.Render(markdown);
        }
    }
}
