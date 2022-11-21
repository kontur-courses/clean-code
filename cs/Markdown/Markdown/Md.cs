using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var tagToTag = new Dictionary<MdTag, HtmlTag>()
            {
                {new MdTag("__", true), new HtmlTag("strong", true)},
                {new MdTag("_", true), new HtmlTag("em", true)},
                {new MdTag("#", false), new HtmlTag("h1", true)}
            };
            var parser = new MarkdownParser(tagToTag.Keys.ToList());
            var replacer = new TagsReplacer<MdTag, HtmlTag>(tagToTag);
            
            var indexesTags = parser.GetIndexesTags(text);
            var stringWithHtmlTags = replacer.ReplaceTag(indexesTags, text);
            return stringWithHtmlTags;
        }
    }
}