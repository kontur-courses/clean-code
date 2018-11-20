using System;
using System.Collections.Generic;

namespace Markdown
{
    public class UnpairedSymbolsRule : IRule
    {
        public SortedList<int, Token> Apply(SortedList<int, Token> symbolsMap)
        {
            throw new System.NotImplementedException();
        }
        private SortedList<int, Token> DeleteNotPairSymbols(SortedList<int, Token> symbolsMap, SymbolType symbol)
        {
            throw new NotImplementedException();
        }
    }
}