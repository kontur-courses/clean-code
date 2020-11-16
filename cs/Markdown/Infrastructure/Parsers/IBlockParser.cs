using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public interface IBlockParser
    {
        public IBlock Parse();
    }
}