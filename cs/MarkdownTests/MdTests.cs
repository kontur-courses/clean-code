using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tests
    {
        private readonly Md md = new Md();

        [TestCase("_abc_", ExpectedResult = "<em>abc</em>")]
        public string Render_ConvertItalicTag(string input)
        {
            return md.Render(input);
        }
    }
}