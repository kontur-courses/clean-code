using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
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
                    new TokenPairInfo(x.Opening!.Token, true, x.Closing!.Token),
                    new TokenPairInfo(x.Closing.Token, false, x.Opening.Token),
                })
                .ToDictionary(x => x.Token.StartPosition);
        }

        public ICollection<Token> ResolvePairs()
        {
            if (result != null)
                return result;
            result = new List<Token>();

            foreach (var token in source)
            {
                if (token is PairedToken paired)
                {
                    if (pairedTokens.TryGetValue(paired.StartPosition, out var pairInfo))
                        ProcessPairedToken(paired, pairInfo);
                    else
                        AppendToken(TokenCreator.CreateDefault(token.StartPosition, token.RawValue));
                }
                else AppendToken(token);
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

        public static IEnumerable<Token> ResolvePairs(IEnumerable<Token> tokens) => new TokenPairsResolver(tokens.ToArray())
            .ResolvePairs();

        public static IEnumerable<Token> ReplaceEmptyPairedWithDefault(IEnumerable<Token> tokens) =>
            tokens.Select(t => t is TokenPair p && p.Inner.Length == 0
                ? TokenCreator.CreateDefault(p.Opening.StartPosition, p.Opening.RawValue + p.Closing.RawValue)
                : t);

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