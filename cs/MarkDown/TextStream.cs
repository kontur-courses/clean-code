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
            const int symbolLength = 1;
            if (!TryGetSubstring(CurrentPosition, specialSymbol.Length, out var cur)) 
                return false;
            var isParOfAnotherSpecSymbol = IsParOfAnotherSpecSymbol(CurrentPosition, cur, symbols);
            if (!TryGetSubstring(CurrentPosition - symbolLength, symbolLength, out var prev) && cur != specialSymbol && !isParOfAnotherSpecSymbol) 
                return false;
            if (!TryGetSubstring(CurrentPosition + specialSymbol.Length, symbolLength, out var next)) 
                return false;
            return prev != @"\" && next != " " && cur.Equals(specialSymbol) && !isParOfAnotherSpecSymbol;
        }

        public bool IsSymbolAtPositionClosing(int position, string specialSymbol, IEnumerable<string> symbols)
        {
            const int symbolLength = 1;
            if (!TryGetSubstring(position, specialSymbol.Length, out var cur)) 
                return false;            
            var isParOfAnotherSpecSymbol = IsParOfAnotherSpecSymbol(position, cur, symbols);
            if (!TryGetSubstring(position + specialSymbol.Length, symbolLength, out var next) && cur != specialSymbol && !isParOfAnotherSpecSymbol)
                return false;
            if (!TryGetSubstring(position - symbolLength, symbolLength, out var prev)) return false;
            return prev != " " && prev != @"\" && cur.Equals(specialSymbol) && !isParOfAnotherSpecSymbol;
        }

        private bool IsParOfAnotherSpecSymbol(int position, string symbol, IEnumerable<string> symbols)
        {
            foreach (var sym in symbols)
            {
                if (sym == symbol) return false;
                TryGetSubstring(position, sym.Length, out var curNext);
                TryGetSubstring(position - sym.Length + 1, sym.Length, out var curPrev);
                return curNext == sym && curNext.StartsWith(symbol)
                    || curPrev == sym && curPrev.EndsWith(symbol);
            }

            return false;
        }

        public bool IsTokenAtCurrentNumberless(int endPosition)
        {
            var prev = Text.Substring(0, CurrentPosition);
            var next = Text.Substring(endPosition + 1);
            if (next.StartsWith(" ") || prev.EndsWith(" ")) return true;
            return !(next.Any(char.IsDigit) || next.Any(char.IsDigit));
        }

        public bool TryGetSubstring(int startPosition, int length, out string substring)
        {
            substring = (startPosition < 0 || startPosition + length > Length) ? Empty : Text.Substring(startPosition, length);
            return substring != Empty;
        }

        public bool Contains(string value) => Text.Contains(value);

        public bool TryReadUntilClosing(string closingSymbol, IEnumerable<string> symbols, out string content, bool checkToken = false)
        {
            var length = closingSymbol.Length;
            content = null;
            for (var i = CurrentPosition + length; i < Length - length + 1; i++)
            {
                if (!IsSymbolAtPositionClosing(i, closingSymbol, symbols)) 
                    continue;
                if (!TryGetSubstring(CurrentPosition + length, i - CurrentPosition - length, out content)) 
                    continue;
                if (checkToken)
                {
                    content = closingSymbol != "__" && closingSymbol != "_" || IsTokenAtCurrentNumberless(i) ? content: null;
                    continue;;
                }
                CurrentPosition = i + 1;
                return true;
            }
            return content != null;
        }
    }
}