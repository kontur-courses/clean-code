using System.Collections.Generic;

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
    }
}