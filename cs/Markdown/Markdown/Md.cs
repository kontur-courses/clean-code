using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var tagToTag = new Dictionary<MdTag, HtmlTag>()
            {
                {new MdTag("_", true), new HtmlTag("em", true)},
                {new MdTag("__", true), new HtmlTag("strong", true)},
                {new MdTag("#", false), new HtmlTag("h1", true)}
            };
            var parser = new MarkdownParser(tagToTag.Keys);
            var replacer = new TagsReplacer<MdTag, HtmlTag>(tagToTag);

            var stringWithHtmlTags = replacer.ReplaceTag(parser.GetIndexesTags(text), text);
            return stringWithHtmlTags;
        }
    }
}