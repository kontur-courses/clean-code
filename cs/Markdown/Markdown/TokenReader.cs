using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;

namespace Markdown
{
    public class TokenReader
    {
        private readonly TokenEscaper escaper;
        private readonly List<IToken> tokens;

        public TokenReader(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentException($"{nameof(tokens)} can't be null.", nameof(tokens));

            var tokensList = tokens.ToList();
            escaper = new TokenEscaper(tokensList);
            this.tokens = tokensList;
        }

        public IEnumerable<TokenMatch> FindAll(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var escapedText = escaper.EscapeTokens(text);
            var matches = GetMatches(escapedText.Text);
            return OffsetMatches(matches, escapedText);
        }

        private IEnumerable<TokenMatch> GetMatches(string text)
        {
            var manager = new TokenMatchManager(tokens);
            var context = new Context(text);

            for (var i = 0; i <= text.Length; i++)
            {
                context.Index = i;
                if (TryGetStartingToken(manager.TokensToOpen, context, out var startingToken))
                    manager.OpenMatch(startingToken, i);
                else if (TryGetClosingMatch(manager.MatchesToClose, context, out var closingMatch))
                    manager.CloseMatch(closingMatch, i);
            }

            return manager.Matches;
        }

        private static bool TryGetStartingToken(IEnumerable<IToken> tokensToOpen, Context context,
            out IToken startingToken)
        {
            var startingTokens = tokensToOpen
                .Where(token => token.Pattern.TrySetStart(context))
                .ToList();

            return IsSinglePatternSuited(startingTokens, out startingToken);
        }

        private static bool TryGetClosingMatch(IEnumerable<TokenMatch> matchesToClose, Context context,
            out TokenMatch closingMatch)
        {
            var closingMatches = matchesToClose
                .Where(match => !match.Token.Pattern.TryContinue(context))
                .ToList();

            return IsSinglePatternSuited(closingMatches, out closingMatch);
        }

        private static bool IsSinglePatternSuited<T>(IReadOnlyList<T> suitableElements, out T element)
        {
            switch (suitableElements.Count)
            {
                case > 1:
                    throw new ArgumentException("Tokens patterns are intersects.");
                case 0:
                    element = default;
                    return false;
                default:
                    element = suitableElements[0];
                    return true;
            }
        }

        private static IEnumerable<TokenMatch> OffsetMatches(IEnumerable<TokenMatch> matches, EscapedText escapedText)
        {
            return matches.Select(match =>
            {
                match.Length += escapedText.GetPositionOffset(match.Start + match.Length)
                                - escapedText.GetPositionOffset(match.Start);

                match.Start += escapedText.GetPositionOffset(match.Start);
                return match;
            });
        }
    }
}