using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Blocks
{
    public class StyledBlock : IBlock
    {
        private readonly IEnumerable<IBlock> blocks;
        private readonly Tag tag;

        public StyledBlock(Tag tag, IEnumerable<IBlock> blocks)
        {
            this.tag = tag;
            this.blocks = blocks;
        }

        public IEnumerable<string> Format(IBlockFormatter blockFormatter)
        {
            return blockFormatter.Format(tag, blocks.SelectMany(token => token.Format(blockFormatter)));
        }
    }
}