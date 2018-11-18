using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Markdown
{
    public class Md
    {
        private readonly IMdTagLocator[] orderedTagsLocator;
        private readonly Stack<HtmlTextWriterTag> openTags = new Stack<HtmlTextWriterTag>();
        
        public Md(IMdTagLocator[] orderedTagsLocator)
        {
            this.orderedTagsLocator = orderedTagsLocator;
        }

        public Md() =>
            new Md(new []
            {
                new MdWrappingTagLocator("__", HtmlTextWriterTag.Strong,new Lazy<Md>(()=>this)),
                new MdWrappingTagLocator("_", HtmlTextWriterTag.U,new Lazy<Md>(()=>this)),
            });

        public IEnumerable<HtmlTextWriterTag> OpenTags => openTags;
        public string RenderingString { get; private set; }
        
        public string Render(string markdowned)
        {
            RenderingString = markdowned;
            LocateTagPairs();
            
            var writer = new StringWriter();
            using (var htmlWriter = new HtmlTextWriter(writer))
                ReplaceMdToHtml(htmlWriter);
            
            return writer.ToString();
        }

        private void LocateTagPairs()
        {   
            for (int i = 0; i < RenderingString.Length; i++)
                foreach (var tag in orderedTagsLocator)
                    if (tag.MatchMdTag(i))
                    {
                    
                        i += tag.AmountSkippedCharsWhileMatching;
                        break;
                    }
        }

        private void ReplaceMdToHtml(HtmlTextWriter writer)
        {
            var changes = orderedTagsLocator
                .SelectMany(x => x.HtmlPairs.GetWrappings)
                .SelectMany(x=>x.GetChanges(writer))
                .OrderBy(x=>x.index)
                .ToArray();

            var lastIndex = 0;
            foreach(var tup in changes)
            {
                var text = RenderingString.Substring(lastIndex, tup.index - lastIndex);
                writer.Write(text);
                lastIndex = tup.index + tup.replacingLength;
                tup.tagInsertion();
            }
            writer.Write(RenderingString.Substring(lastIndex));
        }
    }
}