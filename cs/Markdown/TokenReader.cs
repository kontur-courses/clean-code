using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TokenReader
    {
        public static Token ReadUntil(Func<string, int, StopSymbolDecision> isStopChar, string input, int position)
        {
            throw new NotImplementedException();
        }

        public static Token ReadWhile(Func<char, bool> isControlSymbol, string input, int position)
        {
            throw new NotImplementedException();
        }
    }
}