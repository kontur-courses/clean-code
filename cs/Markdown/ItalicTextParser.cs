using System.Collections.Generic;

namespace Markdown
{
    public class ItalicTextParser : HtmlParser
    {
        public ItalicTextParser(params HtmlParser[] parsers) : base(parsers)
        {
        }

        public override bool TryRender(string mdText, int start, out int end, out string htmlText)
        {
            throw new System.NotImplementedException();
        }
    }
}