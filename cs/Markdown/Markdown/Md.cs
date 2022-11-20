using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var parser = new MarkdownParser();
            var tagToTag = new Dictionary<MdTag, HtmlTag>()
            {
                {new MdTag("_", true), new HtmlTag("em", true)},
                {new MdTag("__", true), new HtmlTag("strong", true)}
            };
            var replacer = new TagsReplacer<MdTag, HtmlTag>(tagToTag);
            
            var indexesTags = parser.GetIndexesTags(text);
            var stringWithHtmlTags = replacer.ReplaceTagOnHtml(indexesTags, text);
            return String.Empty;
        }
    }
}