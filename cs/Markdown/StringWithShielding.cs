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
                if (index > 0  &&  FunctionSymbols.Contains(Text[index]))
                {
                    var shieldingCount = 0;
                    for (var i = index - 1; i >= 0 && Text[i] == ShieldingSymbol; i--)
                        shieldingCount++;
                    return shieldingCount % 2 == 1 ? ShieldingReplacement : Text[index];
                }
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
                if (i + 1 <= endIndex && Text[i] == ShieldingSymbol)
                {
                    if (FunctionSymbols.Contains(Text[i+1]))
                        continue;
                    if (Text[i+1] == ShieldingSymbol)
                    {
                        substring.Append(ShieldingSymbol);
                        i++;
                        continue;
                    }
                }
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