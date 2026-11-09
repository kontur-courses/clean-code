using Markdown.Node;

namespace Markdown.Parser.Interfaces;

public interface IBlockParser
{
    IReadOnlyList<BlockNode> Parse(string markdown);
}