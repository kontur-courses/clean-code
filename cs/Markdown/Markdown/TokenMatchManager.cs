using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenMatchManager
    {
        public IReadOnlyCollection<IToken> TokensToOpen => tokensToOpen.AsReadOnly();
        public IReadOnlyCollection<TokenMatch> MatchesToClose => matchesToClose.ToList().AsReadOnly();
        public IReadOnlyCollection<TokenMatch> Matches => matches.ToList().AsReadOnly();


        private readonly List<IToken> tokensToOpen;

        private readonly Stack<TokenMatch> matchesToClose = new();
        private readonly HashSet<TokenMatch> matches = new();
        private readonly Dictionary<TokenMatch, List<TokenMatch>> matchChildren = new();

        public TokenMatchManager(IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            tokensToOpen = tokens.ToList();
        }

        public void OpenMatch(IToken startingToken, int position)
        {
            tokensToOpen.Remove(startingToken);
            var match = new TokenMatch {Start = position, Token = startingToken};
            matchesToClose.Push(match);
            matchChildren[match] = new List<TokenMatch>();
        }

        public void CloseMatch(TokenMatch closingMatch, int position)
        {
            var lastOpened = matchesToClose.Peek();
            if (lastOpened.Token.TagType == closingMatch.Token.TagType && closingMatch.Token.Pattern.LastEndingSucceed)
                AddMatch(position);
            else
                BreakOpenedMatches(closingMatch);
        }

        private void AddMatch(int position)
        {
            var matchToClose = CreateMatchToClose(position);
            matches.Add(matchToClose);
            tokensToOpen.Add(matchToClose.Token);
            AddAsChildren(matchToClose);
            RemoveForbiddenChildren(matchToClose);
        }

        private TokenMatch CreateMatchToClose(int position)
        {
            var matchToClose = matchesToClose.Pop();
            matchToClose.Length = position
                                  - matchToClose.Start
                                  + matchToClose.Token.Pattern.EndTag.Length;

            return matchToClose;
        }

        private void AddAsChildren(TokenMatch matchToClose)
        {
            if (matchesToClose.TryPeek(out var parent))
                matchChildren[parent].Add(matchToClose);
        }

        private void RemoveForbiddenChildren(TokenMatch matchToClose)
        {
            var forbiddenChildren = matchChildren[matchToClose]
                .Where(child => matchToClose.Token.Pattern.ForbiddenChildren.Contains(child.Token.TagType));

            foreach (var child in forbiddenChildren)
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