using System.Text;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawMarkdown)
        {
            if (string.IsNullOrEmpty(rawMarkdown))
                return "";

            var paragraphs = rawMarkdown.Split('\n');
            var result = new StringBuilder();
            foreach (var paragraph in paragraphs)
            {
                var parser = new MarkdownParser();
                var parsedText = parser.Parse(paragraph);
                result.Append(HtmlMaker.FromTextInfo(parsedText));
            }
            
            return string.Join('\n', result);
        }
    }
}