using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.InlinePieces
{
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
