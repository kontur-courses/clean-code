using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            if (text == null)
                throw new NullReferenceException("input must be null");

            var markdownSymbols = new Stack<Tag>();
            var resultText = new StringBuilder();

            foreach (var symbol in text)
            {
                resultText.Append(symbol);
                FilterMarkdownSymbols(symbol, markdownSymbols);

                var currentSymbol = symbol.ToString();
                if (!Tag.IsTag(currentSymbol))
                    continue;
                if (CanReplaceSymbolsOnTags(currentSymbol, markdownSymbols))
                    resultText = resultText.ReplaceSymbolsOnTags(markdownSymbols.Pop());
                else markdownSymbols.Push(new Tag(currentSymbol, resultText.Length - 1));
            }

            return resultText.ToString();
        }

        private static void FilterMarkdownSymbols(char symbol, Stack<Tag> symbols)
        {
            if (symbols.Count > 0 && (symbol == ' ' || char.IsDigit(symbol)))
                symbols.Pop();
        }

        private static bool CanReplaceSymbolsOnTags(string symbol, Stack<Tag> symbols)
        {
            return symbols.Count > 0 && symbols.Peek().Value == symbol;
        }
    }
}