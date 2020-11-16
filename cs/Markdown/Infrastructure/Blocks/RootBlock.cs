using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Blocks
{
    public class RootBlock : IBlock
    {
        private readonly IEnumerable<IBlock> blocks;

        public RootBlock(IEnumerable<IBlock> blocks)
        {
            this.blocks = blocks;
        }

        public IEnumerable<string> Format(IBlockFormatter blockFormatter)
        {
            return blocks.SelectMany(token => token.Format(blockFormatter));
        }
    }
}