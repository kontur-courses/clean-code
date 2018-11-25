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
        
        public void CollectTagPairs(string renderingString, IEnumerable<ITokenMatcher> matchers)
        {
            for (int i = 0; i < renderingString.Length; i++)
                foreach (var matcher in matchers)
                    if((matcher.TryClose(i, out var range) &&
                        TryCloseTag(matcher.Tag,range))||
                       (matcher.TryOpen(i, out range) &&
                        TryOpenTag(matcher.Tag,range)))
                    {
                        i += range.Length - 1;
                        break;
                    }
        }
        
        public IEnumerable<HtmlTagPairReplacing> GetReplacings => replacings;
        
        private bool TryOpenTag(HtmlTextWriterTag tag, Range insertRange)
        {
            if (StrongInUCase(tag))
                return true;
            openedTags.Push((tag,insertRange));
            return true;
        }

        private bool StrongInUCase(HtmlTextWriterTag tag) => //TODO initialize this check in Md  
            tag == HtmlTextWriterTag.Strong && openedTags.Select(x => x.tag).Contains(HtmlTextWriterTag.U);
        
        private bool TryCloseTag(HtmlTextWriterTag tag, Range insertRange)
        {
            if (StrongInUCase(tag))
                return true;
            if (!openedTags.Any() || openedTags.Peek().tag != tag) 
                return false;
            replacings.Add(new HtmlTagPairReplacing(openedTags.Pop().range,insertRange,tag));
            return true;
        }
    }
}