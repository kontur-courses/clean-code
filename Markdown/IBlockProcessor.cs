using System.Collections.Generic;

namespace Markdown
{
    internal interface IBlockProcessor
    {
        List<Block> Process(List<Block> blocks);
    }
}
