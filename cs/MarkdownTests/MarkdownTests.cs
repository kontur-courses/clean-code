using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Markdown.Core;
using NUnit.Framework;

namespace MarkdownTests
{   
    [TestFixture]
    public class MarkdownTests
    {
        private readonly MdRenderer markdown = new MdRenderer();

        [TestCase("_abc __asf fw__ yre_", TestName = "Strong nested into em ignored",
            ExpectedResult = "<em>abc __asf fw__ yre</em>")]
        [TestCase("__abc _asf fw_ yre__", TestName = "Em nested into strong",
            ExpectedResult = "<strong>abc <em>asf fw</em> yre</strong>")]
        [TestCase("test_t_e_x__t", TestName = "Underscore inside word", 
            ExpectedResult = "test_t_e_x__t")]        
        [TestCaseSource(typeof(MarkdownTestsData), 
            nameof(MarkdownTestsData.SingleTestCases), Category = "Single tag cases")]
        [TestCaseSource(typeof(MarkdownTestsData), 
            nameof(MarkdownTestsData.UnpairedTestCases), Category = "Unpaired tag cases")]
        public string RenderShouldReturnCorrectHtmlText(string mdText)
        {
            return markdown.Render(mdText);
        }
    }
}