using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;
using Microsoft.VisualBasic;

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
                token.SetIsOpening(markdown, openedTokens);
                token.Validate(markdown, tokens);
                if (token.ShouldBeIgnored)
                {
                    tokenBuilder.Append(markdown[i + token.SkipLength]);
                    i += token.SkipLength;
                }
                else 
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
            var tag = new StringBuilder();
            if (tokensByTags.Keys.Any(key => key.StartsWith(markdown[position])))
            {
                tag.Append(markdown[position]);
                for (var i = position + 1; i < markdown.Length; i++)
                {
                    if (tokensByTags.Keys.Any(key => key.StartsWith($"{tag}{markdown[i]}")))
                        tag.Append(markdown[i]);
                    else if (tokensByTags.TryGetValue(tag.ToString(), out var tokenGenerator))
                    {
                        var token = tokenGenerator(position);
                        return token;
                    }
                    else
                        tokenBuilder.Append(tag.ToString());
                }
                if (position + tag.Length == markdown.Length)
                    return tokensByTags[tag.ToString()](position);
            }
            else
            {
                tokenBuilder.SetPosition(position);
                tokenBuilder.Append(markdown[position]);
                for (var i = position + 1; i < markdown.Length; i++)
                {
                    if (tokensByTags.Keys.Any(key => key.StartsWith(markdown[i])))
                    {
                        var token = tokenBuilder.Build();
                        tokenBuilder.Clear();
                        return token;
                    }
                    tokenBuilder.Append(markdown[i]);
                }
            }
            return tokenBuilder.Build();
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
