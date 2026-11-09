using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.InlinePieces
{
    public sealed record StrongInlinePiece : InlinePiece
    {
        public InlinePiece[] Children { get; }

        public StrongInlinePiece(InlinePiece[] children) { Children = children; }
    }
}
