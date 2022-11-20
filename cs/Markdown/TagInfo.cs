using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{   
    public class TagInfo
    {
        public readonly string Symbol;
        public readonly string SymbolOpen;
        public readonly string SymbolClose;

        public TagInfo(string symbol, string symbolOpen, string symbolClose)
        {
            Symbol = symbol;
            SymbolOpen = symbolOpen;
            SymbolClose = symbolClose;
        }
    }
}
