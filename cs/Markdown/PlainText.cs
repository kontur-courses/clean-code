using System.Collections.Generic;

namespace Markdown
{
    public class PlainTextParser : HtmlParser
    {
        public PlainTextParser() : base(null)
        {
        }

        public override bool TryRender(string mdText, int start, out int end, out string htmlText)
        {
            throw new System.NotImplementedException();
        }
    }
}