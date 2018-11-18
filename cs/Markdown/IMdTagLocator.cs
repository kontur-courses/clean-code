using System.Collections.Generic;

namespace Markdown
{
    public interface IMdTagLocator
    {
        HtmlPairReplacingsManager HtmlPairs { get; }
        int AmountSkippedCharsWhileMatching { get; }
        bool MatchMdTag(int machStartIndex);
    }
}