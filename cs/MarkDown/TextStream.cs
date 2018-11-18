using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace MarkDown
{
    public class TextStream
    {
        public int CurrentPosition { get; private set; }

        public int Length => Text.Length;

        private string Text { get; }

        public TextStream(string text)
        {
            Text = text;
            CurrentPosition = 0;
        }

        public bool TryMoveNext(int delta = 1)
        {
            if (CurrentPosition + delta > Text.Length) return false;
            CurrentPosition+=delta;
            return true;
        }

        public bool IsCurrentOpening(string specialSymbol, IEnumerable<string> symbols)
        {
            if (!TryGetSubstring(CurrentPosition, specialSymbol.Length, out var cur)) 
                return false;
            var part = IsParOfAnotherSpecSymbol(CurrentPosition, cur, symbols);
            if (!TryGetSubstring(CurrentPosition - 1, 1, out var prev) && cur != specialSymbol && !part) 
                return false;
            if (!TryGetSubstring(CurrentPosition + specialSymbol.Length, 1, out var next)) 
                return false;
            return prev != @"\" && next != " " && cur.StartsWith(specialSymbol) && !part;
        }

        public bool IsSymbolAtPositionClosing(int position, string specialSymbol, IEnumerable<string> symbols)
        {
            if (!TryGetSubstring(position, specialSymbol.Length, out var cur)) 
                return false;            
            var part = IsParOfAnotherSpecSymbol(position, cur, symbols);
            if (!TryGetSubstring(position + specialSymbol.Length, 1, out var next) && cur != specialSymbol && !part)
                return false;
            if (!TryGetSubstring(position - 1, 1, out var prev)) return false;
            return prev != " " && prev != @"\" && cur.StartsWith(specialSymbol) && !part;
        }

        private bool IsParOfAnotherSpecSymbol(int position, string symbol, IEnumerable<string> symbols)
        {
            foreach (var sym in symbols)
            {
                TryGetSubstring(position, sym.Length, out var curNext);
                TryGetSubstring(position - sym.Length + 1, sym.Length, out var curPrev);
                return curNext == sym && curNext.StartsWith(symbol)
                    || curPrev == sym && curPrev.EndsWith(symbol);
            }

            return false;
        }

        public bool IsTokenAtCurrentNumberLess(int endPosition)
        {
            return !(Text.Substring(0, CurrentPosition).Split().Last().Any(char.IsDigit)
                     || Text.Substring(endPosition).Split().First().Any(char.IsDigit));
        }

        public bool TryGetSubstring(int startPosition, int length, out string substring)
        {
            substring = startPosition < 0 || startPosition + length > Length ? Empty : Text.Substring(startPosition, length);
            return substring != Empty;
        }
    }
}