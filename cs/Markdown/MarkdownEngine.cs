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
                htmlBuilder.Append(Tokenizer.ParseIntoTokens(paragraph).ConvertToHtmlString())
                    .Append(Environment.NewLine);
            return htmlBuilder.ToString();
        }
    }
}