using System.Collections.Generic;

namespace Markdown.Md
{
    public class MdPairedTagsState
    {
        public readonly Stack<(int, MdToken)> OpeningTagsTokens;
        public readonly LinkedList<(int, MdToken)> InvalidTokens;

        public bool InEmphasis { get; set; }

        public MdPairedTagsState()
        {
            OpeningTagsTokens = new Stack<(int, MdToken)>();
            InvalidTokens = new LinkedList<(int, MdToken)>();
        }
    }
}