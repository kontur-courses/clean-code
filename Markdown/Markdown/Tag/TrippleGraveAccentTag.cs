using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Tag
{
    class TrippleGraveAccentTag : ITag
    {
        public string Symbol => "```";
        public string Html => "code";
        public int Length => Symbol.Length;
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Content { get; set; }
        public MdType Type => MdType.TripleGraveAccent;
        public List<MdType> AllowedInnerTypes => new List<MdType>
        {
            MdType.TripleGraveAccent,
            MdType.DoubleUnderLine,
            MdType.Sharp,
            MdType.SingleUnderLine
        };
        public int FindCloseIndex(string text) => this.FindClosePairedTagIndex(text);
        public string GetContent(string text) => this.GetPairedTagContent(text);
        public IAttribute Attribute { get; set; }
    }
}