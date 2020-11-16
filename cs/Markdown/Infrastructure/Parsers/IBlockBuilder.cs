using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public interface IBlockBuilder
    {
        public IBlock Build(IEnumerable<TagInfo> tags);
    }
}