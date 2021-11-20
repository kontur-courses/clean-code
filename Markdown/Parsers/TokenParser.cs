using System.Collections.Generic;
using AhoCorasick;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        private readonly Trie<Token> trie = new();

        internal TokenParser()
        {
        }

        public void SetTokens(List<Token> tokensToSearch)
        {
            foreach (var token in tokensToSearch)
                trie.Add(token.ToString(), token);
            trie.Build();
        }

        public (IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) ValidatePairSets((IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) pair)
        {
            throw new System.NotImplementedException();
        }

        public string ReplaceTokens(IEnumerable<TokenSegment> tokenSegments, ITokenTranslator translator)
        {
            throw new System.NotImplementedException();
        }

        

        public Dictionary<int, TokenInfo> FindAllTokens(string paragraph)
        {
            var result = new Dictionary<int, TokenInfo>();

            foreach (var (token, index) in trie.Find(paragraph))
            {
                if (token is null) continue;
                
                var closeValid = index > 0 && !char.IsWhiteSpace(paragraph[index - 1]);
                var openValid = index < paragraph.Length - token.Length 
                                && !char.IsWhiteSpace(paragraph[index + token.Length]);
                
                result[index] = new TokenInfo(
                    token, closeValid, openValid,
                    closeValid && openValid,
                    closeValid || openValid
                );
            }
            
            return result;
        }
    }
}