using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.InlinePieces
{
    public sealed record TextInlinePiece : InlinePiece
    {
        public string Text { get; }

        public TextInlinePiece(string text) { Text = text; }
    }
}
