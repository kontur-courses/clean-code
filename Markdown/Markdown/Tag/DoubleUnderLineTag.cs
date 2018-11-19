using System.Collections.Generic;

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
    }
}