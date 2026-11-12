using Markdown.Inline;
using System.Collections.Generic;

namespace Markdown.Blocks
{
    public class HeadingBlock : IBlock
    {
        public List<IInlineTag> InlineTags { get; }

        public HeadingBlock(List<IInlineTag> inlineTags)
        {
            InlineTags = inlineTags;
        }
    }
}
