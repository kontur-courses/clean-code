using System.Collections.Generic;
using System.Text;

namespace MarkDown
{
    public static class StringExtensions
    {
        public static string Escape(this string text, List<string> specSymbols)
        {
            var result = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                if (i != text.Length - 1 && text[i] == '\\')
                {
                    if (!ShouldEscape(specSymbols, i + 1, text))
                        result.Append(text[i]);
                    continue;
                }
                result.Append(text[i]);
            }            
            return result.ToString();
        }

        private static bool ShouldEscape(List<string> specSymbols, int position, string text)
        {
            foreach (var specSymbol in specSymbols)
            {
                if (position + specSymbol.Length > text.Length) continue;
                var next = text.Substring(position, specSymbol.Length);
                if (next.Equals(specSymbol)) return true;
            }
            return false;
        }
    }
}
