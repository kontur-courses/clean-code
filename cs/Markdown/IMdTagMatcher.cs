using System.Collections.Generic;

namespace Markdown
{
    public interface IMdTagMatcher
    {
        HtmlPairReplacingsManager HtmlPairs { get; }
        int AmountSkippedCharsWhileMatching { get; }
        bool MatchMdTag(int machStartIndex);
    }
}