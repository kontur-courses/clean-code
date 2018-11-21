using System.Collections.Generic;
using Markdown.Attribute;
using Markdown.Extensions;

namespace Markdown.Tag
{
    public class DoubleUnderLineTag : ITag
    {
        public string Symbol => "__";
        public string Html => "strong";
        public int Length => Symbol.Length;
        public MdType Type => MdType.DoubleUnderLine;

        public List<MdType> AllowedInnerTypes =>
            new List<MdType> {MdType.DoubleUnderLine, MdType.SingleUnderLine, MdType.Sharp};

        public string Content { get; set; }
        public int OpenIndex { get; set; }
        public int CloseIndex { get; set; }

        public int FindCloseIndex(string text)
        {
            return this.FindClosePairedTagIndex(text);
        }

        public string GetContent(string text)
        {
            return this.GetPairedTagContent(text);
        }

        public IAttribute Attribute { get; set; }
    }
}