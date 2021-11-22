using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;

namespace Markdown
{
    public class TokenReader
    {
        private readonly EscapedText escapedText;
        private readonly List<IToken> tokensToOpen;

        private readonly HashSet<TokenMatch> matches = new();
        private readonly Stack<TokenMatch> matchesToClose = new();
        private readonly Dictionary<TokenMatch, List<TokenMatch>> matchChildren = new();
        private int position;

        public TokenReader(string text, IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentException($"{nameof(tokens)} can't be null.", nameof(tokens));

            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"{nameof(text)} is null or empty.", nameof(text));

            var tokensList = tokens.ToList();
            tokensToOpen = tokensList;
            escapedText = new TokenEscaper(text, tokensList).EscapeTokens();
        }

        public IEnumerable<TokenMatch> FindAll()
        {
            for (; position <= escapedText.Text.Length; position++)
                ProceedSymbol();

            return matches;
        }

        private void ProceedSymbol()
        {
            if (TryGetStartingToken(out var startingToken))
                OpenMatch(startingToken);
            else if (TryGetClosingMatch(out var closingMatch))
                CloseMatch(closingMatch);
        }

        private bool TryGetStartingToken(out IToken startingToken)
        {
            var startingTokens = tokensToOpen
                .Where(token => token.Pattern.TrySetStart(new Context(escapedText.Text, position)))
                .ToList();

            return IsSinglePatternSuited(startingTokens, out startingToken);
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

        private void OpenMatch(IToken startingToken)
        {
            tokensToOpen.Remove(startingToken);
            matchesToClose.Push(new TokenMatch
                {Start = position + escapedText.EscapedSymbolsBefore[position], Token = startingToken});

            matchChildren[matchesToClose.Peek()] = new List<TokenMatch>();
        }

        private bool TryGetClosingMatch(out TokenMatch closingMatch)
        {
            var closingMatches = matchesToClose
                .Where(match => !match.Token.Pattern.TryContinue(new Context(escapedText.Text, position)))
                .ToList();

            return IsSinglePatternSuited(closingMatches, out closingMatch);
        }

        private void CloseMatch(TokenMatch closingMatch)
        {
            var lastOpened = matchesToClose.Peek();
            if (lastOpened.Token.TagType == closingMatch.Token.TagType && closingMatch.Token.Pattern.LastCloseSucceed)
                AddMatch();
            else
                BreakOpenedMatches(closingMatch);
        }

        private void AddMatch()
        {
            var matchToClose = CreateMatchToClose();
            matches.Add(matchToClose);
            tokensToOpen.Add(matchToClose.Token);
            AddAsChildren(matchToClose);
            RemoveForbiddenChildren(matchToClose);
        }

        private TokenMatch CreateMatchToClose()
        {
            var matchToClose = matchesToClose.Pop();
            var escapedSymbolsBefore = escapedText.EscapedSymbolsBefore.Count < position
                ? escapedText.EscapedSymbolsBefore[position]
                : escapedText.EscapedSymbolsBefore[position - 1];

            matchToClose.Length = position
                                  - matchToClose.Start
                                  + matchToClose.Token.Pattern.EndTag.Length
                                  + escapedSymbolsBefore;

            return matchToClose;
        }

        private void AddAsChildren(TokenMatch matchToClose)
        {
            if (matchesToClose.TryPeek(out var parent))
                matchChildren[parent].Add(matchToClose);
        }

        private void RemoveForbiddenChildren(TokenMatch matchToClose)
        {
            foreach (var child in matchChildren[matchToClose].Where(child =>
                matchToClose.Token.Pattern.ForbiddenChildren.Contains(child.Token.TagType)))
                matches.Remove(child);
        }

        private void BreakOpenedMatches(TokenMatch closingMatch)
        {
            while (true)
            {
                if (!matchesToClose.TryPop(out var lastOpened))
                    continue;

                tokensToOpen.Add(lastOpened.Token);
                if (lastOpened.Token.TagType == closingMatch.Token.TagType)
                    break;
            }
        }
    }
}