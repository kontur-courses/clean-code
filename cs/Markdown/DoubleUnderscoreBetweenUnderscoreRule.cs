using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class DoubleUnderscoreBetweenUnderscoreRule : IRule
    {
        public List<Token> Apply(List<Token> symbolsMap)
        {
            return DeleteDoubleUnderscoreBetweenUnderscore(symbolsMap);
        }

        private List<Token> DeleteDoubleUnderscoreBetweenUnderscore(List<Token> symbolsMap)
        {
            if (symbolsMap.Count == 0)
                return new List<Token>();
            var maxPosition = symbolsMap.Max(x => x.Position);
            var deleteToken = new List<Token>();
            for (var i = 0; i < maxPosition; i++)
            {
                var startUnderscore = GetPosition(i, maxPosition, "_", TokenType.Start, symbolsMap);
                if (startUnderscore == null)
                    break;
                var endUnderscore = GetPosition(startUnderscore.Position, maxPosition, "_", TokenType.End, symbolsMap);
                i = startUnderscore.Position;
                var startDoubleUnderscore = GetPosition(startUnderscore.Position, endUnderscore.Position, "__",
                    TokenType.Start,
                    symbolsMap);
                if (startDoubleUnderscore == null)
                    continue;
                deleteToken.Add(startDoubleUnderscore);
                var endDoubleUnderscore = GetPosition(startDoubleUnderscore.Position, endUnderscore.Position, "__",
                    TokenType.End, symbolsMap);
                if (endDoubleUnderscore == null)
                    continue;
                deleteToken.Add(endDoubleUnderscore);
            }

            return symbolsMap.Except(deleteToken).ToList();
        }

        private Token GetPosition(int startPosition, int endPosition, string symbol, TokenType type,
            List<Token> symbolsMap)
        {
            return symbolsMap
                .Find(s => s.Data.Symbol == symbol &&
                           s.TokenType == type &&
                           s.Position >= startPosition &&
                           s.Position <= endPosition);
        }
    }
}