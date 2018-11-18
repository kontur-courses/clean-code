using System;
using System.Collections.Generic;
using Markdown.Tag;

namespace Markdown
{
    public class Md
    {
        private readonly MdTagConverter mdTagConverter;
        private readonly HtmlTagWrapper htmlTagWrapper;

        public Md(Dictionary<string, ITag> dictionaryTags)
        {
            mdTagConverter = new MdTagConverter(dictionaryTags);
            htmlTagWrapper = new HtmlTagWrapper(dictionaryTags);
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var tags = mdTagConverter.Parse(text);
            var htmlText = htmlTagWrapper.ConvertToHtml(tags);
            return htmlText;
        }

        public string Render(ITag tag)
        {
            var tags = mdTagConverter.Parse(tag.Content);

            var checkedTags = new List<ITag>();
            foreach (var t in tags)
                checkedTags.Add(t.Length > tag.Length ? t.ToTextTag() : t);

            return checkedTags.Count == 0 ? tag.Content : htmlTagWrapper.ConvertToHtml(checkedTags);
        }
    }
}