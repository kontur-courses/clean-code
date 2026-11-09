using Markdown.InlinePieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class InlineParser
    {
        public static RenderedBlock Parse(RawBlock block)
        {
            var tokens = InlineTokenizer.Tokenize(block.Content);
            var pieces = InlineResolver.Resolve(tokens);
            return new RenderedBlock(block.Type, pieces);
        }
    }

    public class RenderedBlock
    {
        public BlockType Type { get; }
        public InlinePiece[] InlinePieces { get; }

        public RenderedBlock(BlockType type, InlinePiece[] linePieces)
        {
            Type = type;
            InlinePieces = linePieces;
        }
    }
}
