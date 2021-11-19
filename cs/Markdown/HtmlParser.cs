using System.Collections.Generic;

namespace Markdown
{
    public class HtmlParser
    {
        public HtmlParser[] Parsers;

        public HtmlParser(params HtmlParser[] parsers)
        {
            Parsers = parsers;
        }

        public virtual bool TryRender(string mdText, int start, out int end, out string htmlText)
        {
            htmlText = "";
            end = start;
            while (end < mdText.Length)
                foreach (var parser in Parsers)
                {
                    if (parser.TryRender(mdText, end, out var newEnd, out var result))
                    {
                        htmlText += result;
                        end = newEnd;
                    }
                }
            return true;
        }
    }
}