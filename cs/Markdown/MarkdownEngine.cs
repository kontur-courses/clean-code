using System;
using System.Text;
using Markdown.Core;

namespace Markdown
{
    public static class MarkdownEngine
    {
        public static string Render(string mdText)
        {
            var htmlBuilder = new StringBuilder();
            var mdStrings = mdText.Split(Environment.NewLine);
            
            foreach (var paragraph in mdStrings)
                htmlBuilder.AppendLine(HtmlConverter.ConvertToHtmlString(paragraph));
            
            return htmlBuilder.ToString();
        }
    }
}