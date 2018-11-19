using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        private readonly MdTagConverter mdTagConverter;
        private readonly HtmlTagWrapper htmlTagWrapper;

        public Md(List<MdType> types)
        {
            mdTagConverter = new MdTagConverter(types);
            htmlTagWrapper = new HtmlTagWrapper(types);
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