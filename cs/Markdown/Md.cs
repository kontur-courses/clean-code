using System;
using System.Text;

namespace Markdown
{
    public class Md
    {
        public static string Render(string rawMarkdown)
        {
            if (string.IsNullOrEmpty(rawMarkdown))
                return "";

            var result = new StringBuilder();
            foreach (var paragraph in rawMarkdown.Split(Environment.NewLine))
            {
                var parser = new MarkdownParser();
                var parsedText = parser.Parse(paragraph);
                result.Append(HtmlMaker.FromTextInfo(parsedText));
                result.Append(Environment.NewLine);
            }

            result.Remove(result.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            return result.ToString();
        }
    }
}