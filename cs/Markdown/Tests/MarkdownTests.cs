using NUnit.Framework;
using FluentAssertions;
using Markdown.Core;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        private readonly MdRenderer markdown = new MdRenderer();
        
        [TestCase("text _em em_ __strong strong__", TestName = "Simple separate tags with spaces",
            ExpectedResult = "text <em>em em</em> <strong>strong strong</strong>")]
        [TestCase("_abc __asf fw__ yre_", TestName = "Strong nested into em",
            ExpectedResult = "<em>abc <strong>asf fw</strong> yre</em>")]
        [TestCase("__abc _asf fw_ yre__", TestName = "Em nested into strong",
            ExpectedResult = "<strong>abc <em>asf fw</em> yre</strong>")]
        [TestCase("_text", TestName = "Unpaired em in beginning",
            ExpectedResult = "_text")]
        [TestCase("__text", TestName = "Unpaired strong in beginning",
            ExpectedResult = "__text")]
        [TestCase("text_", TestName = "Unpaired em in ending",
            ExpectedResult = "text_")]
        [TestCase("text__", TestName = "Unpaired strong in ending",
            ExpectedResult = "text__")]
        [TestCase("\\_test\\_", TestName = "Escaped em",
            ExpectedResult = "_test_")]
        [TestCase("\\_\\_test\\_\\_", TestName = "Escaped strong",
            ExpectedResult = "__test__")]
        [TestCase("test_t_e_x__t", TestName = "Underscore inside word", 
            ExpectedResult = "test_t_e_x__t")]
        [TestCase("_mark\ndown_", TestName = "Line break inside em",
            ExpectedResult = "<em>mark\ndown</em>")]
        [TestCase("__mark\ndown__", TestName = "Line break inside strong",
            ExpectedResult = "<strong>mark\ndown</strong>")]
        public string RenderShouldReturnCorrectHtmlText(string mdText)
        {
            return markdown.Render(mdText);
        }
    }
}