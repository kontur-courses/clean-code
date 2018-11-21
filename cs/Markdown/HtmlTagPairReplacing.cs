using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Markdown
{
    public struct HtmlTagPairReplacing
    {
        private readonly Range openTagRange;
        private readonly Range closeTagRange;
        private readonly HtmlTextWriterTag tag;

        public HtmlTagPairReplacing(Range openTagRange, Range closeTagRange, HtmlTextWriterTag tag)
        {
            this.openTagRange = openTagRange;
            this.closeTagRange = closeTagRange;
            this.tag = tag;
        }

        public IEnumerable<(Range replacingRange, Action tagInsertion)> GetChanges(HtmlTextWriter writer)
        {
            var tag = this.tag;
            yield return (openTagRange, () => writer.RenderBeginTag(tag));
            yield return (closeTagRange, writer.RenderEndTag);
        }
    }
}