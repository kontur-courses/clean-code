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
            var builder = tokenBuilders
                .Where(t => raw.IndexOf(t.TokenSymbol, startIndex, StringComparison.Ordinal) == startIndex)
                .WithMax(x => x.TokenSymbol.Length);

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

        public static TextToken CreateDefault(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);
    }
}