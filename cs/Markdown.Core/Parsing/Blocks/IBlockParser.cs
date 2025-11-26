using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing.Blocks;

public interface IBlockParser
{
    bool CanParse(ParserState state);
    BlockNode? Parse(ParserState state, InlineParser inlineParser);
}
