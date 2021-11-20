using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;

namespace Markdown
{
    public class TokenReader
    {
        private readonly IEnumerable<IToken> tokens;

        private readonly HashSet<TokenMatch> matches = new();
        private readonly List<IToken> tokensToOpen = new();
        private readonly Stack<TokenMatch> matchesToClose = new();
        private readonly Dictionary<TokenMatch, List<TokenMatch>> matchChildren = new();
        private readonly EscapedText escapedText;

        public TokenReader(string originalText, IEnumerable<IToken> tokens)
        {
            if (string.IsNullOrEmpty(originalText))
                throw new ArgumentException($"{nameof(originalText)} is null or empty.", nameof(originalText));

            this.tokens = tokens.ToList();
            // escapedText = EscapedText.RemoveEscapedTokens(originalText, this.tokens);
            escapedText = new TokenEscaper(originalText, this.tokens).EscapeTokens();
        }

        public IEnumerable<TokenMatch> FindAll()
        {
            var context = InitializeContext(escapedText.Text);

            for (var i = 0; i <= context.Text.Length; i++)
                ProceedSymbol(context, i);

            return matches;
        }


        private Context InitializeContext(string text)
        {
            matches.Clear();
            tokensToOpen.Clear();
            matchesToClose.Clear();
            tokensToOpen.AddRange(tokens);
            return new Context(text);
        }

        private void ProceedSymbol(Context context, int i)
        {
            context.Index = i;

            if (TryGetStartingToken(context, out var startingToken))
                OpenMatch(context, startingToken);
            else if (TryGetClosingMatch(context, out var closingMatch))
                CloseMatch(context, closingMatch);
        }

        private bool TryGetStartingToken(Context context, out IToken startingToken)
        {
            var startingTokens = tokensToOpen
                .Where(token => token.Pattern.TrySetStart(context))
                .ToList();

            switch (startingTokens.Count)
            {
                case > 1:
                    throw new ArgumentException("Tokens pattern start intersects.");
                case 0:
                    startingToken = null;
                    return false;
                default:
                    startingToken = startingTokens[0];
                    return true;
            }
        }

        private void OpenMatch(Context context, IToken startingToken)
        {
            tokensToOpen.Remove(startingToken);
            matchesToClose.Push(new TokenMatch
                {Start = context.Index + escapedText.EscapedSymbolsBefore[context.Index], Token = startingToken});

            matchChildren[matchesToClose.Peek()] = new List<TokenMatch>();
        }

        private bool TryGetClosingMatch(Context context, out TokenMatch closingMatch)
        {
            var closingMatches = matchesToClose
                .Where(match => !match.Token.Pattern.TryContinue(context))
                .ToList();

            switch (closingMatches.Count)
            {
                case > 1:
                    throw new ArgumentException("Tokens pattern end intersects.");
                case 0:
                    closingMatch = null;
                    return false;
                default:
                    closingMatch = closingMatches[0];
                    return true;
            }
        }

        private void CloseMatch(Context context, TokenMatch closingMatch)
        {
            var lastOpened = matchesToClose.Peek();
            if (lastOpened.Token.TagType == closingMatch.Token.TagType && closingMatch.Token.Pattern.LastCloseSucceed)
                AddMatch(context);
            else
                BreakOpenedMatches(closingMatch);
        }

        private void AddMatch(Context context)
        {
            var matchToClose = matchesToClose.Pop();
            var escapedSymbolsBefore = escapedText.EscapedSymbolsBefore.Count < context.Index
                ? escapedText.EscapedSymbolsBefore[context.Index]
                : escapedText.EscapedSymbolsBefore[context.Index - 1];

            matchToClose.Length = context.Index
                                  - matchToClose.Start
                                  + matchToClose.Token.Pattern.EndTag.Length
                                  + escapedSymbolsBefore;

            matches.Add(matchToClose);
            tokensToOpen.Add(matchToClose.Token);

            if (matchesToClose.TryPeek(out var parent))
                matchChildren[parent].Add(matchToClose);

            foreach (var child in matchChildren[matchToClose].Where(child =>
                matchToClose.Token.Pattern.ForbiddenChildren.Contains(child.Token.TagType)))
                matches.Remove(child);
        }

        private void BreakOpenedMatches(TokenMatch closingMatch)
        {
            while (true)
            {
                var lastOpened = matchesToClose.Pop();
                tokensToOpen.Add(lastOpened.Token);
                if (lastOpened.Token.TagType == closingMatch.Token.TagType || matchesToClose.Count == 0)
                    break;
            }
        }
    }
}