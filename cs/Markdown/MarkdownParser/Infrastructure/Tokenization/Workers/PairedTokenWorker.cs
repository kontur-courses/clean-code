using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public static class PairedTokenWorker
    {
        public static IEnumerable<Token> FixCrossingTokens(this IEnumerable<Token> unpaired)
        {
            unpaired = unpaired.ToArray();
            var pairedTokensMap = GetPairedTokens(unpaired);
            return FixCrossingTokens(unpaired, pairedTokensMap);
        }

        private static IEnumerable<Token> FixCrossingTokens(IEnumerable<Token> unpaired,
            (PairedTokenData Opening, PairedTokenData Closing)[] pairedTokens)
        {
            var crossing = EnumerateCrossingTokens(pairedTokens).ToHashSet();

            foreach (var token in unpaired)
            {
                if (token is PairedToken pairedToken && crossing.Contains(pairedToken))
                {
                    crossing.Remove(pairedToken);
                    yield return new TextToken(pairedToken.StartPosition, pairedToken.RawValue);
                }
                else yield return token;
            }
        }

        private static IEnumerable<PairedToken> EnumerateCrossingTokens(
            (PairedTokenData Opening, PairedTokenData Closing)[] pairedTokens) =>
            pairedTokens.SelectMany((x, i) =>
                pairedTokens.Skip(i + 1)
                    .Where(y =>
                        y.Opening!.Index > x.Opening.Index &&
                        y.Closing!.Index > x.Closing.Index &&
                        y.Opening.Index < x.Closing.Index)
                    .SelectMany(y => new[]
                    {
                        x.Closing.Token,
                        x.Opening.Token,
                        y.Closing.Token,
                        y.Opening.Token
                    }));

        public static (PairedTokenData Opening, PairedTokenData Closing)[] GetPairedTokens(IEnumerable<Token> unpaired)
        {
            var pairedTokens = unpaired.Select((t, i) => new PairedTokenData(i, t as PairedToken))
                .Where(x => x.Token != null); //Такой порядок операций чтобы не потерять индексы

            var closedTokens = new List<(PairedTokenData Opening, PairedTokenData Closing)>();
            var openedTokens = new Dictionary<Type, PairedTokenData>();

            foreach (var current in pairedTokens)
            {
                if (openedTokens.TryGetValue(current.Token.GetType(), out var existing))
                {
                    ProcessAlreadyOpened(current, existing, openedTokens, out var isClosed);
                    if (isClosed) closedTokens.Add((Opening: existing, Closing: current));
                }
                else ProcessNotOpened(current, openedTokens);
            }

            return closedTokens.ToArray();
        }

        private static void ProcessAlreadyOpened(PairedTokenData current, PairedTokenData existing,
            Dictionary<Type, PairedTokenData> openedTokens, out bool isClosed)
        {
            if (!current.Token.CanBeClosing())
            {
                AddOrReplaceOpening(current, openedTokens);
                isClosed = false;
            }
            else
            {
                CloseOpened(existing, openedTokens);
                isClosed = true;
            }
        }

        private static void ProcessNotOpened(PairedTokenData current, Dictionary<Type, PairedTokenData> openedTokens)
        {
            if (current.Token.CanBeOpening())
                AddOrReplaceOpening(current, openedTokens);
        }

        private static void AddOrReplaceOpening(PairedTokenData current,
            Dictionary<Type, PairedTokenData> openedTokens) =>
            openedTokens[current.Token.GetType()] = current;

        private static void CloseOpened(PairedTokenData existing, Dictionary<Type, PairedTokenData> openedTokens) =>
            openedTokens.Remove(existing.Token.GetType());
    }
}