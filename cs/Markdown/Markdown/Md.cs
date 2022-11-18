using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var stringWithHtmlTags = MarkdownParser.Parse(text);
            return stringWithHtmlTags;
        }
    }
}