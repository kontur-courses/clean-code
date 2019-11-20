using System.Collections.Generic;
using MarkdownProcessing.Tokens;

namespace MarkdownProcessing.Markdowns
{
    public interface IResultMarkdown
    {
        Dictionary<TokenType, string> OpeningTags { get;}
        Dictionary<TokenType, string> ClosingTags { get;}
    }
}