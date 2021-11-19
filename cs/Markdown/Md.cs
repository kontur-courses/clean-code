using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Md
    {
        public const string ItalicQuotes = "_";
        public const string BoldQuotes = "__";
        public const char ShieldingSymbol = '\\';
        public const char HeaderSymbol = '#';
        
        public static string Render(string mdText)
        {
            var plain = new PlainTextParser();
            var italic = new ItalicTextParser(plain);
            var bold =  new BoldTextParser(italic, plain);
            var parag = new ParagraphParser(bold, italic, plain);
            var header = new HeaderParser(bold, italic, plain);
            var html = new HtmlParser(header, parag);
            html.TryRender(mdText, 0, out var _, out var result);
            return result;
        }
    }
}