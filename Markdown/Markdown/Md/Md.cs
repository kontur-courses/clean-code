using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly HtmlTagWrapper htmlTagWrapper;
        private readonly MdTagConverter mdTagConverter;

        public Md(List<MdType> types)
        {
            var tags = types.Select(TagFactory.Create).ToArray();
            var symbolMdTypeDictionary = tags.ToDictionary(t => t.Symbol, t => t.Type);
            var symbols = symbolMdTypeDictionary.Keys.ToList();
            mdTagConverter = new MdTagConverter(symbolMdTypeDictionary);
            htmlTagWrapper = new HtmlTagWrapper(symbols);
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