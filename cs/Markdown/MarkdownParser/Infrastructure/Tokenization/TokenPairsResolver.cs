using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public class TokenPairsResolver
    {
        private readonly Token[] source;
        private readonly Dictionary<int, TokenPairInfo> pairedTokens;
        private TokenPairBuilder currentBuilder;
        private List<Token> result;

        public TokenPairsResolver(Token[] source)
        {
            this.source = source;
            pairedTokens = PairedTokenWorker.GetPairedTokens(this.source)
                .SelectMany(x => new[]
                {
                    new TokenPairInfo(x.Opening.Token, true, x.Closing.Token),
                    new TokenPairInfo(x.Closing.Token, false, x.Opening.Token),
                })
                .ToDictionary(x => x.Token.StartPosition);
        }

        public ICollection<Token> ResolvePairs()
        {
            if (result == null)
            {
                result = new List<Token>();
                foreach (var token in source)
                {
                    if (token is PairedToken paired && pairedTokens.TryGetValue(paired.StartPosition, out var pairInfo))
                        ProcessPairedToken(paired, pairInfo);
                    else AppendToken(token);
                }
            }

            return result;
        }

        private void ProcessPairedToken(PairedToken pairedToken, TokenPairInfo pairInfo)
        {
            if (pairInfo.IsOpening)
                currentBuilder = new TokenPairBuilder(pairedToken, pairInfo.Other, currentBuilder);
            else
            {
                var tokenPair = currentBuilder!.Build();
                currentBuilder = currentBuilder.Parent;
                AppendToken(tokenPair);
            }
        }

        private void AppendToken(Token token)
        {
            if (currentBuilder != null) currentBuilder.Inner.Add(token);
            else result.Add(token);
        }

        public static Token[] ResolvePairs(Token[] tokens) => new TokenPairsResolver(tokens).ResolvePairs().ToArray();

        private class TokenPairBuilder
        {
            public TokenPairBuilder(PairedToken opening, PairedToken closing, TokenPairBuilder parent)
            {
                Closing = closing;
                Opening = opening;
                Parent = parent;
            }

            public PairedToken Opening { get; }
            public PairedToken Closing { get; }
            public List<Token> Inner { get; } = new List<Token>();
            public TokenPairBuilder Parent { get; }

            public TokenPair Build() => new TokenPair(Opening, Inner.ToArray(), Closing);
        }

        private class TokenPairInfo
        {
            public PairedToken Token { get; }
            public bool IsOpening { get; }
            public PairedToken Other { get; }

            public TokenPairInfo(PairedToken token, bool isOpening, PairedToken other)
            {
                Token = token;
                IsOpening = isOpening;
                Other = other;
            }
        }
    }
}