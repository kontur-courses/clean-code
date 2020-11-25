using System;
using System.Linq;
using Markdown.Core;

namespace Markdown
{
    public static class MarkdownEngine
    {
        public static string Render(string mdText) => string.Join(Environment.NewLine,
            mdText.Split(Environment.NewLine).Select(HtmlConverter.ConvertToHtmlString)
        );
    }
}