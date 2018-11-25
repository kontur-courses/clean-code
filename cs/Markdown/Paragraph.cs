using System.Collections.Generic;

namespace Markdown
{
    public class Paragraph
    {
        public Paragraph(int start, int end, string mdText, List<SingleToken> inlineTokens, List<SingleToken> startingTokens, TokenTypeEnum startingTokenType)
        {
            InlineTokens = inlineTokens;
            StartingTokens = startingTokens;
            Start = start;
            End = end;
            MdText = mdText;
            StartingTokenType = startingTokenType;
            ValidTokens = new List<SingleToken>();
        }

        public string MdText { get; }
        public List<SingleToken> InlineTokens { get; }
        public List<SingleToken> StartingTokens { get; }
        public List<SingleToken> ValidTokens { get; }
        public TokenTypeEnum StartingTokenType { get; }

        public int Start { get; }
        public int End { get; }
    }
}
