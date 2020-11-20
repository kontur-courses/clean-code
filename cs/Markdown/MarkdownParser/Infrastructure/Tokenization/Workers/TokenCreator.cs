using System;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public class TokenCreator
    {
        private readonly int maxTokenLength;
        private readonly ITokenBuilder[] orderedTokenBuilders;
        private readonly string raw;

        public TokenCreator(ITokenBuilder[] tokenBuilders, string raw)
        {
            this.raw = raw;
            orderedTokenBuilders = tokenBuilders.OrderByDescending(x => x.TokenSymbol.Length).ToArray();
            maxTokenLength = orderedTokenBuilders[0].TokenSymbol.Length;
        }

        public bool TryCreateOnPosition(int startIndex, out Token token)
        {
            var symbolsCount = Math.Min(maxTokenLength, raw.Length - startIndex);
            var builder = orderedTokenBuilders.FirstOrDefault(t =>
                raw.IndexOf(t.TokenSymbol, startIndex, symbolsCount, StringComparison.Ordinal) == startIndex);

            if (builder != null)
            {
                token = builder.CanCreate(raw, startIndex)
                    ? builder.Create(raw, startIndex)
                    : CreateDefault(startIndex, builder.TokenSymbol);
                return true;
            }

            token = default;
            return false;
        }

        public static TextToken CreateDefault(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);
    }
}