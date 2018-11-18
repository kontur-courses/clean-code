using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Markdown
{
    public struct HtmlTagPairReplacing
    {
        private readonly int openIndex;
        private readonly int openReplaceLength;
        private int closeIndex;
        private int closeReplaceLength;
        private readonly HtmlTextWriterTag tag;
        
        public HtmlTagPairReplacing(int openIndex, int replaceLength, HtmlTextWriterTag tag)
        {
            this.openIndex = openIndex;
            openReplaceLength = replaceLength;
            this.tag = tag;
            
            closeIndex = -1;
            closeReplaceLength = -1;
        }
        
        public static HtmlTagPairReplacing Open(int openIndex, int replaceLength, HtmlTextWriterTag tag)=>
                new HtmlTagPairReplacing(openIndex,replaceLength,tag);

        //TODO Refactor: make only one closing.
        public HtmlTagPairReplacing Close(int closeIndex, int replaceLength)
        {
            this.closeIndex = closeIndex;
            closeReplaceLength = replaceLength;
            return this;
        }

        public IEnumerable<(int index, int replacingLength, Action tagInsertion)> GetChanges(HtmlTextWriter writer)
        {
            var tag = this.tag;
            yield return (openIndex, openReplaceLength, () => writer.RenderBeginTag(tag));
            yield return (closeIndex, closeReplaceLength, writer.RenderEndTag);
        }
    }
}