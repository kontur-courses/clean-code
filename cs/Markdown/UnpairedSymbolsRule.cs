using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Markdown
{
    public class UnpairedSymbolsRule : IRule
    {
        public List<Token> Apply(List<Token> symbolsMap, List<TokenInformation> baseTokens)
        {
            var pairedSymbols = baseTokens.Where(t => t.IsPaired).ToArray();
            var correctTokens = new List<Token>();
            foreach (var pairedSymbol in pairedSymbols)
                correctTokens.AddRange(DeleteNotPairSymbols(symbolsMap, pairedSymbol.Symbol));

            return correctTokens;
        }

        private List<Token> DeleteNotPairSymbols(List<Token> symbolsMap, string symbol)
        {
            var startPos = GetAllPosition(symbol, TokenType.Start, symbolsMap);
            var endPos = GetAllPosition(symbol, TokenType.End, symbolsMap);

            var pairedSymbols = new List<int>();
            for (var i = 0; i < endPos.Length; i++)
            {
                var end = endPos[i];
                var start = GetStartPositionForEnd(startPos, end, end, pairedSymbols);
                if (start == -1)
                    continue;
                pairedSymbols.Add(end);
                pairedSymbols.Add(start);
            }
            
            var correctTokens = new List<Token>();
            foreach (Token token in symbolsMap)
            {
               if (pairedSymbols.Contains(token.Position) || token.TokenType == TokenType.Escaped || token.TokenType == TokenType.Ordinary)
                   correctTokens.Add(token);
            }
           return correctTokens;
        }

        private int[] GetAllPosition(string symbol, TokenType type, List<Token> symbolsMap)
        {
            return symbolsMap.Where(s => s.Data.Symbol == symbol && s.TokenType == type)
                .Select(s => s.Position)
                .ToArray();
        }

        private int GetStartPositionForEnd(int[] startPos, int indexEnd, int bound, List<int> paired)
        {
            if (!IsArrayContainFreeValue(startPos, paired, indexEnd))
                return -1;
            var start = startPos.Last(p => p < bound);
            if (paired.Contains(start))
                start = GetStartPositionForEnd(startPos, indexEnd, start, paired);
            return start;
        }

        private bool IsArrayContainFreeValue(int[] array, List<int> paired, int indexEnd)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var value = array[i];
                if (!paired.Contains(value) && value < indexEnd)
                    return true;
            }

            return false;
        }
    }
}