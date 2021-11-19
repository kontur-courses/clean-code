using System.Collections.Generic;

namespace Markdown
{
    public class BoldTextParser : HtmlParser
    {
        public BoldTextParser(params HtmlParser[] parsers) : base(parsers)
        {
        }

        public override bool TryRender(string mdText, int start, out int end, out string htmlText)
        {
            throw new System.NotImplementedException();
        }
    }
}