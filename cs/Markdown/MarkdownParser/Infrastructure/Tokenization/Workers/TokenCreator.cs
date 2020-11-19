using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Helpers;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public static class TokenCreator
    {
        public static bool TryCreateFrom(ICollection<ITokenBuilder> tokenBuilders, string raw, int startIndex, 
            out Token token)
        {
            var symbolsCount = GetSymbolsCount(tokenBuilders, raw, startIndex);
            var str = raw.Substring(startIndex, symbolsCount);
            var builder = tokenBuilders.Where(t => str.StartsWith(t.TokenSymbol)).WithMax(x => x.TokenSymbol.Length);

            if (builder == null)
            {
                token = default;
                return false;
            }

            token = builder.CanCreate(raw, startIndex)
                ? builder.Create(raw, startIndex)
                : CreateDefault(startIndex, builder.TokenSymbol);
            return true;
        }

        private static int GetSymbolsCount(ICollection<ITokenBuilder> tokenBuilders, string raw, int startIndex)
        {
            return Math.Min(
                tokenBuilders.Max(tb => tb.TokenSymbol.Length),
                raw.Length - startIndex);
        }

        public static TextToken CreateDefault(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);

        public static TextToken CreateDefault(Token proto) => CreateDefault(proto.StartPosition, proto.RawValue);
    }
}