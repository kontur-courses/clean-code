using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Md_Tests
    {
        [TestCase("text", "text")]
        [TestCase("_em_", "<em>em</em>")]
        [TestCase("_emCut", "_emCut")]
        [TestCase("__strong__", "<strong>strong</strong>")]
        [TestCase("__strongCut", "__strongCut")]
        [TestCase("__strong _em_ strong__", "<strong>strong <em>em</em> strong</strong>")]
        [TestCase("__s _a_ text _a_ s__", "<strong>s <em>a</em> text <em>a</em> s</strong>")]
        [TestCase("__ __", "__ __")]
        [TestCase("__a __", "<strong>a </strong>")]
        [TestCase("_ _", "_ _")]
        [TestCase("____", "____")]
        [TestCase("___three underlines___", "___three underlines___")]
        [TestCase("____four underlines____", "____four underlines____")]
        public void Md_Render_ReturnRightResult(string text, string expectedResult)
        {
            var md = new Md();
            md.Render(text, new MdParser(), new MdConverter()).Should().BeEquivalentTo(expectedResult);
        }
    }
}