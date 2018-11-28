using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class UnpairedSymbolsRule : IRule
    {
        private readonly List<TokenInformation> baseTokens;

        public UnpairedSymbolsRule(List<TokenInformation> baseTokens)
        {
            this.baseTokens = baseTokens;
        }

        public List<Token> Apply(List<Token> symbolsMap)
        {
            var ordinaryAndEscapedSymbols = GetOrdinaryAndEscapedSymbols(symbolsMap);
            if (!symbolsMap.Any(s => s.Data.IsPaired))
                return symbolsMap;
            var pairedSymbols = baseTokens.Where(t => t.IsPaired).ToArray();
            var correctTokens = new List<Token>();
            foreach (var pairedSymbol in pairedSymbols)
                correctTokens.AddRange(DeleteNotPairSymbols(symbolsMap, pairedSymbol.Symbol));

            correctTokens.AddRange(ordinaryAndEscapedSymbols);
            return correctTokens;
        }

        private List<Token> GetOrdinaryAndEscapedSymbols(List<Token> symbolsMap)
        {
            return symbolsMap.Where(s => s.TokenType == TokenType.Escaped || s.TokenType == TokenType.Ordinary)
                .ToList();
        }

        private List<Token> DeleteNotPairSymbols(List<Token> symbolsMap, string symbol)
        {
            var startPos = GetAllPosition(symbol, TokenType.Start, symbolsMap);
            var endPos = GetAllPosition(symbol, TokenType.End, symbolsMap);

            var pairedSymbols = new List<int>();

            foreach (var end in endPos)
            {
                var start = GetStartPositionForEnd(startPos, end, end, pairedSymbols);
                if (start == -1)
                    continue;
                pairedSymbols.Add(end);
                pairedSymbols.Add(start);
            }

            var correctTokens = new List<Token>();
            foreach (var token in symbolsMap)
                if (pairedSymbols.Contains(token.Position))
                    correctTokens.Add(token);

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
            if (!IsArrayContainAvailableStart(startPos, paired, indexEnd))
                return -1;
            var start = startPos.Last(p => p < bound);
            if (paired.Contains(start))
                start = GetStartPositionForEnd(startPos, indexEnd, start, paired);
            return start;
        }

        private bool IsArrayContainAvailableStart(int[] startPos, List<int> paired, int indexEnd)
        {
            foreach (var value in startPos)
                if (!paired.Contains(value) && value < indexEnd)
                    return true;

            return false;
        }
    }
}