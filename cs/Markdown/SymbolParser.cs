using System;
using System.Collections.Generic;

namespace Markdown
{
    public class SymbolParser
    {
        public SortedDictionary<int, SymbolType> GetTagsPosition(string mdText)
        {
            var tagsPosition = new SortedDictionary<int, SymbolType>();
            for (var i = 0; i < mdText.Length; i++)
            {
                var symbol = GetSymbolType(i, mdText);
                if (symbol == SymbolType.Ordinary)
                    continue;

                tagsPosition.Add(i, symbol);
                if (symbol == SymbolType.DoubleUnderscore)
                    i++;
            }
            return tagsPosition;
        }

        private SymbolType GetSymbolType(int index, string mdText)
        {
            if (!CorrectIndex(index, mdText))
                throw new IndexOutOfRangeException();
            var symbol = mdText[index];
            var nextIndex = index + 1;
            if (CorrectIndex(nextIndex, mdText))
            {
                var nextSymbol = mdText[nextIndex];
                if (symbol == '_' && nextSymbol == '_')
                    return SymbolType.DoubleUnderscore;
            }
            if (symbol == '_')
                return SymbolType.Underscore;
            if (symbol == '`')
                return SymbolType.GraveAccent;
            if (symbol == '\\')
                return SymbolType.Backslash;
            return SymbolType.Ordinary;
        }

        private bool CorrectIndex(int index, string mdText)
        {
            return index >= 0 && index < mdText.Length;
        }
    }
}