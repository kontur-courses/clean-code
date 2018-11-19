using System.Collections.Generic;

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
    }
}
