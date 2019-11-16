using System.Collections.Generic;

namespace Markdown.Html
{
    public class HtmlConverter : IHtmlConverter
    {
        private readonly IReadOnlyDictionary<string, PairedHtmlTag> separatorHtmlTags;

        public HtmlConverter(IReadOnlyDictionary<string, PairedHtmlTag> separatorHtmlTags)
        {
            this.separatorHtmlTags = separatorHtmlTags;
        }

        public string ConvertSeparatedStringToPairedHtmlTag(string text, string separator)
        {
            return separatorHtmlTags[separator].ApplyTo(text);
        }
    }
}