using Markdown.Core.Lexing;
using Markdown.Core.Parsing.Nodes;

namespace Markdown.Core.Parsing.Blocks;

public class ParagraphBlockParser : IBlockParser
{
    public bool CanParse(ParserState state) =>
        state.CurrentToken.Kind is not TokenKind.Eof;

    public BlockNode? Parse(ParserState state, InlineParser inlineParser)
    {
        var paragraph = new ParagraphNode();

        while (!state.IsEndOfLine())
        {
            var inline = inlineParser.ParseInline();
            if (inline != null)
                paragraph.Inlines.Add(inline);
        }

        if (state.CurrentToken.Kind != TokenKind.NewLine) return paragraph;
        state.MoveNext();
        state.SkipEmptyLines();
        return paragraph;
    }
}
