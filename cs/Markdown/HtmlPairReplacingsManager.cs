using System.Collections.Generic;
using System.Web.UI;

namespace Markdown
{
    public class HtmlPairReplacingsManager
    {
        //TODO Consider: make you unique for every tag!            
        
        private readonly List<HtmlTagPairReplacing> wrappings;
        private HtmlTagPairReplacing? currentPairReplacing;
        private HtmlTextWriterTag Tag { get; }

        public HtmlPairReplacingsManager( HtmlTextWriterTag tag)
        {
            Tag = tag;
            wrappings = new List<HtmlTagPairReplacing>();
            currentPairReplacing = null;
        }
        public bool IsOpened => currentPairReplacing != null;
        public IEnumerable<HtmlTagPairReplacing> GetWrappings => wrappings;

        public void AddUniversalTag(int index, int replacingLength)
        {
            if (IsOpened)
            {
                wrappings.Add(currentPairReplacing.Value.Close(index, replacingLength));
                currentPairReplacing = null;
            }
            else 
                currentPairReplacing = HtmlTagPairReplacing.Open(index, replacingLength, Tag);
                
        }
    }
}