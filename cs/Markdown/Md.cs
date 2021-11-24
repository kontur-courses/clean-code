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
        
        public static string Render(string mdText)
        {
            var mdParser = new MdParser();
            var htmlRenderer = new HtmlRender();
            var parseResult = mdParser.Parse(mdText);
            if (parseResult.Failure)
                throw new ArgumentException("Incorrect");
            return htmlRenderer.Render(parseResult.Value);
        }
    }
}