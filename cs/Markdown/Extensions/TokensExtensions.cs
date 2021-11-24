using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class TokensExtensions
    {
        public static HashSet<IToken> GetForbiddenTokens(this IEnumerable<IToken> tokens,
            IEnumerable<PairedToken> pairedTokens, HashSet<IToken> unpairedTokens)
        {
            var forbiddenTokens = pairedTokens.GetIntersectedTokens(
                (i, b) => i.From.Position < b.From.Position
                          && i.To.Position > b.To.Position)
                .Where(t => t.Type == TokenType.Bold)
                .ToHashSet();

            forbiddenTokens.UnionWith(pairedTokens.GetIntersectedTokens(
                (i, b) => i.From.Position > b.From.Position
                          && i.To.Position > b.To.Position));

            forbiddenTokens.UnionWith(tokens.GetEmptyTokens());

            forbiddenTokens.UnionWith(unpairedTokens);
            forbiddenTokens.UnionWith(tokens.GetTokensByRule(pairedTokens,
                (x, y, z) => (x || z) && y,
                c => c != ' ',
                x => !x));

            forbiddenTokens.UnionWith(tokens.GetTokensByRule(pairedTokens,
                (x, y, z) => x && y && z,
                c => c != ' ',
                x => !x));

            forbiddenTokens.UnionWith(tokens.GetTokensByRule(pairedTokens,
                (x, y, z) => (x || z) && y,
                char.IsDigit));
            return forbiddenTokens;
        }

        public static IEnumerable<IToken> GetTokensByRule(this IEnumerable<IToken> tokens,
            IEnumerable<PairedToken> pairedTokens,
            Func<bool, bool, bool, bool> tokensCondition,
            Func<char, bool> predicate) =>
            tokens.GetTokensByRule(pairedTokens, tokensCondition, predicate, x => x);

        public static IEnumerable<IToken> GetTokensByRule(this IEnumerable<IToken> tokens,
            IEnumerable<PairedToken> pairedTokens,
            Func<bool, bool, bool, bool> tokensCondition,
            Func<char, bool> predicate,
            Func<bool, bool> switchResult)
        {
            var previousContent = tokens.First();

            foreach (var pair in pairedTokens)
            {
                var isLeftTokenEndsOnSpace = false;
                var isMiddleTokenMatched = false;
                var isRightTokenBeginsOnSpace = false;
                var isLastToken = false;
                var flag = false;
                foreach (var token in tokens)
                {
                    if (token == pair.From && previousContent != token)
                    {
                        isLeftTokenEndsOnSpace = previousContent.Value.LastOrDefault() != ' ';
                    }
                    if (token == pair.To)
                    {
                        isMiddleTokenMatched = switchResult(previousContent.Value.All(predicate));
                        isLastToken = true;
                    }

                    if (isLastToken && token != pair.To)
                        isRightTokenBeginsOnSpace = token.Value.FirstOrDefault() != ' ';

                    if (tokensCondition(isLeftTokenEndsOnSpace, isMiddleTokenMatched, isRightTokenBeginsOnSpace))
                    {
                        yield return pair.From;
                        yield return pair.To;
                        break;
                    }
                    if (isLastToken && token != pair.To)
                    {
                        previousContent = tokens.First();
                        break;
                    }
                    previousContent = token;
                }
            }
        }

        public static IEnumerable<IToken> GetIntersectedTokens(this IEnumerable<PairedToken> tokens,
            Func<PairedToken, PairedToken, bool> condition)
        {
            var boldTokens = tokens.Where(t => t.Type == TokenType.Bold);
            var italicTokens = tokens.Where(t => t.Type == TokenType.Italics);
            foreach (var bold in boldTokens)
                foreach (var italic in italicTokens)
                    if (condition(italic, bold))
                    {
                        yield return bold.From;
                        yield return bold.To;
                        yield return italic.From;
                        yield return italic.To;
                    }
        }

        public static IEnumerable<IToken> GetEmptyTokens(this IEnumerable<IToken> tokens)
        {
            IToken previousToken = null;
            foreach (var token in tokens)
            {
                if (previousToken == null)
                    previousToken = token;
                else if (previousToken.Type == token.Type)
                {
                    yield return previousToken;
                    yield return token;
                }
                else
                    previousToken = token;
            }
        }
    }
}
