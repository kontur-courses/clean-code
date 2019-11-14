using System.Collections.Generic;

namespace Markdown.Html
{
    public class HtmlConverter
    {
        private readonly Dictionary<string, HtmlTag> separatorHtmlTags;

        public HtmlConverter(Dictionary<string, HtmlTag> separatorHtmlTags)
        {
            this.separatorHtmlTags = separatorHtmlTags;
        }

        public string ConvertSeparatedStringToHtmlString(string text, string separator)
        {
            return separatorHtmlTags[separator].ApplyTo(text);
        }
    }
}