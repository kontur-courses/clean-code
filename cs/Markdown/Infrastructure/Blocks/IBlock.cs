using System.Collections.Generic;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Blocks
{
    public interface IBlock
    {
        public IEnumerable<string> Format(IBlockFormatter blockFormatter);
    }
}