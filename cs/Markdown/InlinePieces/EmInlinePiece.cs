using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.InlinePieces
{
    public sealed record EmInlinePiece : InlinePiece
    {
        public InlinePiece[] Children { get; }

        public EmInlinePiece(InlinePiece[] children) { Children = children; }
    }
}
