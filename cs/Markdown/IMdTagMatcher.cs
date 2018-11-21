using System.Collections.Generic;

namespace Markdown
{
    public interface IMdTagMatcher
    {
        string TargetString { set; }
        
        int AmountSkippedCharsWhileMatching { get; }
        bool MatchOpenMdTag(int machStartIndex);
        bool MatchCloseMdTag(int machStartIndex);
    }
}