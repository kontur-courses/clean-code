using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;

namespace Markdown
{
    public class Md
    {
        private readonly HtmlTagWrapper htmlTagWrapper;
        private readonly MdTagConverter mdTagConverter;
        private readonly List<MdType> types;

        public Md(List<MdType> types)
        {
            this.types = types;
            if (types.Count != 0)
            {
                var tags = types.Select(TagFactory.Create).ToArray();
                var symbolMdTypeDictionary = tags.ToDictionary(t => t.Symbol, t => t.Type);
                var symbols = symbolMdTypeDictionary.Keys.ToList();
                mdTagConverter = new MdTagConverter(symbolMdTypeDictionary);
                htmlTagWrapper = new HtmlTagWrapper(symbols);
            }
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (types.Count == 0)
                return text;

            var tags = mdTagConverter.Parse(text);
            var htmlText = htmlTagWrapper.ConvertToHtml(tags);
            return htmlText;
        }
    }
}