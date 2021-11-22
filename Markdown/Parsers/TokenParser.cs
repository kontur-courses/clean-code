using System.Collections.Generic;
using System.Linq;
using System.Text;
using AhoCorasick;

namespace Markdown
{
    internal class TokenParser : ITokenParser
    {
        private readonly Trie<Token> trie;
        private readonly TagRules rules;

        internal TokenParser(Trie<Token> trie, TagRules rules)
        {
            this.trie = trie;
            this.rules = rules;
        }

        public (SegmentsCollection, SegmentsCollection) IgnoreSegmentsThatDoNotMatchRules(SegmentsCollection first, SegmentsCollection second)
        {
            var firstSorted = first.GetSortedSegments().ToList();
            var secondSorted = second.GetSortedSegments().ToList();
            
            var firstSegments = firstSorted
                .Where(x => !secondSorted.Any(y => y.IsIntersectWith(x)))
                .ToList();
            
            var secondSegment = secondSorted
                .Where(x => !firstSorted.Any(y => y.IsIntersectWith(x)))
                .ToList();

            return (new SegmentsCollection(firstSegments), new SegmentsCollection(secondSegment));
        }

        public string ReplaceTokens(string text, SegmentsCollection tokenSegments, ITagTranslator translator)
        {
            var result = new StringBuilder();
            var sortedSegments = tokenSegments.GetSortedSegments().ToList();
            var sortedTokens = sortedSegments
                .Select(x => new TokenInfo(x.StartPosition, x.GetBaseTag().Start, false, true, false, false))
                .Union(sortedSegments.Select(x => new TokenInfo(x.EndPosition, x.GetBaseTag().End, true, false, false, false)))
                .OrderBy(x => x.Position);

            var lastTokenEndIndex = 0;

            foreach (var (index, token, _, start, _, _) in sortedTokens)
            {
                result.Append(text.Substring(lastTokenEndIndex, index - lastTokenEndIndex));
                
                var translatedTag = translator.Translate(Tag.GetTagByChars(token.ToString()));
                result.Append(start ? translatedTag.Start : translatedTag.End);

                lastTokenEndIndex = index + token.Length;
            }

            result.Append(text[lastTokenEndIndex..]);
            
            return result.ToString();
        }

        public TokenInfoCollection FindAllTokens(string paragraph)
        {
            var tokenInfos = new Dictionary<int, TokenInfo>();

            foreach (var (token, index) in trie.Find(paragraph))
            {
                if (token is null) continue;
                
                var closeValid = index > 0 && !char.IsWhiteSpace(paragraph[index - 1]);
                var openValid = index < paragraph.Length - token.Length 
                                && !char.IsWhiteSpace(paragraph[index + token.Length]);

                tokenInfos[index] = new TokenInfo(
                    index,
                    token, closeValid, openValid,
                    closeValid && openValid,
                    closeValid || openValid
                );
            }
            
            return new TokenInfoCollection(tokenInfos.Select(x => x.Value));
        }
    }
}