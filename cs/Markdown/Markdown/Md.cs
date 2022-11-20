using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var parser = new MarkdownParser();
            var replacer = new Replacer<MdTag>();
            var indexesTags = parser.GetIndexesTags(text);
            var stringWithHtmlTags = replacer.ReplaceTagOnHtml(indexesTags, text);
            return String.Empty;
        }
    }
}