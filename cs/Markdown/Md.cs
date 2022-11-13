using System;
using System.Globalization;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var parser = new Parser(markdownText);
            return HtmlBuilder.ConvertMarkdownToHTML(parser.Parse());
        }
    }
}
