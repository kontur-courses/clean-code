using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Markdown
{
    public class ReplacingsApplyer: IDisposable
    {
        private readonly string targetString;
        private int lastIndex = 0;
        private readonly HtmlTextWriter htmlWriter;

        public ReplacingsApplyer(string target)
        {
            targetString = target;
            Result = new StringWriter();
            htmlWriter = new HtmlTextWriter(Result);
        }
        
        public StringWriter Result { get; }
        
        public void Apply(IEnumerable<HtmlTagPairReplacing> replacings)
        {
            var changes = replacings    
                .SelectMany(x=>x.GetChanges(htmlWriter))
                .OrderBy(x=>x.replacingRange.Index)
                .ToArray();

            foreach(var tup in changes)
            {
                WriteTextBefore(tup.replacingRange);
                tup.tagInsertion();
            }
            
            htmlWriter.Write(targetString.Substring(lastIndex));
        }

        private void WriteTextBefore(Range tagRange)
        {
            var text = targetString.Substring(lastIndex, tagRange.Index - lastIndex);
            htmlWriter.Write(text);
            lastIndex = tagRange.Index + tagRange.Length;
        }

        public void Dispose()
        {
            htmlWriter.Dispose();
            Result.Dispose();
        }
    }
}