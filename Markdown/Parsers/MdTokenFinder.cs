using System.Collections.Generic;
using System.Linq;
using AhoCorasick;

namespace Markdown
{
    internal class MdTokenFinder
    {
        private readonly Trie trie;
        private readonly TagRules rules;
        private readonly string shieldingSymbol;
        
        private readonly Dictionary<int, TokenInfo> tokenInfos = new();
        private int currentSearchStartIndex;
        private int lastIndex;
        private TokenInfo lastShieldToken;
        
        public MdTokenFinder(Trie trie, TagRules rules, string shieldingSymbol)
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

        private TokenInfo CreateTokenInfo(string text, int position, string token)
        {
            var closeValid = CanTokenOpenTag(text, position);
            var openValid = CanTokenCloseTag(text, position, token);

            return new TokenInfo(
                position,
                token, closeValid, openValid,
                closeValid && openValid,
                closeValid || openValid || shieldingSymbol == token || rules.IsInterruptTag(Tag.GetTagByChars(token))
            );
        }

        private bool Shielded(int position)
        {
            return shieldingSymbol is not null
                   && lastShieldToken is not null
                   && lastShieldToken.Position + shieldingSymbol.Length == position;
        }
        
        public TokenInfoCollection FindAllTokens(string text)
        {
            foreach (var (token, index) in trie.Find(text))
            {
                if (token is null || currentSearchStartIndex > index && lastIndex != index) continue;
                if (Shielded(index))
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
            
            return new TokenInfoCollection(tokenInfos.Select(x => x.Value));
        }
    }
}