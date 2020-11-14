using Markdown.Models.Converters;
using Markdown.Models.ConvertingRules;
using Markdown.Models.Renders;
using Markdown.Models.Syntax;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        private Md render;

        [SetUp]
        public void SetUp()
        {
            var convertRules = ConvertingRulesFactory.GetAllRules();
            var toHtmlConverter = new MdToHtmlConverter(convertRules);
            render = new Md(new Syntax(), toHtmlConverter);
        }
    }
}
