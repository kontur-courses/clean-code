using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Blocks
{
    public class StyledBlock : IBlock
    {
        private readonly Style style;
        private readonly IEnumerable<IBlock> blocks;

        public StyledBlock(Style style, IEnumerable<IBlock> blocks)
        {
            this.style = style;
            this.blocks = blocks;
        }

        public IEnumerable<string> Format(BlockFormatter blockFormatter)
        {
            return blockFormatter.Format(style, blocks.SelectMany(token => token.Format(blockFormatter)));
        }
    }
}