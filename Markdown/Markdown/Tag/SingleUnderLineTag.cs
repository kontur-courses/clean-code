using System.Collections.Generic;
using Markdown.Extensions;

namespace Markdown.Tag
{
    public class SingleUnderLineTag : ITag
    {
        public string Symbol => "_";
        public string Html => "em";
        public int Length => Symbol.Length;
        public MdType Type => MdType.SingleUnderLine;
        public List<MdType> AllowedInnerTypes =>
            new List<MdType> {MdType.SingleUnderLine, MdType.Sharp};
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }
        public string Content { get; set; }
        public int FindCloseIndex(string text) => this.FindClosePairedTagIndex(text);
        public string GetContent(string text) => this.GetPairedTagContent(text);
        public IAttribute Attribute { get; set; }
    }
}