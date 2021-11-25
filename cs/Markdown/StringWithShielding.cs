using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class StringWithShielding 
    {
        public readonly string Text;
        public readonly char ShieldingSymbol;
        public readonly HashSet<char> FunctionSymbols;
        public readonly char ShieldingReplacement;

        public int Length => Text.Length;

        public StringWithShielding(string text, char shieldingSymbol, char shieldingReplacement, HashSet<char> functionSymbols)
        {
            Text = text;
            ShieldingSymbol = shieldingSymbol;
            ShieldingReplacement = shieldingReplacement;
            FunctionSymbols = functionSymbols;
        }
        
        public char this[int index]
        {
            get
            {
                if (index >= Text.Length)
                    throw new IndexOutOfRangeException();
                if (index > 0  && Text[index - 1] == ShieldingSymbol && FunctionSymbols.Contains(Text[index]))
                    return ShieldingReplacement;
                return Text[index];
            }
        }

        public string ShieldedSubstring(int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex >= Text.Length)
                throw new IndexOutOfRangeException();
            var substring = new StringBuilder();
            for (var i = startIndex; i <= endIndex; i++)
            {
                if (i + 1 <= endIndex && Text[i] == ShieldingSymbol && FunctionSymbols.Contains(Text[i+1]))
                    continue;
                substring.Append(Text[i]);
            }
            return substring.ToString();
        }
        
        public bool ContainsAt(int index, string substring)
        {
            if (substring.Length > Text.Length - index)
                return false;
            return !substring.Where((t, i) => this[index + i] != t).Any();
        }

        public int IndexOf(char c, int startIndex)
        {
            for (var i = startIndex; i < Text.Length; i++)
            {
                if (this[i] == c)
                    return i;
            }
            return -1;
        }
    }
}