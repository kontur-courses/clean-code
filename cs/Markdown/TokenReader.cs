using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Models;

namespace Markdown
{
    public class TokenReader
    {
        private readonly string text;
        private readonly IEnumerable<IToken> tokens;

        private readonly List<TokenMatch> matches = new();
        private readonly List<IToken> tokensToOpen = new();
        private readonly List<TokenMatch> matchesToClose = new();

        public TokenReader(string text, IEnumerable<IToken> tokens)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"{nameof(text)} is null or empty.", nameof(text));

            this.tokens = tokens;
            this.text = text;
        }

        public IEnumerable<TokenMatch> FindAll()
        {
            var context = InitializeContext();

            for (var i = 0; i < text.Length; i++)
            {
                context.Index = i;

                var startingTokens = GetStartingTokens(context).ToList();
                if (startingTokens.Count != 0)
                    OpenMatches(context, startingTokens);
                else
                    CloseMatches(context);
            }

            return matches;
        }

        private Context InitializeContext()
        {
            matches.Clear();
            tokensToOpen.Clear();
            matchesToClose.Clear();
            tokensToOpen.AddRange(tokens);
            return new Context(text);
        }

        private IEnumerable<IToken> GetStartingTokens(Context context)
        {
            return tokensToOpen.Where(token => token.Pattern.IsStart(context));
        }

        private void OpenMatches(Context context, IEnumerable<IToken> startingTokens)
        {
            foreach (var token in startingTokens)
            {
                tokensToOpen.Remove(token);
                matchesToClose.Add(new TokenMatch {Start = context.Index, Token = token});
            }
        }

        private void CloseMatches(Context context)
        {
            matchesToClose
                .Where(match => match.Token.Pattern.IsEnd(context))
                .ToList()
                .ForEach(match =>
                {
                    match.Length = context.Index - match.Start + 1;
                    matches.Add(match);

                    matchesToClose.Remove(match);
                    tokensToOpen.Add(match.Token);
                });
        }
    }
}