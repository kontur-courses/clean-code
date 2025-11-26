using Markdown.Core.Lexing;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing.Blocks;
public class HeadingBlockParser : IBlockParser
{
    public bool CanParse(ParserState state) =>
        state.CurrentToken.Kind == TokenKind.Hash && state.IsAtLineStart();

    public BlockNode? Parse(ParserState state, InlineParser inlineParser)
    {
        var heading = new HeadingNode(1);

        state.MoveNext();
        state.SkipSpace();

        while (!state.IsEndOfLine())
        {
            var inline = inlineParser.ParseInline();
            if (inline != null)
                heading.Inlines.Add(inline);
        }

        if (state.CurrentToken.Kind == TokenKind.NewLine)
            state.MoveNext();
        return heading;
    }
}
