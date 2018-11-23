using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MarkDown
{
    internal static class StringExtensions
    {
        internal static List<Character> GetCharStates(this string text, List<string> specSymbols)
        {
            var result = text.Select(s => new Character(s, CharState.NotEscaped)).ToList();
            for (var i = 0; i < text.Length; i++)
            {
                if (i == text.Length - 1 || text[i] != '\\' || result[i].CharState == CharState.Escaped) continue;
                foreach (var specSymbol in specSymbols)
                {
                    if (i+1 + specSymbol.Length > text.Length) continue;
                    var next = text.Substring(i+1, specSymbol.Length);
                    if (!next.Equals(specSymbol)) continue;
                    result[i] = new Character(text[i], CharState.Ignored);
                    result = result.Escape(i + 1, specSymbol.Length);
                    break;
                }
            }
            return result;
        }
        
        private static List<Character> Escape(this List<Character> charStates, int start, int length)
        {
            for (var i = start; i < start + length; i++)
                charStates[i] = new Character(charStates[i].Char, CharState.Escaped);
            return charStates;
        }
    }
}
