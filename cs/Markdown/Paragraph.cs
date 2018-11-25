using System.Collections.Generic;

namespace Markdown
{
    public class Paragraph
    {
        public Paragraph(int start, int end, string mdText)
        {
            InlineTokens = new List<SingleToken>();
            StartingTokens = new List<SingleToken>();
            Start = start;
            End = end;
            MdText = mdText;
            HtmlTags = new List<HtmlTag>();
        }

        public Paragraph(int start, int end, string mdText, List<SingleToken> inlineTokens, List<SingleToken> startingTokens)
        {
            InlineTokens = inlineTokens;
            StartingTokens = startingTokens;
            Start = start;
            End = end;
            MdText = mdText;
            HtmlTags = new List<HtmlTag>();
        }

        public string MdText { get; }
        public List<SingleToken> InlineTokens { get; }
        public List<SingleToken> StartingTokens { get; }
        public List<HtmlTag> HtmlTags { get; }

        public int Start { get; }
        public int End { get; }
    }
}
