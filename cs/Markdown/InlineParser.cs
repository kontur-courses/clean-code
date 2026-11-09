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

    public abstract record InlinePiece { }

    public sealed record TextInlinePiece : InlinePiece
    {
        public string Text { get; }

        public TextInlinePiece(string text) {  Text = text; }
    }

    public sealed record EmInlinePiece : InlinePiece
    {
        public InlinePiece[] Children { get; }

        public EmInlinePiece(InlinePiece[] children) { Children = children; }
    }

    public sealed record StrongInlinePiece : InlinePiece
    {
        public InlinePiece[] Children { get; }

        public StrongInlinePiece(InlinePiece[] children) { Children = children; }
    }

    public sealed record LinkInlinePiece : InlinePiece
    {
        public string Url { get; }
        public InlinePiece[] Children { get; }

        public LinkInlinePiece(string url, InlinePiece[] children)
        {
            Url = url;
            Children = children;
        }
    }
}
