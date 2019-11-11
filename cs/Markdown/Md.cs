using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string markDownParagraph)
        {
            var parser = new MdParser(markDownParagraph);
            var tokens = parser.GetTokens();
            var htmlTokens = tokens.Select(token => token.ConvertToHtml());

            return string.Join(" ", htmlTokens);
        }
    }
}