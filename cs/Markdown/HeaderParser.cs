using System.Collections.Generic;

namespace Markdown
{
    public class HeaderParser : HtmlParser
    {
        public HeaderParser(params HtmlParser[] parsers) : base(parsers)
        {
        }

        public override bool TryRender(string mdText, int start, out int end, out string htmlText)
        {
            if (mdText[start] != Md.HeaderSymbol)
            {
                end = 0;
                htmlText = null;
                return false;
            }
            if (!base.TryRender(mdText, start + 1, out end, out htmlText))
                return false;
            htmlText = "<h1>" + htmlText + "</h1>";
            return true;
        }
    }
}