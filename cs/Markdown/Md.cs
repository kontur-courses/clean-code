using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace Markdown
{
    public class Md
    {   
        private readonly IMdTagMatcher[] orderedTagsMatchers;
        
        public Md(IMdTagMatcher[] orderedTagsMatchers)=>
            this.orderedTagsMatchers = orderedTagsMatchers;
        
        public Md() =>
            this.orderedTagsMatchers = new []
            {
                new MdWrappingTagMatcher("__", HtmlTextWriterTag.Strong),
                new MdWrappingTagMatcher("_", HtmlTextWriterTag.U),
            };

        private string renderingString;
        
        public string Render(string markdowned)
        {
            renderingString = markdowned;
            foreach (var matcher in orderedTagsMatchers)
                matcher.TargetString = renderingString;
            
            var collector = CollectTagPairs();
            
            var writer = new StringWriter();
            using (var htmlWriter = new HtmlTextWriter(writer))
                ReplaceMdToHtml(htmlWriter, collector.GetReplacings);
            
            return writer.ToString();
        }

        private HtmlReplacingsCollector CollectTagPairs()
        {   
            var collector = new HtmlReplacingsCollector();
            for (int i = 0; i < renderingString.Length; i++)
                foreach (var matcher in orderedTagsMatchers)
                    if (Shitpile(collector,i,matcher))
                    {
                        i += matcher.AmountSkippedCharsWhileMatching;
                        break;
                    }

            return collector;
        }

        #region Shitpile method! Do not read!

        private bool Shitpile(HtmlReplacingsCollector collector, int startIndex, IMdTagMatcher matcher)
        {
            var m1 = matcher.MatchCloseMdTag(startIndex);
            var m2 = matcher.MatchOpenMdTag(startIndex);
            var matchingRange = new Range(startIndex,((MdWrappingTagMatcher) matcher).wrappingSequence.Length);
            if (m1 && collector.TryCloseTag(((MdWrappingTagMatcher) matcher).tag, matchingRange))
                return true;
            if (m2 && collector.TryOpenTag(((MdWrappingTagMatcher) matcher).tag, matchingRange))
                return true;
            return m1 || m2;
        }

        #endregion
        

        private void ReplaceMdToHtml(HtmlTextWriter writer,IEnumerable<HtmlTagPairReplacing> replacings)
        {
            var changes = replacings
                .SelectMany(x=>x.GetChanges(writer))
                .OrderBy(x=>x.replacingRange.Index)
                .ToArray();

            var lastIndex = 0;
            foreach(var tup in changes)
            {
                var text = renderingString.Substring(lastIndex, tup.replacingRange.Index - lastIndex);
                writer.Write(text);
                lastIndex = tup.replacingRange.Index + tup.replacingRange.Length;
                tup.tagInsertion();
            }
            writer.Write(renderingString.Substring(lastIndex));
        }
    }
}