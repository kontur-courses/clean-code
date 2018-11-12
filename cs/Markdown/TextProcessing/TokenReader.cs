using System;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TokenReader
    {
        private int Position { get; set; }

        public Token ReadUntil(Func<char, bool> isStopChar)
        {
            throw new NotImplementedException();
        }

        public Token ReadWhile(Func<char, bool> accept)
        {
            throw new NotImplementedException();
        } 
    }
}