using Markdown.Inline;
using System.Collections.Generic;

namespace Markdown.Blocks
{
    public class ParagraphBlock : IBlock
    {
        public List<IInlineTag> InlineTags { get; }

        public ParagraphBlock(List<IInlineTag> inlineTags)
        {
            InlineTags = inlineTags;
        }
    }
}