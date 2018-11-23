using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace MarkDown
{
    public class TextStream
    {
        public int CurrentPosition { get; private set; }

        public int Length => Text.Count;

        private List<Character> Text { get; }

        public bool HaveLink { get; }

        public TextStream(List<Character> text)
        {
            Text = text;
            HaveLink = Contains("[") && Contains("]");
            CurrentPosition = 0;
        }

        public bool TryMoveNext(int delta = 1)
        {
            if (CurrentPosition + delta > Text.Count) return false;
            CurrentPosition+=delta;
            return true;
        }

        public bool IsCurrentOpening(string specialSymbol, IEnumerable<string> symbols)
        {
            const int symbolLength = 1;
            if (!TryGetSubstring(CurrentPosition, specialSymbol.Length, out var cur)) return false;
            var curString = Concat(cur.Select(s => s.Char));
            var isPartOfSpecSymbol = IsParOfAnotherSpecSymbol(CurrentPosition, curString, symbols);
            if (!TryGetSubstring(CurrentPosition + specialSymbol.Length, symbolLength, out var prev)
                && !curString.Equals(specialSymbol) && !isPartOfSpecSymbol)
                return false;
            if (!TryGetSubstring(CurrentPosition + specialSymbol.Length, symbolLength, out var next)) 
                    return false;
            return cur.All(s => s.CharState != CharState.Escaped)
                   && next.All(s => !char.IsWhiteSpace(s.Char)) && curString == specialSymbol 
                   && !isPartOfSpecSymbol;
        }

        public bool IsSymbolAtPositionClosing(int position, string specialSymbol, IEnumerable<string> symbols)
        {
            const int symbolLength = 1;
            if (!TryGetSubstring(position, specialSymbol.Length, out var cur))
                return false;
            var curString = Concat(cur.Select(s => s.Char));
            var isPartOfSpecSymbol = IsParOfAnotherSpecSymbol(position, curString, symbols);
            if (!TryGetSubstring(position + specialSymbol.Length, symbolLength, out var next) 
                && curString != specialSymbol && !isPartOfSpecSymbol)
                return false;
            if (!TryGetSubstring(position - symbolLength, symbolLength, out var prev)) return false;
            return cur.All(s => s.CharState != CharState.Escaped)
                   && prev.All(s => !char.IsWhiteSpace(s.Char)) && curString == specialSymbol
                   && !isPartOfSpecSymbol;
        }

        private bool IsParOfAnotherSpecSymbol(int position, string symbol, IEnumerable<string> symbols)
        {
            foreach (var sym in symbols)
            {
                if (sym == symbol) continue;
                if (TryGetSubstring(position, sym.Length, out var curNext) 
                    && Concat(curNext.Select(s => s.Char)) == sym && curNext.First().Char.ToString().Equals(symbol) 
                    || TryGetSubstring(position - sym.Length + 1, sym.Length, out var curPrev)
                    && Concat(curPrev.Select(s => s.Char)) == sym && curPrev.Last().Char.ToString().Equals(symbol))
                return true;
            }
            return false;
        }

        public bool IsTokenAtCurrentNumberless(int endPosition)
        {
            var prev = Text.GetRange(0, CurrentPosition);
            var next = Text.Skip(endPosition + 1).ToList();
            if (next.Count != 0 && next.First().Char.Equals(' ') 
                || prev.Count != 0 && prev.Last().Char.Equals(' ')) return true;
            return !(next.Any(s => char.IsDigit(s.Char)) || prev.Any(s => char.IsDigit(s.Char)));
        }

        public bool TryGetSubstring(int startPosition, int length, out List<Character> substring)
        {
            substring = (startPosition < 0 || startPosition + length > Length) ? null : Text.GetRange(startPosition, length);
            return substring != null;
        }

        public bool TryReadUntilClosing(string closingSymbol, IEnumerable<string> symbols, out List<Character> content, bool checkToken = false)
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

        private bool Contains(string val)
        {
            return Concat(Text.Select(s => s.Char)).Contains(val);
        }
    }
}