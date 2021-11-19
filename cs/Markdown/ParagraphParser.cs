using System;
using System.Collections.Generic;

namespace Markdown
{
    public class ParagraphParser : HtmlParser
    {
        public ParagraphParser(params HtmlParser[] parsers) : base(parsers)
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
            htmlText = "<p>" + htmlText + "<p>";
            return true;
        }
    }
}