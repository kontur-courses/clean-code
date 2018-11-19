using System.Collections.Generic;

namespace Markdown
{
    public class SymbolMap
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
            var symbol = mdText[index];
            var nextIndex = index + 1;
            switch (symbol)
            {
                case '_' when CorrectIndex(nextIndex, mdText) && nextIndex == '_':
                    return SymbolType.DoubleUnderscore;
                case '_':
                    return SymbolType.Underscore;
                case '`':
                    return SymbolType.GraveAccent;
                case '\\':
                    return SymbolType.Backslash;
                default:
                    return SymbolType.Ordinary;
            }
        }

        private bool CorrectIndex(int index, string mdText)
        {
            return index >= 0 && index < mdText.Length;
        }
    }
}