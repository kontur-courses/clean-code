using System.Collections.Generic;
using System.Linq;
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

        //нужно убрать в другое место
        public (IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) ValidatePairSets((IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) pair)
        {
            // Реализация без учета правил пересечения и вложенности

            var firstSegments = pair.Item1
                .Where(x => pair.Item2.Any(y => y.IsIntersectWith(x)))
                .Where(x => pair.Item2.Any(y => y.Contain(x)));
            
            var secondSegment = pair.Item2
                .Where(x => pair.Item1.Any(y => y.IsIntersectWith(x)))
                .Where(x => pair.Item1.Any(y => y.Contain(x)));

            return (firstSegments, secondSegment);
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