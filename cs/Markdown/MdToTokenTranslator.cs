using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown
{
    public class MdToTokenTranslator : IStringTranslator
    {
        private readonly IReadOnlyDictionary<string, Func<int, IToken>> tokensByTags;

        public MdToTokenTranslator(IReadOnlyDictionary<string, Func<int, IToken>> tokensByTags)
        {
            this.tokensByTags = tokensByTags;
        }

        public IEnumerable<IToken> Translate(string markdown)
        {
            var tokens = new List<IToken>();
            var tokenBuilder = new TokenBuilder();
            var openedTokens = new HashSet<TokenType>();
            for (var i = 0; i < markdown.Length; i++)
            {
                var token = ChooseTokenBySymbol(tokenBuilder, markdown, i);
                i += token.Length - 1;
                token.SetIsOpening(tokenBuilder, markdown, openedTokens);
                token.Validate(markdown, tokens);
                if (token.ShouldBeIgnored)
                {
                    i += token.SkipLength;
                    tokenBuilder.SetPosition(i)
                        .Append(markdown[i]);
                    tokens.Add(tokenBuilder.Build());
                    tokenBuilder.Clear();
                }
                else 
                    tokens.Add(token);
            }

            var tokenToClose = tokens.FirstOrDefault(t => t.ShouldBeClosed);
            if (tokenToClose != null)
            {
                var token = tokensByTags[tokenToClose.Value](markdown.Length - 1);
                token.SetIsOpening(tokenBuilder, markdown, openedTokens);
                tokens.Add(token);
            }
            
            var unpairedTokens = new HashSet<IToken>();
            var pairedTokens = PairedToken.GetPairedTokens(tokens, unpairedTokens);
            tokens.RemoveAll(token => token.Length == 0);
            var forbiddenTokens = tokens.GetForbiddenTokens(pairedTokens, unpairedTokens, markdown);
            var resultTokens = SetSkipForTokens(tokens, forbiddenTokens).ToList();
            return resultTokens.OrderBy(token => token.Position);
        }

        private IToken ChooseTokenBySymbol(TokenBuilder tokenBuilder, string markdown, int position)
        {
            //if (tokenBuilder.CurrentValue != "")
                
            var tag = tokensByTags.Keys
                .Where(tag => markdown[position..].StartsWith(tag))
                .Max();
            if (tag != null)
                return tokensByTags[tag](position);
            var index = position + 1;
            while (index < markdown.Length
                   && !tokensByTags.Keys.Any(tag => tag.StartsWith(markdown[index])))
                index++;
            var token = tokenBuilder.SetPosition(position)
                .Append(markdown[position..index])
                .Build();
            tokenBuilder.Clear();
            return token;
        }

        private IEnumerable<IToken> SetSkipForTokens(IEnumerable<IToken> tokens, HashSet<IToken> tokensForSkip)
        {
            var tokenBuilder = new TokenBuilder();
            foreach (var token in tokens)
            {
                if (tokensForSkip.Contains(token))
                    yield return tokenBuilder.SetSettingsByToken(token)
                        .Build();
                else
                    yield return token;
            }
        }
    }
}
