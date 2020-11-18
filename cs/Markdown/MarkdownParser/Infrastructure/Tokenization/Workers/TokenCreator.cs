using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Helpers;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public static class TokenCreator
    {
        public static bool TryCreateFrom(ICollection<ITokenBuilder> tokenBuilders, TokenizationContext context,
            out Token token)
        {
            var symbolsCount = GetSymbolsCount(tokenBuilders, context);
            var str = context.Source.Substring(context.CurrentStartIndex, symbolsCount);
            var builder = tokenBuilders.Where(t => str.StartsWith(t.TokenSymbol)).WithMax(x => x.TokenSymbol.Length);

            if (builder == null)
            {
                token = default;
                return false;
            }

            token = builder.CanCreate(context)
                ? builder.Create(context)
                : CreateDefault(context.CurrentStartIndex, builder.TokenSymbol);
            return true;
        }

        private static int GetSymbolsCount(ICollection<ITokenBuilder> tokenBuilders, TokenizationContext context)
        {
            return Math.Min(
                tokenBuilders.Max(tb => tb.TokenSymbol.Length),
                context.Source.Length - context.CurrentStartIndex);
        }

        public static TextToken CreateDefault(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);

        public static TextToken CreateDefault(Token proto) => CreateDefault(proto.StartPosition, proto.RawValue);
    }
}