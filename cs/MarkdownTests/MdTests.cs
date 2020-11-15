using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tests
    {
        private readonly Md md = new Md();

        [TestCase("_abc_", ExpectedResult = "<em>abc</em>", TestName = "When_Only_One_Tag")]
        public string Render_ConvertItalicTag(string input)
        {
            return md.Render(input);
        }
        
        [TestCase(@"\_abc_", ExpectedResult = "_abc_", TestName = "When_One_Of_Tags_Is_Screened")]
        [TestCase(@"\\_abc_", ExpectedResult = @"\<em>abc</em>", TestName = "When_Slash_Is_Screened")]
        [TestCase(@"\ _abc_", ExpectedResult = @"\ <em>abc</em>", TestName = "When_Slash_Do_Not_Screen")]
        public string Render_ConsiderScreening(string input)
        {
            return md.Render(input);
        }
    }
}