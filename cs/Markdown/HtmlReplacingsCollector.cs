using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Markdown
{
    public class HtmlReplacingsCollector
    {
        private readonly Stack<(HtmlTextWriterTag tag, Range range)> openedTags;
        private readonly List<HtmlTagPairReplacing> replacings; 
        
        public HtmlReplacingsCollector()
        {
            openedTags = new Stack<(HtmlTextWriterTag, Range)>();
            replacings = new List<HtmlTagPairReplacing>();
        }
        
        public IEnumerable<HtmlTagPairReplacing> GetReplacings => replacings;
        public bool TryOpenTag(HtmlTextWriterTag tag, Range insertRange)
        {
            if (StrongInUCase(tag))
                return false;
            openedTags.Push((tag,insertRange));
            return true;
        }

        private bool StrongInUCase(HtmlTextWriterTag tag) => //TODO initialize this check in Md  
            tag == HtmlTextWriterTag.Strong && openedTags.Select(x => x.tag).Contains(HtmlTextWriterTag.U);
        
        public bool TryCloseTag(HtmlTextWriterTag tag, Range insertRange)
        {
            if (!openedTags.Any() || openedTags.Peek().tag != tag) 
                return false;
            replacings.Add(new HtmlTagPairReplacing(openedTags.Pop().range,insertRange,tag));
            return true;
        }
    }
}