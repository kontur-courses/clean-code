using System.Web.UI;

namespace Markdown
{
    public class LineTokenMatcher : ITokenMatcher
    {
        private readonly string wrappingSequence;
 
        public LineTokenMatcher(string wrappingSequence, HtmlTextWriterTag tag)
        {
            this.wrappingSequence = wrappingSequence;
            this.Tag = tag;
        }
        
        public string TargetString { get; set; }
        public HtmlTextWriterTag Tag { get; }
        
        public bool TryOpen(int matchStartIndex, out Range openTagRange)
        {
            openTagRange = default(Range);
            if (!MatchOpenToken(matchStartIndex)) 
                return false;
            openTagRange = new Range(matchStartIndex,wrappingSequence.Length);
            return true;
        }

        public bool TryClose(int matchStartIndex, out Range closeTagRange)
        {
            closeTagRange = default(Range);
            if (TargetString[matchStartIndex] != '\n') 
                return false;
            closeTagRange = new Range(matchStartIndex,1);
            return true;
        }
        
        private bool MatchOpenToken(int machStartIndex) =>
            TargetString.Length >= wrappingSequence.Length + machStartIndex + 1 &&
            TargetString.Substring(machStartIndex, wrappingSequence.Length) == wrappingSequence;
    }
}