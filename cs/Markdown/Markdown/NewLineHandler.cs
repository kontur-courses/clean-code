using System;

namespace Markdown
{
    internal static class NewLineHandler
    {
        internal static bool TryGetNewLineSymbolAtTheEnd(string text, out string newLineSymbol)
        {
            newLineSymbol = null;
            if (text.EndsWith("\r\n"))
            {
                newLineSymbol = "\r\n";
            }
            else if (text.EndsWith("\n"))
            {
                newLineSymbol = "\n";
            }
            else
                return false;

            return true;
        }

        internal static bool TryGetNewLineSymbolAtPosition(string text, out string newLineSymbol, int position)
        {
            var maxNewLineSymbolLength = Math.Min(2, text.Length - position);
            newLineSymbol = null;
            while (maxNewLineSymbolLength > 0)
            {
                if (TryGetNewLineSymbolAtTheEnd(text.Substring(position, maxNewLineSymbolLength), out newLineSymbol))
                    return true;
                maxNewLineSymbolLength--;
            }

            return false;
        }
    }
}