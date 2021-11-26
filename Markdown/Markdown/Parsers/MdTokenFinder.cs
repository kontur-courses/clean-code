using System.Collections.Generic;
using System.Linq;
using AhoCorasick;

namespace Markdown
{
    internal class MdTokenFinder
    {
        private readonly Trie trie;
        private readonly TagRules rules;
        private readonly string? shieldingSymbol;
        
        private readonly Dictionary<int, TokenInfo> tokenInfos = new();
        private int currentSearchStartIndex;
        private int lastIndex;
        private TokenInfo? lastShieldToken;
        
        public MdTokenFinder(Trie trie, TagRules rules, string? shieldingSymbol)
        {
            this.shieldingSymbol = shieldingSymbol;
            this.trie = trie;
            this.rules = rules;
        }

        private static bool CanTokenOpenTag(string text, int position)
        {
            return position > 0 && !char.IsWhiteSpace(text[position - 1]);
        }
        
        private static bool CanTokenCloseTag(string text, int position, string token)
        {
            return position < text.Length - token.Length 
                   && !char.IsWhiteSpace(text[position + token.Length]);
        }

        private bool IsInWordToken(string text, int position, string token)
        {
            return CanTokenCloseTag(text, position, token) 
                   && CanTokenOpenTag(text, position) 
                   && text[position - 1].ToString() != shieldingSymbol 
                   && text[position + token.Length].ToString() != shieldingSymbol;
        }

        private bool IsValidToken(string text, int position, string token)
        {
            return CanTokenOpenTag(text, position) 
                   || CanTokenCloseTag(text, position, token) 
                   || shieldingSymbol == token 
                   || rules.IsInterruptTag(TagFactory.GetTagByChars(token));
        }

        private TokenInfo CreateTokenInfo(string text, int position, string token)
        {
            return new TokenInfo(
                position,
                token, CanTokenOpenTag(text, position), CanTokenCloseTag(text, position, token),
                IsInWordToken(text, position, token), IsValidToken(text, position, token)
            );
        }

        private bool Shielded(string token, int position)
        {
            return shieldingSymbol is not null
                   && lastShieldToken is not null
                   && lastShieldToken.Position + shieldingSymbol.Length == position
                   && rules.CanBeShielded(TagFactory.GetTagByChars(token));
        }
        
        public TokenInfoCollection FindAllTokens(string text)
        {
            foreach (var (token, index) in trie.Find(text))
            {
                if (token is null || currentSearchStartIndex > index && lastIndex != index) continue;
                if (Shielded(token, index))
                {
                    lastShieldToken = null;
                    continue;
                }

                if (lastShieldToken is not null)
                    lastShieldToken.Valid = false;

                var tokenInfo = CreateTokenInfo(text, index, token);
                tokenInfos[lastIndex = index] = tokenInfo;
                currentSearchStartIndex = index + token.Length;

                if (shieldingSymbol == token)
                    lastShieldToken = tokenInfo;
            }
            
            if (lastShieldToken is not null)
                lastShieldToken.Valid = false;
            
            return new TokenInfoCollection(tokenInfos.Select(x => x.Value));
        }
    }
}