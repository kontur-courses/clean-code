using System;
using System.Collections.Generic;
using Markdown.Render;

namespace Markdown
{
    public static class Md
    {
        public const char ItalicQuotes = '_';
        public const string BoldQuotes = "__";
        public const char ShieldingSymbol = '\\';
        public const char HeaderSymbol = '#';
        
        public static string TranslateToHtml(string mdText)
        {
            var mdParser = MdParser.Default;
            var htmlRenderer = HtmlRender.Default;
            var shieldedText = new StringWithShielding(mdText, ShieldingSymbol, '!',
                new HashSet<char>() { ItalicQuotes, HeaderSymbol });
            var parseResult = mdParser.Parse(shieldedText);
            if (parseResult.Status != Status.Success)
                throw new ArgumentException("Incorrect");
            return htmlRenderer.Render(parseResult.Value);
        }
    }
}