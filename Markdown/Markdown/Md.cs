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
            htmlTagWrapper = new HtmlTagWrapper();
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var tags = mdTagConverter.Parse(text);
            var htmlText = htmlTagWrapper.ConvertToHtml(tags);
            return htmlText;
        }
    }
}