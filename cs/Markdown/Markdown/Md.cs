using System;

namespace Markdown
{
    public class Md : IMarkupProcessor
    {
        public string GetHtmlMarkup(string text)
        {
            return text;
        }
    }
}